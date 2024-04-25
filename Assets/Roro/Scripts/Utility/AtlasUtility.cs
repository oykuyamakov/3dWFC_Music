#if UNITY_EDITOR

using System.Linq;
using UnityCommon.Editor.Utility;
using UnityCommon.Runtime.Extensions;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Based.Utility
{
    public static class AtlasUtility 
    {
        [MenuItem("Tools/Update Atlases")]
        public static void UpdateAtlases()
        {
            var allAtlases = AssetDatabaseHelpers.LoadAll<SpriteAtlas>();
            var allAtlasDefinitions = AssetDatabaseHelpers.LoadAll<AtlasDefinition>();

            foreach (var atlasDefinition in allAtlasDefinitions.Where(def => !def.BoundAtlas))
            {
                Debug.Log($"Atlas definition {atlasDefinition.name} has no bound atlas.");
            }

            var groups = allAtlasDefinitions.Where(def => def.BoundAtlas).GroupBy(def => def.BoundAtlas);

            foreach (IGrouping<SpriteAtlas, AtlasDefinition> group in groups)
            {
                var atlas = group.Key;

                foreach (var atlasDefinition in group)
                {
                    atlasDefinition.Sprites = atlasDefinition.Sprites.Distinct().ArgDistinct(AssetDatabase.GetAssetPath)
                        .ToList();
                    EditorUtility.SetDirty(atlasDefinition);
                }

                var sprites = group.SelectMany(def => def.Sprites).Distinct().ArgDistinct(AssetDatabase.GetAssetPath)
                    .Select(sprite => sprite as UnityEngine.Object).ToArray();

                atlas.Remove(atlas.GetPackables());

                atlas.Add(sprites);

                EditorUtility.SetDirty(atlas);
            }


            AssetDatabase.SaveAssets();
        }
    }
}
#endif
