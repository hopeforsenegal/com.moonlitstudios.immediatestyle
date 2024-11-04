using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    [UsedImplicitly]
    public class LabelWidthScope : GUI.Scope
    {
        private readonly float m_LabelWidth;

        public LabelWidthScope(int tempWidth)
        {
            m_LabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = tempWidth;
        }
        protected override void CloseScope()
        {
            EditorGUIUtility.labelWidth = m_LabelWidth;
        }
    }

    [UsedImplicitly]
    public class HorizontalScope : GUI.Scope
    {
        public HorizontalScope() => EditorGUILayout.BeginHorizontal();

        protected override void CloseScope() => EditorGUILayout.EndHorizontal();
    }
}