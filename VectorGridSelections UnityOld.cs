namespace Codefarts.VectorGrid
{
    public partial class VectorGridSelections
    {
#if  UNITY_5 && !UNITY_5_3_OR_NEWER && !UNITY_2017_1_OR_NEWER && !UNITY_2018_1_OR_NEWER
        private void CreateLineMaterial()
        {
            if (!this.gridMaterial)
            {
                this.gridMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                                 "SubShader { Pass { " +
                                                 "    Blend SrcAlpha OneMinusSrcAlpha " +
                                                 "    ZWrite Off Cull Off Fog { Mode Off } " +
                                                 "    BindChannels {" +
                                                 "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                                 "} } }");
                this.gridMaterial.hideFlags = HideFlags.HideAndDontSave;
                this.gridMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        }
#endif
    }
}