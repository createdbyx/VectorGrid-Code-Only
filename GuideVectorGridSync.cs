namespace Codefarts.GeneralTools.Scripts
{
    using UnityEngine;

    public enum GuideGridSync
    {
        GuideGetsValuesFromGrid,
        GridGetsValuesFromGuide
    }

    public class GuideVectorGridSync : MonoBehaviour
    {
        public GuideGridSync SynchronizeFromGrid;

        public Guides guides;

        public VectorGrid grid;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public void Update()
        {
            if (this.guides == null || this.grid == null)
            {
                return;
            }

            if (this.SynchronizeFromGrid == GuideGridSync.GuideGetsValuesFromGrid)
            {
                this.guides.Width = this.grid.Width;
                this.guides.Height = this.grid.Height;
                this.guides.Depth = this.grid.Depth;
            }
            else
            {
                this.grid.Width = this.guides.Width;
                this.grid.Height = this.guides.Height;
                this.grid.Depth = this.guides.Depth;
            }
        }
    }
}