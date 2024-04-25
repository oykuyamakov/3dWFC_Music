using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace Based.Utility
{
    [CreateAssetMenu]
    public class AtlasDefinition : ScriptableObject
    {
        public SpriteAtlas BoundAtlas;
        public bool CleanFirst = true;

        [ListDrawerSettings(NumberOfItemsPerPage = 30)]
        public List<Sprite> Sprites = new List<Sprite>();
    }
}
