namespace Codefarts.GeneralTools.Editor
{
    using System;

    using Codefarts.GeneralTools.Scripts;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides a editor for your script files that you can edit within the inspector.
    /// </summary>
    [CustomEditor(typeof(VectorGrid))]
    public class VectorGridEditor : Editor
    {
        private GUIContent content;

        /// <summary>
        /// Called by unity to draw the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            this.content = this.content ?? new GUIContent();

            var grid = this.target as VectorGrid;
            //  this.DrawDefaultInspector();

            if (grid == null)
            {
                return;
            }

            var changed = false;
            grid.ShowMain = this.Toggle(grid.ShowMain, "Show Main", (p, n) => changed = true);
            grid.ShowSub = this.Toggle(grid.ShowSub, "Show Sub", (p, n) => changed = true);
            grid.UseMesh = this.Toggle(grid.UseMesh, "Use Mesh", (p, n) => changed = true);

            grid.SmallStep = EditorGUILayout.FloatField("Small Step", grid.SmallStep);
            grid.LargeStep = EditorGUILayout.FloatField("Large Step", grid.LargeStep);

            grid.Width = EditorGUILayout.FloatField("Width", grid.Width);
            grid.Height = EditorGUILayout.FloatField("Height", grid.Height);
            grid.Depth = EditorGUILayout.FloatField("Depth", grid.Depth);

            grid.Offset = EditorGUILayout.Vector3Field("Offset", grid.Offset);

            grid.Origin = EditorGUILayout.ObjectField("Origin", grid.Origin, typeof(Transform), true) as Transform;
            grid.GridMaterial = EditorGUILayout.ObjectField("Grid Material", grid.GridMaterial, typeof(Material), true) as Material;
            grid.MainColor = EditorGUILayout.ColorField("Main Color", grid.MainColor);
            grid.SubColor = EditorGUILayout.ColorField("Sub Color", grid.SubColor);

            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the value on or off?</param>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="valueChangedCallback">The value changed callback that is called if the toggle state changed. 
        /// The arguments are Previous state, New State.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties.
        ///  Any values passed in here will override settings defined by the style.</param>
        /// <returns>The new value of the button.</returns>
        private bool Toggle(bool value, string text, Action<bool, bool> valueChangedCallback, params GUILayoutOption[] options)
        {
            this.content.text = text;
            return this.Toggle(value, this.content, null, valueChangedCallback, options);
        }

        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the value on or off?</param>
        /// <param name="content">Text, image and tooltip for this button.</param>
        /// <param name="style">The style to use. If left out, the button style from the current GUISkin is used.</param>
        /// <param name="valueChangedCallback">The value changed callback that is called if the toggle state changed. 
        /// The arguments are Previous state, New State.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties.
        ///  Any values passed in here will override settings defined by the style.</param>
        /// <returns>The new value of the button.</returns>
        private bool Toggle(bool value, GUIContent content, GUIStyle style, Action<bool, bool> valueChangedCallback, params GUILayoutOption[] options)
        {
            bool changed;
            var result = this.Toggle(value, content, style ?? GUI.skin.toggle, out changed, options);
            if (changed && valueChangedCallback != null)
            {
                valueChangedCallback(value, result);
            }

            return result;
        }

        /// <summary>
        /// Make an on/off toggle button.
        /// </summary>
        /// <param name="value">Is the value on or off?</param>
        /// <param name="content">Text, image and tooltip for this button.</param>
        /// <param name="style">The style to use. If left out, the button style from the current GUISkin is used.</param>
        /// <param name="valueChanged">if set to <c>true</c> the toggle state was changed.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties.
        ///  Any values passed in here will override settings defined by the style.</param>
        /// <returns>The new value of the button.</returns>
        private bool Toggle(bool value, GUIContent content, GUIStyle style, out bool valueChanged, params GUILayoutOption[] options)
        {
            var result = GUILayout.Toggle(value, content, style, options);
            valueChanged = result != value;
            return result;
        }
    }
}