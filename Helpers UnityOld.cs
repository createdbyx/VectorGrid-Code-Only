namespace Codefarts.VectorGrid
{
    using UnityEngine;

    public partial class Helpers
    {
#if  UNITY_5 && !UNITY_5_3_OR_NEWER && !UNITY_2017_1_OR_NEWER && !UNITY_2018_1_OR_NEWER
        public static Material CreateLineMaterial(Material material)
        {
            if (material == null)
            {
                material = new Material("Shader \"Lines/Colored Blended\" {" +
                                                 "SubShader { Pass { " +
                                                 "    Blend SrcAlpha OneMinusSrcAlpha " +
                                                 "    ZWrite Off Cull Off Fog { Mode Off } " +
                                                 "    BindChannels {" +
                                                 "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                                 "} } }");
                material.hideFlags = HideFlags.HideAndDontSave;
                material.shader.hideFlags = HideFlags.HideAndDontSave;
            }

            return material;
        } 
#endif 
    }
}