namespace Codefarts.GeneralTools.Scripts
{
    using System;
    using System.Collections.Generic;

    using Codefarts.GeneralTools.Models;

    using UnityEngine;

    [ExecuteInEditMode]
    public class VectorGridSelections : MonoBehaviour
    {
        [HideInInspector]
        public List<Point2D> Selections = new List<Point2D>();

        public Vector3 Offset;

        public Material gridMaterial;

        public Color SelectionColor = new Color32(0, 255, 0, 64);

        public event EventHandler<CellSelectedArgs> CellSelected;
        public event EventHandler<CellSelectedArgs> CellDeselected;

        public Transform origin;
        public float Width = 10;
        public float Depth;
        public float Height = 10;
        private bool isSelectingCells;

        public VectorGrid grid;

        public bool MouseIsDragging { get; set; }

        public bool AutomaticallyHandleSelecting = false;

        private readonly CellSelectedArgs cellSelectedArgs = new CellSelectedArgs();

        [Tooltip("Specifies the mouse button index to use.")]
        public int MouseSelectionButton = 0;

        public void ToggleCell(int x, int y)
        {
            if (this.Selections == null)
            {
                return;
            }

            this.cellSelectedArgs.Row = y;
            this.cellSelectedArgs.Column = x;
            for (var i = 0; i < this.Selections.Count; i++)
            {
                var selection = this.Selections[i];
                if (selection.X == x && selection.Y == y)
                {
                    this.Selections.RemoveAt(i);
                    var onCellDeselected = this.CellDeselected;
                    if (onCellDeselected != null)
                    {
                        onCellDeselected(this, this.cellSelectedArgs);
                    }

                    return;
                }
            }

            var item = new Point2D(x, y);
            this.Selections.Add(item);
            var onCellSelected = this.CellSelected;
            if (onCellSelected != null)
            {
                onCellSelected(this, this.cellSelectedArgs);
            }
        }

        public void SelectCell(int x, int y)
        {
            if (this.Selections == null)
            {
                return;
            }

            var item = new Point2D(x, y);
            if (this.Selections.Contains(item))
            {
                return;
            }

            this.Selections.Add(item);
            this.cellSelectedArgs.Row = y;
            this.cellSelectedArgs.Column = x;
            var onCellSelected = this.CellSelected;
            if (onCellSelected != null)
            {
                onCellSelected(this, this.cellSelectedArgs);
            }
        }

        public bool IsCellSelected(int x, int y)
        {
            if (this.Selections == null)
            {
                return false;
            }

            return this.Selections.Contains(new Point2D(x, y));
        }

        public void DeselectCell(int x, int y)
        {
            if (this.Selections == null)
            {
                return;
            }

            var item = new Point2D(x, y);
            if (!this.Selections.Contains(item))
            {
                return;
            }

            this.Selections.Remove(item);
            this.cellSelectedArgs.Row = y;
            this.cellSelectedArgs.Column = x;
            var onCellDeselected = this.CellDeselected;
            if (onCellDeselected != null)
            {
                onCellDeselected(this, this.cellSelectedArgs);
            }
        }
      
#if !UNITY_5
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
#else
        private void CreateLineMaterial()
        {
            if (!this.gridMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                this.gridMaterial = new Material(shader);
                this.gridMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                this.gridMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                this.gridMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                this.gridMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                this.gridMaterial.SetInt("_ZWrite", 0);
            }
        }
#endif

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public void Update()
        {
            if (this.AutomaticallyHandleSelecting)
            {
                this.HandleCellSelection();
            }
        }

        private void HandleCellSelection()
        {
            // if the mouse is positioned over the layer allow drawing actions to occur
            var guide = this.GetComponent<Guides>();
            if (guide == null)
            {
                return;
            }

            if (this.MouseIsDragging && Input.GetMouseButtonUp(this.MouseSelectionButton))
            {
                this.MouseIsDragging = false;
            }

            if (!guide.MouseIsOver)
            {
                return;
            }

            var position = guide.HighlightPosition;
            // var mouseHitPos =  guide.MouseHitPos;
            //var mouseOnLayer = guide.MouseIsOver;// mouseHitPos.x >= guide.Offset.x && mouseHitPos.x <= guide.Width && mouseHitPos.z >= guide.Offset.z && mouseHitPos.z <= guide.Height;

            var column = (int)position.x;
            var row = (int)position.z;
            if (Input.GetMouseButtonDown(this.MouseSelectionButton))
            {
                this.MouseIsDragging = true;
                this.isSelectingCells = !this.IsCellSelected(column, row);
            }

            if (this.MouseIsDragging)
            {
                if (this.isSelectingCells)
                {
                    this.SelectCell(column, row);
                }
                else
                {
                    this.DeselectCell(column, row);
                    var onCellDeselected = this.CellDeselected;
                    if (onCellDeselected != null)
                    {
                        onCellDeselected(this, this.cellSelectedArgs);
                    }
                }
            }
        }

        public void OnRenderObject()
        {
            if (!this.enabled)
            {
                return;
            }

            if (this.grid == null && (this.Width < 0 || this.Depth < 0 || this.Height < 0))
            {
                return;
            }

            this.CreateLineMaterial();
            this.gridMaterial.SetPass(0);

            Vector3 position;
            Quaternion rotation;

            if (this.grid != null)
            {
                position = (this.grid.Origin == null ? this.transform : this.grid.Origin.transform).position;
                rotation = (this.grid.Origin == null ? this.transform : this.grid.Origin.transform).localRotation;
            }
            else
            {
                position = (this.origin == null ? this.transform : this.origin.transform).position;
                rotation = (this.origin == null ? this.transform : this.origin.transform).localRotation;
            }

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(position, rotation, Vector3.one));
            GL.Begin(GL.QUADS);

            GL.Color(this.SelectionColor);
            if (this.Selections != null)
            {
                foreach (var selection in this.Selections)
                {
                    GL.Vertex3(selection.X + this.Offset.x, this.Offset.y, selection.Y + this.Offset.z);
                    GL.Vertex3(selection.X + this.Offset.x, this.Offset.y, selection.Y + this.Offset.z + 1);
                    GL.Vertex3(selection.X + this.Offset.x + 1, this.Offset.y, selection.Y + this.Offset.z + 1);
                    GL.Vertex3(selection.X + this.Offset.x + 1, this.Offset.y, selection.Y + this.Offset.z);
                }
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}