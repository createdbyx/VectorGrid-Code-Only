namespace Codefarts.VectorGrid.Editor
{                                           
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides a editor for your script files that you can edit within the inspector.
    /// </summary>
    [CustomEditor(typeof(Guides))]
    public class GuidesEditor : Editor
    {
        /// <summary>
        /// Called by unity to draw the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var guides = this.target as Guides;

            if (guides == null)
            {
                return;
            }

            guides.lineMaterial = EditorGUILayout.ObjectField("Line Material", guides.lineMaterial, typeof(Material),false) as Material;

            guides.GuideColor = EditorGUILayout.ColorField("Guide Color", guides.GuideColor);                     
            guides.HighlightColor = EditorGUILayout.ColorField("Highlight Color", guides.HighlightColor);

            guides.SyncDimensions = EditorGUILayout.Toggle("Sync Dimensions", guides.SyncDimensions);
            guides.DefaultSyncDirection = (Guides.SyncDirection)EditorGUILayout.EnumPopup("Default Sync Direction", guides.DefaultSyncDirection);


            EditorGUILayout.PrefixLabel("Dimensions");
            EditorGUI.indentLevel++;
            guides.Width = EditorGUILayout.FloatField("Width", guides.Width);
            guides.Depth = EditorGUILayout.FloatField("Depth", guides.Depth);
            guides.Height = EditorGUILayout.FloatField("Height", guides.Height);
            EditorGUI.indentLevel--;


            GUILayout.Label("Guideline Dimensions");
            EditorGUI.indentLevel++;
            guides.GuidelineWidth = EditorGUILayout.FloatField("Width", guides.GuidelineWidth);
            guides.GuidelineDepth = EditorGUILayout.FloatField("Depth", guides.GuidelineDepth);
            guides.GuidelineHeight = EditorGUILayout.FloatField("Height", guides.GuidelineHeight);
            EditorGUI.indentLevel--;

            guides.origin = EditorGUILayout.ObjectField("Origin", guides.origin, typeof(Transform), true) as Transform;
            guides.HighlighterObject = EditorGUILayout.ObjectField("Highlighter Object", guides.HighlighterObject, typeof(Transform), true) as Transform;
            EditorGUI.indentLevel++;
            guides.ScaleHighlighterObjectWithHighlighterSize = EditorGUILayout.Toggle("Scale Highlighter Object", guides.ScaleHighlighterObjectWithHighlighterSize);
            EditorGUI.indentLevel--;

            guides.HighlightSize = EditorGUILayout.Vector3Field("Highlight Size", guides.HighlightSize);
            guides.HighlightPosition = EditorGUILayout.Vector3Field("Highlight Position", guides.HighlightPosition);
            guides.HighlightOffset = EditorGUILayout.Vector3Field("Highlight Offset", guides.HighlightOffset);
            guides.Offset = EditorGUILayout.Vector3Field("Offset", guides.Offset);

            guides.InfiniteGuidelines = EditorGUILayout.Toggle("Infinite Guidelines", guides.InfiniteGuidelines);
            guides.DrawHighlighter = EditorGUILayout.Toggle("Draw Highlighter", guides.DrawHighlighter);
            guides.DrawGuideLines = EditorGUILayout.Toggle("Draw Guidelines", guides.DrawGuideLines);
            guides.FollowMouse = EditorGUILayout.Toggle("Follow Mouse", guides.FollowMouse);
            guides.OnlyUpdateOnMouseOver = EditorGUILayout.Toggle("Update only on mouse over", guides.OnlyUpdateOnMouseOver);
            guides.SnapToGrid = EditorGUILayout.Toggle("Snap to grid", guides.SnapToGrid);
            guides.CellSize = EditorGUILayout.Vector3Field("Cell Size", guides.CellSize);
        }
    }
}