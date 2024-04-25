using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private int m_GridWidth = 5;
    [SerializeField]
    private int m_GridHeight = 3;
    [SerializeField]
    private int m_GridDepth = 5;
    
    [SerializeField]
    private float m_WaitTime = 1f;
    
    [SerializeField] 
    List<Tile> m_Tiles;
    [SerializeField]
    private List<Tile> m_NoPossibilityTiles;
    
    private int[] m_TopTileIndexes;
    private int[] m_GroundableTileIndexes;
    
    private SuperPosition[,,] _grid;
    
    private const int MAX_TRIES = 10;
    
    private void Start()
    {
        m_TopTileIndexes = m_Tiles.FindAll(tile => tile.IsCeiling).ConvertAll(tile => m_Tiles.IndexOf(tile)).ToArray();
        m_GroundableTileIndexes = m_Tiles.FindAll(tile => tile.OnGround).ConvertAll(tile => m_Tiles.IndexOf(tile)).ToArray();
        
        int tries = 0;
        bool result;

        do
        {
            tries++;
            result = RunWfc();
        }
        while (result == false && tries < MAX_TRIES);

        if (result == false)
        {
            print("Unable to solve wave function collapse after " + tries + " tries.");
        }
        else
        {
            StartCoroutine(DrawTiles());
        }
    }

    private bool RunWfc()
    {
        InitGrid();

        var count = 0;

        while (DoUnobservedNodesExist())
        {
            Vector3Int node = GetNextUnobservedNode();
            if (node.x == -1)
            {
                return false; 
            }

            int observedValue = Observe(node);
            count++;
            PropagateNeighbors(node, observedValue);
        }

        return true; 
    }

    private IEnumerator DrawTiles() {
        
        var randomX = Randomizer(m_GridWidth);
        var randomZ = Randomizer(m_GridDepth);
        
        for (int x = 0; x < m_GridWidth; x++)
        {
            for (int z = 0; z < m_GridDepth; z++)
            {
                for (int y = 0; y < m_GridHeight; y++)
                {
                    var value = _grid[randomX[x], y, randomZ[z]].GetObservedValue();
                    var tileSet = value < 0 ? m_NoPossibilityTiles[Random.Range(0, m_NoPossibilityTiles.Count)] : m_Tiles[value];
                    GameObject tile = GameObject.Instantiate(tileSet.gameObject);
                    tile.transform.position = tile.transform.position 
                        + new Vector3(randomX[x], y, randomZ[z]) - new Vector3((m_GridWidth-1)/2f, (m_GridHeight-1)/2f, (m_GridDepth-1)/2f);
                    yield return new WaitForSeconds(m_WaitTime);
                }
            }
        }
    }
    
    private List<int> Randomizer(int count)
    {
        List<int> randomList = new List<int>();
        for (int i = 0; i < count; i++)
        {
            randomList.Add(i);
        }
        for (int i = 0; i < count; i++)
        {
            int temp = randomList[i];
            int randomIndex = Random.Range(i, count);
            randomList[i] = randomList[randomIndex];
            randomList[randomIndex] = temp;
        }
        return randomList;
    }

    bool DoUnobservedNodesExist()
    {
        for (int x = 0; x < m_GridWidth; x++)
        {
            for (int y = 0; y < m_GridHeight; y++)
            {
                for (int z = 0; z < m_GridDepth; z++)
                {
                    if (_grid[x, y, z].IsObserved() == false) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    int Observe(Vector3Int node)
    {
        //weird way of checking if this is the toppest tile we gonna give an index of a random ceiling tile
        var ceilingIndex = node.y == m_GridHeight -1 ? m_TopTileIndexes[Random.Range(0, m_TopTileIndexes.Length)] : -999;
        return _grid[node.x, node.y, node.z].Observe(ceilingIndex);
    }

    private void InitGrid()
    {
        _grid = new SuperPosition[m_GridWidth, m_GridHeight, m_GridDepth];

        for (int x = 0; x < m_GridWidth; x++)
        {
            for (int y = 0; y < m_GridHeight; y++)
            {
                for (int z = 0; z < m_GridDepth; z++)
                {
                    if (y == 0)
                    {
                        _grid[x, y, z] = new SuperPosition(m_GroundableTileIndexes);
                    }
                    else
                        _grid[x, y, z] = new SuperPosition(m_Tiles.Count);
                }
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    void PropagateNeighbors(Vector3Int node, int observedValue)
    {
        //if the index is -1 that means that tile has no options left anymore
        var selectedTile = observedValue < 0 ? 
            m_NoPossibilityTiles[Random.Range(0, m_NoPossibilityTiles.Count)] : m_Tiles[observedValue];
        
        // // Propagate to neighboring nodes in all six cardinal directions in 3D space
        PropagateTo(node, new Vector3Int(-1, 0, 0), selectedTile); // Left
        PropagateTo(node, new Vector3Int(1, 0, 0), selectedTile);  // Right
        PropagateTo(node, new Vector3Int(0, -1, 0), selectedTile); // Down
        PropagateTo(node, new Vector3Int(0, 1, 0), selectedTile);  // Up
        PropagateTo(node, new Vector3Int(0, 0, -1), selectedTile); // Back
        PropagateTo(node, new Vector3Int(0, 0, 1), selectedTile);  // Forward
        
    }

    void PropagateTo(Vector3Int node, Vector3Int direction, Tile nodesSelectedOption)
    {
        // Calculate the neighbor's position
        Vector3Int neighbor = node + direction;
        
        // Out of Bounds Check
        if (neighbor.x < 0 || neighbor.x >= m_GridWidth || neighbor.y < 0 || neighbor.y >= m_GridHeight
            || neighbor.z < 0 || neighbor.z >= m_GridDepth) return;

        for (int i = 0; i < m_Tiles.Count; i++)
        {
            if (direction == Vector3Int.up)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.UpFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
            else if (direction == Vector3Int.down)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.DownFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
            else if (direction == Vector3Int.right)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.RightFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
            else if (direction == Vector3Int.left)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.LeftFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
            else if (direction == Vector3Int.forward)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.ForwardFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
            else if (direction == Vector3Int.back)
            {
                if (!m_Tiles[i].CanFit(nodesSelectedOption.BackFits))
                    _grid[neighbor.x, neighbor.y, neighbor.z].RemovePossibleValue(i);
            }
        }
    }

    Vector3Int GetNextUnobservedNode()
    {
        //return the coordinates of the unobserved node with the fewest possible options
        var lastMinPos = int.MaxValue;
        
        var dict = new Dictionary<int, List<Vector3Int>>();
        
        for (int x = 0; x < m_GridWidth; x++)
        {
            for (int y = 0; y < m_GridHeight; y++)
            {
                for (int z = 0; z < m_GridDepth; z++)
                {
                    if (!_grid[x, y, z].IsObserved())
                    {
                        int numOptions = _grid[x, y, z].NumOptions;
                        
                        if(dict.ContainsKey(numOptions))
                            dict[numOptions].Add(new Vector3Int(x, y, z));
                        else
                            dict.Add(numOptions, new List<Vector3Int>{new(x, y, z)});
                        if(numOptions < lastMinPos)
                            lastMinPos = numOptions;
                    }
                }
            }
        }

        var list = dict[lastMinPos];
        return list[Random.Range(0, list.Count)];
        
    }
}
