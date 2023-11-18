using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChemistryElements.Editor
{
    [CustomPropertyDrawer(typeof(EditInlineAttribute))]
    public class EditInlinePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            rect.xMax -= 15;


            EditorGUI.PropertyField(rect, property, label);
            rect.y += rect.height;

            Rect boxRect = position;
            boxRect.yMin += 15 + EditorGUIUtility.standardVerticalSpacing * 4;
            boxRect.yMax -= 15 + EditorGUIUtility.standardVerticalSpacing * 3;

            GUI.Box(boxRect, "", GUI.skin.window);
            EditorGUI.indentLevel++;

            if (!property.objectReferenceValue)
            {
                return;
            }

            SerializedObject obj = new SerializedObject(property.objectReferenceValue);
            var iterator = obj.GetIterator();

            iterator.NextVisible(true);

            while (iterator.NextVisible(false))
            {
                rect.height = EditorGUI.GetPropertyHeight(iterator);
                rect.height += EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(rect, iterator, new GUIContent(iterator.displayName));
                rect.y += rect.height;
            }

            obj.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Rect rect = new Rect();
            rect.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (!property.objectReferenceValue)
            {
                return rect.height;
            }

            SerializedObject obj = new SerializedObject(property.objectReferenceValue);
            var iterator = obj.GetIterator();

            iterator.NextVisible(true);

            while (iterator.NextVisible(false))
            {
                rect.height += EditorGUI.GetPropertyHeight(iterator);
                rect.height += EditorGUIUtility.standardVerticalSpacing;
            }

            return rect.height + 15 + (EditorGUIUtility.standardVerticalSpacing * 4);
        }
    }
}