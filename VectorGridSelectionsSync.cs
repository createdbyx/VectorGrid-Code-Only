namespace Codefarts.GeneralTools.Scripts
{
    using UnityEngine;

    public enum VectorGridSync
    {
        SelectionsGetsValuesFromGrid,
        GridGetsValuesFromSelections
    }

    public class VectorGridSelectionsSync : MonoBehaviour
    {
        public VectorGridSync SynchronizeFromGrid;

        public VectorGridSelections selections;

        public VectorGrid grid;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public void Update()
        {
            if (this.selections == null || this.grid == null)
            {
                return;
            }

            if (this.SynchronizeFromGrid == VectorGridSync.SelectionsGetsValuesFromGrid)
            {
                this.selections.Width = this.grid.Width;
                this.selections.Height = this.grid.Height;
                this.selections.Depth = this.grid.Depth;
            }
            else
            {
                this.grid.Width = this.selections.Width;
                this.grid.Height = this.selections.Height;
                this.grid.Depth = this.selections.Depth;
            }
        }
    }
}