using System.Linq;
using Roro.Scripts.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public GameObject targetObject;

    public bool Layered = true;

    public bool Materials = false;
    //public GameObject tag;

#if UNITY_EDITOR
    
    
    [Button]
    public void Clone()
    {
        var targetComponents = targetObject.GetComponents(typeof(Component));

        targetComponents.ForEach((component =>
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(component);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(this.gameObject);
        }));
        
        if(Layered)
            targetObject.layer = this.gameObject.layer;

        if (Materials)
        {
            if (targetObject.TryGetComponentInChildren<Renderer>(out var rendTarget))
            {
                if(this.gameObject.TryGetComponentInChildren<Renderer>(out var rendThis))
                {
                    rendThis.sharedMaterials = rendTarget.sharedMaterials;
                }
            }
        }

    }

    // [Button]
    // public void AddChild()
    // {
    //     PrefabUtility.InstantiatePrefab(tag.gameObject, this.transform);
    //
    //     transform.GetChild(0).transform.localPosition = transform.up;
    //     transform.GetChild(0).transform.localScale = Vector3.one * .2f;
    //     transform.GetChild(0).transform.localRotation = Quaternion.Euler(Vector3.forward * 180f);
    //     transform.GetChild(0).GetComponentInChildren<TextMesh>().text = this.gameObject.name.ToUpper();
    // }
    
    // [Button]
    // public void Done()
    // {
    //     DestroyImmediate(this);
    // }
    //
    // [MenuItem("Tools/MakePrefab")]
    // public static void MakePrefab()
    // {
    //     foreach (var o in Selection.objects.Select(obj => obj as GameObject))
    //     {
    //         PrefabUtility.SaveAsPrefabAsset(o, $"Assets/Prefabs/NewObjects/{o.name}.prefab");
    //     }
    //     
    //     AssetDatabase.SaveAssets();
    //
    // }
    
#endif
}
