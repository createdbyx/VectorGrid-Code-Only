 namespace Codefarts.VectorGrid
{
    public partial class Guides
    {
#if  UNITY_5 && !UNITY_5_3_OR_NEWER && !UNITY_2017_1_OR_NEWER && !UNITY_2018_1_OR_NEWER
        private void CreateLineMaterial()
        {
            if (!this.lineMaterial)
            {
                this.lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                                 "SubShader { Pass { " +
                                                 "    Blend SrcAlpha OneMinusSrcAlpha " +
                                                 "    ZWrite Off Cull Off Fog { Mode Off } " +
                                                 "    BindChannels {" +
                                                 "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                                 "} } }");
                this.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                this.lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        } 
#endif
    }
}