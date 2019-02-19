namespace Codefarts.VectorGrid
{
    using UnityEngine;

    [RequireComponent(typeof(Guides))]
    public class GuideTester : MonoBehaviour
    {
        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public void OnGUI()
        {
            var guide = this.GetComponent<Guides>();
            var text = guide.HighlightPosition.ToString();          
            var rect = new Rect(100, 100, 100, 100);
            var mousePosition = Input.mousePosition;
            rect.x = mousePosition.x - 50;
            rect.y = -mousePosition.y - 25;
            GUI.Label(rect, text);
        }
    }
}
