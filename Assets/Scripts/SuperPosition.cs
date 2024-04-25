using System.Collections.Generic;
using UnityEngine;

public class SuperPosition
{
    private readonly List<int> m_PossibleValues = new();
    private bool m_Observed = false;
    public int NumOptions => m_PossibleValues.Count;
    
    public bool IsObserved() => m_Observed;

    public SuperPosition(int maxValue)
    {
        for (int i = 0; i < maxValue; i++)
        {
            m_PossibleValues.Add(i);
        }
    }
    public SuperPosition(int[] customVals)
    {
        foreach (var t in customVals)
        {
            m_PossibleValues.Add(t);
        }
    }

    public int GetObservedValue()
    {
        if (NumOptions > 0)
        {
            return m_PossibleValues[0];
        }

        throw new System.InvalidOperationException("NO" + m_Observed);
    }

    public int Observe(int ceilingIndex = -999)
    {
        if (m_Observed) 
            return GetObservedValue();
        
        
        if (NumOptions == 0)
        {
            throw new System.InvalidOperationException("SAD");
        }
            
        //weird way of if it is the ceiling location and this position still has ceilings as options, it will put
        //other possibility is not having ceiling anymore (already empty) in that case we just skip
        if(m_PossibleValues.Contains(ceilingIndex))
        {
            m_PossibleValues.Clear();
            m_PossibleValues.Add(ceilingIndex);
            m_Observed = true;
            return ceilingIndex;
        }
            
        int observedValue = m_PossibleValues[Random.Range(0,  m_PossibleValues.Count)];
        
        m_PossibleValues.Clear();
        m_PossibleValues.Add(observedValue);
        
        m_Observed = true;
        
        return observedValue;

    }


    public void RemovePossibleValue(int value)
    {
        //No need to remove if it is already observed
        if (!m_PossibleValues.Contains(value) || m_Observed)
        {
            return;
        }
        
        m_PossibleValues.Remove(value);

        if (NumOptions != 0) 
            return;
        
        //No possibility left for this tile, will have NO POSSIBILITY TILE INDEX AS -1
        Debug.Log("No Possibility Left :(");
        
        m_PossibleValues.Clear();
        m_PossibleValues.Add(-1);
    }
}