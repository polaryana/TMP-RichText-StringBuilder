using System.Collections.Generic;
using TMP_RichText_StringBuilder.Runtime;
using TMP_RichText_StringBuilder.Runtime.Base;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMP_RichText_StringBuilder.Editor
{
    public class TMPRichTextStringBuilderWindow : EditorWindow
    {
        private const string WindowTitle = "TMP RichText StringBuilder Window";

        [SerializeReference]
        private List<TextTool> tools = new();

        private SerializedObject _serializedObject;
        private SerializedProperty _toolsProperty;
        private ReorderableList _toolsList;

        private string _currentText;

        [MenuItem("Tools/TMP RichText StringBuilder Window")]
        public static void Open()
        {
            var window = GetWindow<TMPRichTextStringBuilderWindow>();
            window.titleContent = new GUIContent(WindowTitle);
            window.minSize = new Vector2(450, 600);
            window.Show();
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _toolsProperty = _serializedObject.FindProperty("tools");

            _toolsList = new ReorderableList(
                _serializedObject,
                _toolsProperty,
                true,
                true,
                true,
                true
            );

            _toolsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Text Tools (Drag to reorder)");
            };

            _toolsList.drawElementCallback = DrawElement;

            _toolsList.elementHeightCallback = index =>
            {
                var element = _toolsProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element, true) + 8f;
            };

            _toolsList.onAddDropdownCallback = OnAddDropdown;
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            DrawToolbar();
            EditorGUILayout.Space();

            _toolsList.DoLayoutList();

            EditorGUILayout.Space();
            DrawPreview();

            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Copy", EditorStyles.toolbarButton))
            {
                GUIUtility.systemCopyBuffer = _currentText;
            }

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                _toolsProperty.ClearArray();
                _currentText = string.Empty;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _toolsProperty.GetArrayElementAtIndex(index);

            rect.y += 4f;

            EditorGUI.PropertyField(
                rect,
                element,
                new GUIContent(element.managedReferenceValue.GetType().Name),
                true
            );
        }

        private void OnAddDropdown(Rect rect, ReorderableList list)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Text String"), false, () => AddTool(new TextString()));
            menu.AddItem(new GUIContent("New Line"), false, () => AddTool(new NewLine()));
            menu.AddItem(new GUIContent("Color Change"), false, () => AddTool(new ColorChange()));
            menu.AddItem(new GUIContent("Sprite From Atlas"), false, () => AddTool(new SpriteFromAtlas()));

            menu.ShowAsContext();
        }

        private void AddTool(TextTool tool)
        {
            _serializedObject.Update();

            _toolsProperty.arraySize++;
            var element = _toolsProperty.GetArrayElementAtIndex(_toolsProperty.arraySize - 1);
            element.managedReferenceValue = tool;

            _serializedObject.ApplyModifiedProperties();
        }

        private void DrawPreview()
        {
            RebuildText();

            EditorGUILayout.LabelField("Visual Preview", EditorStyles.boldLabel);

            var previewStyle = new GUIStyle(EditorStyles.textArea)
            {
                richText = true,
                wordWrap = true
            };

            var visualText = _currentText.Replace("\\n", "\n");
            EditorGUILayout.TextArea(visualText, previewStyle, GUILayout.Height(120));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Raw Generated String (Read Only)", EditorStyles.boldLabel);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextArea(_currentText, GUILayout.Height(80));
            }
        }

        private void RebuildText()
        {
            _currentText = string.Empty;

            for (var i = 0; i < tools.Count; i++)
            {
                if (tools[i] == null)
                {
                    continue;
                }

                _currentText += tools[i].ConvertToString();
            }
        }
    }
}
