namespace Codefarts.GeneralTools.Scripts
{
    using System;

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
            GUI.Label(new Rect(100, 100, 100, 100), guide.HighlightPosition.ToString());
        }
    }
}
