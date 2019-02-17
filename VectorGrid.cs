namespace Codefarts.GeneralTools.Scripts
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GeneralTools.Code;
    using Codefarts.GeneralTools.TypeVisualizers.Unity;

#if PERFORMANCE
    using Codefarts.PerformanceTesting;
#endif

    using UnityEngine;
    using UnityEngine.Rendering;

    [ExecuteInEditMode]
    public class VectorGrid : MonoBehaviour
    {
        /// <summary>
        /// The backing field for the <see cref="ShowMain"/> property.
        /// </summary>
        [SerializeField]
        private bool showMain = true;

        /// <summary>
        /// The backing field for the <see cref="ShowSub"/> property.
        /// </summary>
        [SerializeField]
        private bool showSub = false;

        /// <summary>
        /// The backing field for the <see cref="UseMesh"/> property.
        /// </summary>
        [SerializeField]
        private bool useMesh = false;

        /// <summary>
        /// The backing field for the <see cref="Width"/> property.
        /// </summary>
        [SerializeField]
        private float width = 10;

        /// <summary>
        /// The backing field for the <see cref="Depth"/> property.
        /// </summary>
        [SerializeField]
        private float depth;

        /// <summary>
        /// The backing field for the <see cref="Height"/> property.
        /// </summary>
        [SerializeField]
        private float height = 10;

        /// <summary>
        /// The backing field for the <see cref="Offset"/> property.
        /// </summary>
        [SerializeField]
        private Vector3 offset;

        /// <summary>
        /// The backing field for the <see cref="SmallStep"/> property.
        /// </summary>
        [SerializeField]
        private float smallStep = 4;

        /// <summary>
        /// The backing field for the <see cref="LargeStep"/> property.
        /// </summary>
        [SerializeField]
        private float largeStep = 16;

        /// <summary>
        /// The backing field for the <see cref="Origin"/> property.
        /// </summary>
        [SerializeField]
        private Transform origin;

        /// <summary>
        /// The backing field for the <see cref="GridMaterial"/> property.
        /// </summary>
        [SerializeField]
        private Material gridMaterial;

        /// <summary>
        /// The backing field for the <see cref="MainColor"/> property.
        /// </summary>
        [SerializeField]
        private Color mainColor = new Color(0f, 1f, 0f, 1f);

        /// <summary>
        /// The backing field for the <see cref="SubColor"/> property.
        /// </summary>
        [SerializeField]
        private Color subColor = new Color(0f, 0.5f, 0f, 1f);

        private Mesh mesh;

        private bool changed;

        /// <summary>
        /// Occurs before drawing grid.
        /// </summary>
        public event EventHandler BeforeDrawGrid;

        /// <summary>
        /// Gets or sets a value indicating whether the main grid lines are shown.
        /// </summary>                                                           
        public bool ShowMain
        {
            get
            {
                return this.showMain;
            }

            set
            {
                this.showMain = value;
            }
        }

        private void UpdateMesh()
        {
            this.mesh = this.mesh == null ? new Mesh() : this.mesh;

            var callback = new Func<float, Vector3[]>(step =>
                {
                    var vectors = new List<Vector3>();

                    // layers
                    for (float j = 0; j <= this.depth; j += step)
                    {
                        //X axis lines
                        for (float i = 0; i <= this.height; i += step)
                        {
                            vectors.Add(new Vector3(0 + this.offset.x, j + this.offset.y, i + this.offset.z));
                            vectors.Add(new Vector3(this.width + this.offset.x, j + this.offset.y, i + this.offset.z));
                        }

                        //Z axis lines
                        for (float i = 0; i <= this.width; i += step)
                        {
                            vectors.Add(new Vector3(i + this.offset.x, j + this.offset.y, 0 + this.offset.z));
                            vectors.Add(new Vector3(i + this.offset.x, j + this.offset.y, this.height + this.offset.z));
                        }
                    }

                    // Y axis lines
                    for (float i = 0; i <= this.height; i += step)
                    {
                        for (float k = 0; k <= this.width; k += step)
                        {
                            vectors.Add(new Vector3(k + this.offset.x, 0 + this.offset.y, i + this.offset.z));
                            vectors.Add(new Vector3(k + this.offset.x, this.depth + this.offset.y, i + this.offset.z));
                        }
                    }

                    return vectors.ToArray();
                });

            // vertexes
            var mainVertexes = callback(this.LargeStep);
            var subVertexes = callback(this.SmallStep);
            var vertexes = new Vector3[subVertexes.Length + mainVertexes.Length];
            subVertexes.CopyTo(vertexes, 0);
            mainVertexes.CopyTo(vertexes, subVertexes.Length);

            // vertex colors
            var colors = new Color[vertexes.Length];
            for (var i = 0; i < subVertexes.Length; i++)
            {
                colors[i] = this.SubColor;
            }

            for (var i = 0; i < mainVertexes.Length; i++)
            {
                colors[i + subVertexes.Length] = this.MainColor;
            }

            // indicies
            var subIndicies = new int[subVertexes.Length];
            var mainIndicies = new int[mainVertexes.Length];

            for (var i = 0; i < subIndicies.Length; i++)
            {
                subIndicies[i] = i;
            }

            for (var i = 0; i < mainIndicies.Length; i++)
            {
                mainIndicies[i] = i;
            }

            // build mesh
            this.mesh.Clear();
            this.mesh.vertices = vertexes;
            this.mesh.colors = colors;
            this.mesh.subMeshCount = 2;

            // set indecies
            this.mesh.SetIndices(subIndicies, MeshTopology.Lines, 0);
            this.mesh.SetIndices(mainIndicies, MeshTopology.Lines, 1);

            // update data data
            this.mesh.UploadMeshData(false);
        }

        //private void UpdateMesh()
        //{
        //    this.mesh = this.mesh == null ? new Mesh() : this.mesh;

        //    var callback = new Func<float, Vector3[]>(step =>
        //        {
        //            var w = (int)(this.width / step);
        //            var h = (int)(this.height / step);
        //            var d = (int)(this.depth / step);

        //            var vectors = new Vector3[w * h * d];
        //            var index = 0;

        //            for (float z = 0; z <= this.depth; z += this.depth)
        //            {
        //                for (float y = 0; y <= this.height; y += this.height)
        //                {
        //                    // x line
        //                    vectors[index++] = new Vector3(0 + this.offset.x, z + this.offset.y, y + this.offset.z);
        //                    vectors[index++] = new Vector3(this.width + this.offset.x, z + this.offset.y, y + this.offset.z);
        //                }
        //            }

        //            return vectors;
        //        });

        //    var indiciesCallback = new Func<int[]>(step =>
        //    {
        //        var w = (int)(this.width / step);
        //        var h = (int)(this.height / step);
        //        var d = (int)(this.depth / step);

        //        var indicies = new int[w * h * d * 2];
        //        var xIndex = 0;
        //        var yIndex = 0;
        //        var zIndex = 0;
        //        var index = 0;

        //        //for (float y = 0; y <= this.height; y += step)
        //        {
        //            for (float x = 0; x <= this.width; x += step)
        //            {
        //                // x line
        //                indicies[index++] = xIndex;
        //                xIndex += (w * h) - w;
        //                indicies[index++] = xIndex;
        //            }
        //        }

        //        return indicies;
        //    });

        //    var mainVertexes = callback(this.LargeStep);
        //    var subVertexes = callback(this.SmallStep);
        //    var vertexes = new Vector3[subVertexes.Length + mainVertexes.Length];
        //    subVertexes.CopyTo(vertexes, 0);
        //    mainVertexes.CopyTo(vertexes, subVertexes.Length);

        //    this.mesh.Clear();
        //    this.mesh.vertices = vertexes;
        //    var subIndicies = new int[subVertexes.Length];
        //    var mainIndicies = new int[mainVertexes.Length];

        //    var colors = new Color[vertexes.Length];
        //    for (var i = 0; i < subIndicies.Length; i++)
        //    {
        //        subIndicies[i] = i;
        //        colors[i] = this.SubColor;
        //    }

        //    for (var i = 0; i < mainIndicies.Length; i++)
        //    {
        //        mainIndicies[i] = i;
        //        colors[i + subVertexes.Length] = this.MainColor;
        //    }

        //    this.mesh.colors = colors;

        //    this.mesh.subMeshCount = 2;
        //    this.mesh.SetIndices(subIndicies, MeshTopology.Lines, 0);
        //    this.mesh.SetIndices(mainIndicies, MeshTopology.Lines, 1);
        //    this.mesh.UploadMeshData(false);
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the sub grid lines are shown.
        /// </summary>          
        public bool ShowSub
        {
            get
            {
                return this.showSub;
            }

            set
            {
                this.showSub = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the main grid lines.
        /// </summary>
        public Color MainColor
        {
            get
            {
                return this.mainColor;
            }

            set
            {
                this.changed = this.changed || this.mainColor != value;
                this.mainColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the sub grid lines.
        /// </summary>
        public Color SubColor
        {
            get
            {
                return this.subColor;
            }

            set
            {
                this.changed = this.changed || this.subColor != value;
                this.subColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the grid material used to render the grid lines.
        /// </summary>
        /// <remarks>Setting this to null will create a internal line drawing material.</remarks>
        public Material GridMaterial
        {
            get
            {
                return this.gridMaterial;
            }

            set
            {
                this.gridMaterial = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Transform"/> that will be used as the origin. 
        /// </summary>
        public Transform Origin
        {
            get
            {
                return this.origin;
            }

            set
            {
                this.origin = value;
            }
        }

        /// <summary>
        /// Gets or sets the large grid step.
        /// </summary>
        public float LargeStep
        {
            get
            {
                return this.largeStep;
            }

            set
            {
                this.changed = this.changed || Math.Abs(this.largeStep - value) > float.Epsilon;
                this.largeStep = Math.Max(0.1f, value);
            }
        }

        /// <summary>
        /// Gets or sets the small grid step.
        /// </summary>
        public float SmallStep
        {
            get
            {
                return this.smallStep;
            }

            set
            {
                this.changed = this.changed || Math.Abs(this.smallStep - value) > float.Epsilon;
                this.smallStep = Math.Max(0.1f, value);
            }
        }

        /// <summary>
        /// The backing field for the <see cref="UseMesh"/> property.
        /// </summary>
        public bool UseMesh
        {
            get
            {
                return this.useMesh;
            }

            set
            {
                this.changed = this.changed || this.useMesh != value;
                this.useMesh = value;
            }
        }

        /// <summary>
        /// Gets or sets the grid width.
        /// </summary>
        public float Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.changed = this.changed || Math.Abs(this.width - value) > float.Epsilon;
                this.width = value;
            }
        }

        /// <summary>
        /// Gets or sets the grid depth.
        /// </summary>
        public float Depth
        {
            get
            {
                return this.depth;
            }

            set
            {
                this.changed = this.changed || Math.Abs(this.depth - value) > float.Epsilon;
                this.depth = value;
            }
        }

        /// <summary>
        /// Gets or sets the grid height.
        /// </summary>
        public float Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.changed = this.changed || Math.Abs(this.height - value) > float.Epsilon;
                this.height = value;
            }
        }

        /// <summary>
        /// Gets or sets the offset that will be applied to the grid.
        /// </summary>
        public Vector3 Offset
        {
            get
            {
                return this.offset;
            }

            set
            {
                this.changed = this.changed || this.offset != value;
                this.offset = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="BeforeDrawGrid"/> event.
        /// </summary>
        protected virtual void OnBeforeDrawGrid()
        {
            var handler = this.BeforeDrawGrid;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs after drawing grid.
        /// </summary>
        public event EventHandler AfterDrawGrid;

        /// <summary>
        /// Raises the <see cref="AfterDrawGrid"/> event.
        /// </summary>
        protected virtual void OnAfterDrawGrid()
        {
            var handler = this.AfterDrawGrid;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

#if !UNITY_5
        private void CreateLineMaterial()
        {
            if (this.gridMaterial == null)
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
#else
        private void CreateLineMaterial()
        {
            if (this.gridMaterial == null)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                this.gridMaterial = new Material(shader);
                this.gridMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                this.gridMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                this.gridMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                this.gridMaterial.SetInt("_Cull", (int)CullMode.Off);
                // Turn off depth writes
                this.gridMaterial.SetInt("_ZWrite", 0);
            }
        }
#endif

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        public void Start()
        {
            this.UpdateMesh();
        }

        /// <summary>
        /// OnRenderObject is called after camera has rendered the scene.
        /// </summary>
        private void OnRenderObject()
        {
            this.CreateLineMaterial();
            if (this.changed)
            {
                this.UpdateMesh();
                this.changed = false;
            }

            if (this.useMesh)
            {
                var transformReference = this.origin == null ? this.transform : this.origin.transform;
                this.gridMaterial.SetPass(0);
                if (this.ShowMain)
                {
                    Graphics.DrawMeshNow(this.mesh, transformReference.position, transformReference.rotation, 0);
                }

                if (this.ShowSub)
                {
                    Graphics.DrawMeshNow(this.mesh, transformReference.position, transformReference.rotation, 1);
                }
            }
            else
            {
                this.RenderUsingGL();
            }
        }

        private void RenderUsingGL()
        {
#if PERFORMANCE
            var perf = PerformanceTesting<string>.Instance;
            perf.Start(PerformanceConstants.VectorGrid_OnRenderObject);
#endif

            if (!this.enabled)
            {
#if PERFORMANCE
                perf.Stop(PerformanceConstants.VectorGrid_OnRenderObject);
#endif
                return;
            }

            if (this.largeStep < 0.05f || this.smallStep < 0.05f || this.width < 0 || this.depth < 0 || this.height < 0)
            {
#if PERFORMANCE
                perf.Stop(PerformanceConstants.VectorGrid_OnRenderObject);
#endif
                return;
            }

            this.gridMaterial.SetPass(0);

            var transformReference = this.origin == null ? this.transform : this.origin.transform;
            var position = transformReference.position; //+ this.Offset;// new Vector3(this.OffsetX, this.OffsetY, this.OffsetZ);
            var rotation = transformReference.localRotation;

            this.OnBeforeDrawGrid();
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(position, rotation, Vector3.one));
            GL.Begin(GL.LINES);

            var callback = new Action<float>(step =>
                {
                    // layers
                    for (float j = 0; j <= this.depth; j += step)
                    {
                        //X axis lines
                        for (float i = 0; i <= this.height; i += step)
                        {
                            GL.Vertex3(0 + this.offset.x, j + this.offset.y, i + this.offset.z);
                            GL.Vertex3(this.width + this.offset.x, j + this.offset.y, i + this.offset.z);
                        }

                        //Z axis lines
                        for (float i = 0; i <= this.width; i += step)
                        {
                            GL.Vertex3(i + this.offset.x, j + this.offset.y, 0 + this.offset.z);
                            GL.Vertex3(i + this.offset.x, j + this.offset.y, this.height + this.offset.z);
                        }
                    }

                    // Y axis lines
                    for (float i = 0; i <= this.height; i += step)
                    {
                        for (float k = 0; k <= this.width; k += step)
                        {
                            GL.Vertex3(k + this.offset.x, 0 + this.offset.y, i + this.offset.z);
                            GL.Vertex3(k + this.offset.x, this.depth + this.offset.y, i + this.offset.z);
                        }
                    }
                });

            if (this.showSub)
            {
                GL.Color(this.subColor);
                callback(this.smallStep);
            }

            if (this.showMain)
            {
                GL.Color(this.mainColor);
                callback(this.largeStep);
            }

            GL.End();
            GL.PopMatrix();
            this.OnAfterDrawGrid();

#if PERFORMANCE
            perf.Stop(PerformanceConstants.VectorGrid_OnRenderObject);
#endif
        }
    }
}