using Unity.VisualScripting;
using UnityEngine;

namespace Utility.Extensions
{
    public static class MaterialExtensions
    {
        public static Material GetSetClonedMat(this Renderer rend, int sharedIndex)
        {
            var materials = rend.materials;
            var mat = new Material(materials[sharedIndex]);
            rend.materials[sharedIndex] = mat;
            return rend.materials[sharedIndex];
        }
    }
}
