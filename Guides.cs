namespace Codefarts.VectorGrid
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Provides guide lines for visually indicating where the pointer is on a grid.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [ExecuteInEditMode]
    public partial class Guides : MonoBehaviour
    {
        /// <summary>
        /// Provides enums for determining sync direction.
        /// </summary>
        public enum SyncDirection
        {
            UseDimensions,
            UseGuidelineDimensions
        }

        /// <summary>
        /// The line material used to render lines.
        /// </summary>
        public Material lineMaterial;

        /// <summary>
        /// The guide color to be used when not rendering lines with a material.
        /// </summary>
        public Color GuideColor = Color.yellow;

        /// <summary>
        /// The highlight color to be used when not rendering lines with a material.
        /// </summary>
        public Color HighlightColor = Color.red;

        public Vector3 HighlightOffset;

        public Vector3 Offset;


        public float Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
                this.DoSyncDimensions(SyncDirection.UseDimensions);
            }
        }

        public float Depth
        {
            get
            {
                return this.depth;
            }

            set
            {
                this.depth = value;
                this.DoSyncDimensions(SyncDirection.UseDimensions);
            }
        }

        public float Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                this.DoSyncDimensions(SyncDirection.UseDimensions);
            }
        }

        public float GuidelineWidth
        {
            get
            {
                return this.guidelineWidth;
            }

            set
            {
                this.guidelineWidth = value;
                this.DoSyncDimensions(SyncDirection.UseGuidelineDimensions);
            }
        }

        public float GuidelineDepth
        {
            get
            {
                return this.guidelineDepth;
            }

            set
            {
                this.guidelineDepth = value;
                this.DoSyncDimensions(SyncDirection.UseGuidelineDimensions);
            }
        }

        public float GuidelineHeight
        {
            get
            {
                return this.guidelineHeight;
            }

            set
            {
                this.guidelineHeight = value;
                this.DoSyncDimensions(SyncDirection.UseGuidelineDimensions);
            }
        }

        private void DoSyncDimensions(SyncDirection direction)
        {
            if (!this.syncDimensions)
            {
                return;
            }

            switch (direction)
            {
                case SyncDirection.UseDimensions:
                    this.guidelineWidth = this.width;
                    this.guidelineDepth = this.depth;
                    this.guidelineHeight = this.height;
                    break;

                case SyncDirection.UseGuidelineDimensions:
                    this.width = this.guidelineWidth;
                    this.depth = this.guidelineDepth;
                    this.height = this.guidelineHeight;
                    break;
            }
        }

        [SerializeField]
        private float width = 10;
        [SerializeField]
        private float depth;
        [SerializeField]
        private float height = 10;

        [SerializeField]
        private float guidelineWidth = 10;
        [SerializeField]
        private float guidelineDepth;
        [SerializeField]
        private float guidelineHeight = 10;

        public SyncDirection DefaultSyncDirection = SyncDirection.UseDimensions;

        public bool SyncDimensions
        {
            get
            {
                return this.syncDimensions;
            }

            set
            {
                this.syncDimensions = value;
                this.DoSyncDimensions(this.DefaultSyncDirection);
            }
        }

        public bool InfiniteGuidelines = false;

        public Transform origin;

        public void OnRenderObject()
        {
            if (!this.enabled)
            {
                return;
            }

            this.lineMaterial = Helpers.CreateLineMaterial(this.lineMaterial);
            this.lineMaterial.SetPass(0);

            var transformReference = this.origin == null ? this.transform : this.origin.transform;
            var position = transformReference.position;//+ this.Offset;// new Vector3(this.Offset.x, this.Offset.y, this.Offset.z);
            var rotation = transformReference.localRotation;

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(position, rotation, Vector3.one));
            GL.Begin(GL.LINES);

            this.DrawHighlight();

            // if guide lines are not to be drawn we can just exit here
            if (this.DrawGuideLines)
            {
                // draw guidelines
                GL.Color(this.GuideColor);

                var pos = this.HighlightPosition + (this.SnapToGrid ? this.Offset : Vector3.zero);

                var halfSize = this.SnapToGrid ? this.CellSize / 2f : Vector3.zero;// this.HighlightSize / 2f;

                Vector3 min;
                Vector3 max;
                if (this.InfiniteGuidelines)
                {
                    min = new Vector3(Int16.MinValue, Int16.MinValue, Int16.MinValue);
                    max = new Vector3(Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);
                }
                else
                {
                    min = this.Offset;// Vector3.zero;
                    max = new Vector3(this.guidelineWidth, this.guidelineDepth, this.guidelineHeight) + this.Offset;
                }

                // draw column guide line
                GL.Vertex3(min.x, pos.y, pos.z + halfSize.z);
                GL.Vertex3(max.x, pos.y, pos.z + halfSize.z);

                // draw row guide line
                GL.Vertex3(pos.x + halfSize.x, pos.y, min.z);
                GL.Vertex3(pos.x + halfSize.x, pos.y, max.z);
            }

            GL.End();
            GL.PopMatrix();
        }

        private void DrawHighlight()
        {
            if (!this.DrawHighlighter)
            {
                return;
            }

            // Draw marker position
            GL.Color(this.HighlightColor);

            var halfSize = this.HighlightSize / 2f;

            var position = this.HighlightPosition + this.HighlightOffset + (this.SnapToGrid ? -this.CellSize / 2 : Vector3.zero);

            // bottom square
            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);

            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);

            // top square
            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);

            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);

            // sides
            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);

            GL.Vertex3(-halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);
            GL.Vertex3(-halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, halfSize.z + position.z);

            GL.Vertex3(halfSize.x + position.x, -halfSize.y + position.y, -halfSize.z + position.z);
            GL.Vertex3(halfSize.x + position.x, halfSize.y + position.y, -halfSize.z + position.z);
        }

        public bool DrawHighlighter = true;

        public bool DrawGuideLines = true;

        public Vector3 HighlightSize = Vector3.one;

        public Vector3 HighlightPosition;

        public bool FollowMouse = true;

        public Vector3 MouseHitPos
        {
            get
            {
                return this.mouseHitPos;
            }

            set
            {
                this.mouseHitPos = value;
                if ((value.x >= this.Offset.x && value.z >= this.Offset.z) && (value.x <= this.width + this.Offset.x && value.z <= this.height + this.Offset.z))
                {
                    // convert the hit location from world space to local space
                    this.MouseIsOver = true;
                }
                else
                {
                    this.MouseIsOver = false;
                }
            }
        }

        public Transform HighlighterObject;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public void Update()
        {
            if (this.FollowMouse)
            {
                this.UpdateHitPosition();

                var gridPosition = this.GetGridPositionFromMouseLocation();

                // store the grid position in world space
                var pos = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z);

                this.HighlightPosition = pos;

                if (this.HighlighterObject != null)
                {
                    var transformReference = this.HighlighterObject.transform;
                    transformReference.position = this.HighlightPosition + this.HighlightOffset;
                    if (this.ScaleHighlighterObjectWithHighlighterSize)
                    {
                        transformReference.localScale = this.HighlightSize;
                    }
                }
            }
        }

        public bool ScaleHighlighterObjectWithHighlighterSize = true;

        public bool OnlyUpdateOnMouseOver = true;

        /// <summary>
        /// Calculates the position of the mouse over the grid map in local space coordinates.
        /// </summary>
        /// <returns>Returns true if the mouse is over the grid map.</returns>
        private void UpdateHitPosition()
        {
            // build a ray type from the current mouse position
            var cameraReference = UnityEngine.Camera.main;
            if (cameraReference == null)
            {
                return;
            }

            // build a plane object that 
            var transformReference = this.origin == null ? this.transform : this.origin.transform;
            var p = new Plane(transformReference.up, transformReference.position);

            var mousePosition = Input.mousePosition;
            var ray = cameraReference.ScreenPointToRay(new Vector2(mousePosition.x, mousePosition.y));

            // stores the hit location
            var hit = new Vector3();

            // stores the distance to the hit location
            float dist;

            // cast a ray to determine what location it intersects with the plane
            if (p.Raycast(ray, out dist))
            {
                // the ray hits the plane so we calculate the hit location in world space
                hit = ray.origin + (ray.direction.normalized * dist);
            }

            var hitPos = transformReference.InverseTransformPoint(hit);
            if (this.OnlyUpdateOnMouseOver)
            {
                if ((hitPos.x >= this.Offset.x && hitPos.z >= this.Offset.z) && (hitPos.x <= this.width + this.Offset.x && hitPos.z <= this.height + this.Offset.z))
                {
                    this.mouseHitPos = hitPos;
                    // convert the hit location from world space to local space
                    this.MouseIsOver = true;
                }
                else
                {
                    this.MouseIsOver = false;
                    return;
                }
            }

            // convert the hit location from world space to local space
            this.MouseHitPos = hitPos;
        }

        public bool MouseIsOver { get; private set; }

        public bool SnapToGrid = true;

        public Vector3 CellSize = Vector3.one;

        [SerializeField]
        private bool syncDimensions;

        private Vector3 mouseHitPos;

        /// <summary>
        /// Calculates the location in grid coordinates (Column/Row) of the mouse position
        /// </summary>
        /// <returns>Returns a <see cref="Vector2"/> type representing the Column and Row where the mouse of positioned over.</returns>
        private Vector3 GetGridPositionFromMouseLocation()
        {
            float col;
            float row;
            float yPos;

            if (this.SnapToGrid)
            {
                var pos = new Vector3(
                    (float)Math.Truncate((this.MouseHitPos.x - this.Offset.x) / this.CellSize.x),
                    (float)Math.Truncate((this.MouseHitPos.y - this.Offset.y) / this.CellSize.y),
                    (float)Math.Truncate((this.MouseHitPos.z - this.Offset.z) / this.CellSize.z));

                // do a check to ensure that the row and column are with the bounds of the grid map         
                col = pos.x * this.CellSize.x;
                yPos = pos.y * this.CellSize.y;
                row = pos.z * this.CellSize.z;
                if (row < 0)
                {
                    row = 0;
                }

                if (row > this.width)
                {
                    row = this.width;
                }

                if (col < 0)
                {
                    col = 0;
                }

                if (col > this.height)
                {
                    col = this.height;
                }

                if (yPos < 0)
                {
                    yPos = 0;
                }

                if (yPos > this.depth)
                {
                    yPos = this.depth;
                }
            }
            else
            {
                var pos = this.MouseHitPos;

                // do a check to ensure that the row and column are with the bounds of the grid map         
                col = pos.x;
                yPos = pos.y;
                row = pos.z;
                if (row < this.Offset.x)
                {
                    row = this.Offset.x;
                }

                if (row > this.width - this.Offset.x)
                {
                    row = this.width - this.Offset.x;
                }

                if (col < this.Offset.z)
                {
                    col = this.Offset.z;
                }

                if (col > this.height - this.Offset.z)
                {
                    col = this.height - this.Offset.z;
                }

                if (yPos < this.Offset.y)
                {
                    yPos = this.Offset.y;
                }

                if (yPos > this.depth - this.Offset.y)
                {
                    yPos = this.depth - this.Offset.y;
                }
            }

            // return the column and row values
            return new Vector3(col, yPos, row);
        }
    }
}