using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    internal class LabelWidthScope : GUI.Scope
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
}