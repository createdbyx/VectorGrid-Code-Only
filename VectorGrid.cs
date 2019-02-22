namespace Codefarts.VectorGrid
{
    using System;
    using System.Collections.Generic;

#if PERFORMANCE
    using Codefarts.PerformanceTesting;
#endif

    using UnityEngine.Rendering;

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu("Codefarts/Vector Grid")]
    public partial class VectorGrid : MonoBehaviour
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
        private bool showSub;

        /// <summary>
        /// The backing field for the <see cref="UseMesh"/> property.
        /// </summary>
        [SerializeField]
        private bool useMesh;

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
        /// The backing field for the <see cref="MainMaterial"/> property.
        /// </summary>
        [SerializeField]
        private Material mainMaterial;

        /// <summary>
        /// The backing field for the <see cref="SubMaterial"/> property.
        /// </summary>
        [SerializeField]
        private Material subMaterial;

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

        /// <summary>
        /// The mesh object that holds mesh information.
        /// </summary>
        private Mesh mesh;

        /// <summary>
        /// The changed filed used to determine if a property change occured.
        /// </summary>
        /// <remarks>If set to true the mesh will be rebuild on the next call to <see cref="OnRenderObject"/>.</remarks>
        private bool changed;

        /// <summary>
        /// Holds a cached reference to a mesh renderer component.
        /// </summary>
        private MeshRenderer meshRenderer;

        /// <summary>
        /// Holds a cache d reference to a mesh filter component.
        /// </summary>
        private MeshFilter meshFilter;

        /// <summary>
        /// Occurs before drawing grid.
        /// </summary>
        /// <remarks>Only raised when not rendering a mesh.</remarks>
        public event EventHandler BeforeDrawGrid;

        /// <summary>
        /// Occurs after drawing grid.
        /// </summary>
        /// <remarks>Only raised when not rendering a mesh.</remarks>
        public event EventHandler AfterDrawGrid;

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

        /// <summary>
        /// Updates the mesh vertex and index data.
        /// </summary>
        private void UpdateMesh()
        {
            this.mesh = this.mesh == null ? new Mesh() : this.mesh;

            var callback = new Func<float, Vector3[]>(step =>
                {
                    var vectors = new List<Vector3>();

                    // layers
                    for (var j = 0f; j <= this.depth; j += step)
                    {
                        //X axis lines
                        for (var i = 0f; i <= this.height; i += step)
                        {
                            vectors.Add(new Vector3(0 + this.offset.x, j + this.offset.y, i + this.offset.z));
                            vectors.Add(new Vector3(this.width + this.offset.x, j + this.offset.y, i + this.offset.z));
                        }

                        //Z axis lines
                        for (var i = 0f; i <= this.width; i += step)
                        {
                            vectors.Add(new Vector3(i + this.offset.x, j + this.offset.y, 0 + this.offset.z));
                            vectors.Add(new Vector3(i + this.offset.x, j + this.offset.y, this.height + this.offset.z));
                        }
                    }

                    // Y axis lines
                    for (var i = 0f; i <= this.height; i += step)
                    {
                        for (var k = 0f; k <= this.width; k += step)
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

            // IndexFormat.UInt32 may not work on all platforms. Although this code is originally intended for in Editor use only.
            this.mesh.indexFormat = IndexFormat.UInt32;
            this.mesh.vertices = vertexes;
            this.mesh.colors = colors;
            this.mesh.subMeshCount = 2;

            // set indecies
            this.mesh.SetIndices(mainIndicies, MeshTopology.Lines, 0);
            this.mesh.SetIndices(subIndicies, MeshTopology.Lines, 1);

            // update data data
            this.mesh.UploadMeshData(false);

            this.meshFilter.sharedMesh = this.mesh;
        }

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
        public Material MainMaterial
        {
            get
            {
                return this.mainMaterial;
            }

            set
            {
                this.mainMaterial = value;
                this.SetMeshRendererMaterial(0, value);
            }
        }

        /// <summary>
        /// Gets or sets the grid material used to render the grid sub lines.
        /// </summary>
        /// <remarks>Setting this to null will create a internal line drawing material.</remarks>
        public Material SubMaterial
        {
            get
            {
                return this.subMaterial;
            }

            set
            {
                this.subMaterial = value;
                this.SetMeshRendererMaterial(1, value);
            }
        }

        private void SetMeshRendererMaterial(int index, Material mat)
        {
            if (this.meshRenderer.sharedMaterials.Length < index + 1)
            {
                var items = new Material[index + 1];
                this.meshRenderer.sharedMaterials.CopyTo(items, 0);
                items[index] = mat;
                this.meshRenderer.sharedMaterials = items;
            }

            this.meshRenderer.sharedMaterials[index] = mat;

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

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        public void Start()
        {
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            this.meshRenderer.receiveShadows = false;
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.UpdateMesh();
        }

        /// <summary>
        /// OnRenderObject is called after camera has rendered the scene.
        /// </summary>
        private void OnRenderObject()
        {
            this.mainMaterial = Helpers.CreateLineMaterial(this.mainMaterial);
            if (this.changed)
            {
                this.UpdateMesh();
                this.changed = false;
            }

            if (this.useMesh)
            {
                this.meshRenderer.enabled = true;
                //var transformReference = this.origin == null ? this.transform : this.origin.transform;
                //this.mainMaterial.SetPass(0);
                //if (this.ShowMain)
                //{
                //    Graphics.DrawMeshNow(this.mesh, transformReference.position, transformReference.rotation, 0);
                //}

                //if (this.ShowSub)
                //{
                //    Graphics.DrawMeshNow(this.mesh, transformReference.position, transformReference.rotation, 1);
                //}
            }
            else
            {
                this.meshRenderer.enabled = false;
                this.RenderUsingGL();
            }
        }

        /// <summary>
        /// Renders the grid using low level graphics lib.
        /// </summary>   
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
                this.mainMaterial.SetPass(0);
                GL.Color(this.subColor);
                callback(this.smallStep);
            }

            if (this.showMain)
            {
                this.mainMaterial.SetPass(0);
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