namespace Codefarts.VectorGrid
{
    using UnityEngine;

#if UNITY_5_3_OR_NEWER
    public partial class Guides
    {
        /// <summary>
        /// Creates the material for drawing lines.
        /// </summary>
        private void CreateLineMaterial()
        {
            var material = this.lineMaterial;
            if (!material)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                material = new Material(shader);
                material.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                material.SetInt("_ZWrite", 0);
                this.lineMaterial = material;
            }
        }
    }
#endif
}