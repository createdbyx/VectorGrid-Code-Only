namespace Codefarts.VectorGrid
{
    public partial class VectorGrid
    {
#if  UNITY_5 && !UNITY_5_3_OR_NEWER && !UNITY_2017_1_OR_NEWER && !UNITY_2018_1_OR_NEWER
        private void CreateLineMaterial()
        {
            if (this.mainMaterial == null)
            {
                this.mainMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                                 "SubShader { Pass { " +
                                                 "    Blend SrcAlpha OneMinusSrcAlpha " +
                                                 "    ZWrite Off Cull Off Fog { Mode Off } " +
                                                 "    BindChannels {" +
                                                 "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                                 "} } }");
                this.mainMaterial.hideFlags = HideFlags.HideAndDontSave;
                this.mainMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        } 
#endif
    }
}