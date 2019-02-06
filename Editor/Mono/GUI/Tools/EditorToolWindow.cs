// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine;

namespace UnityEditor.EditorTools
{
    [CustomEditor(typeof(EditorTool), true)]
    class EditorToolCustomEditor : Editor
    {
        const string k_GeneratorAssetProperty = "m_GeneratorAsset";
        const string k_ScriptProperty = "m_Script";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var property = serializedObject.GetIterator();

            bool expanded = true;

            while (property.NextVisible(expanded))
            {
                if (property.propertyPath == k_GeneratorAssetProperty)
                    continue;

                using (new EditorGUI.DisabledScope(property.propertyPath == k_ScriptProperty))
                {
                    EditorGUILayout.PropertyField(property, true);
                }

                expanded = false;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    sealed class EditorToolWindow : EditorWindow
    {
        static class Styles
        {
            public static GUIContent title = EditorGUIUtility.TrTextContent("Editor Tool");
        }

        Editor m_Editor;

        EditorToolWindow() {}

        [MenuItem("Window/General/Active Tool")]
        static void ShowEditorToolWindow()
        {
            GetWindow<EditorToolWindow>();
        }

        void OnEnable()
        {
            EditorToolContext.toolChanged += ToolChanged;
            ToolChanged(null, EditorToolContext.GetActiveTool());
        }

        void OnDisable()
        {
            EditorToolContext.toolChanged -= ToolChanged;
            if (m_Editor != null)
                DestroyImmediate(m_Editor);
        }

        void ToolChanged(EditorTool from, EditorTool to)
        {
            if (m_Editor != null)
                DestroyImmediate(m_Editor);
            m_Editor = Editor.CreateEditor(to);
            titleContent = new GUIContent(EditorToolUtility.GetToolName(to.GetType()));
            Repaint();
        }

        void OnGUI()
        {
            m_Editor.OnInspectorGUI();
        }
    }
}
