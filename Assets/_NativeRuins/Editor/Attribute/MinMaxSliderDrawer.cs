using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
class MinMaxSliderDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            float textFieldWidth = 30;

            EditorGUI.LabelField(position, label);
            Vector2 range = property.vector2Value;
            float min = range.x;
            float max = range.y;
            MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;

            Rect sliderPos = position;
            sliderPos.x += EditorGUIUtility.labelWidth + textFieldWidth;
            sliderPos.width -= EditorGUIUtility.labelWidth + textFieldWidth * 2;
            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(position, ref min, ref max, attr.min, attr.max);
            if (EditorGUI.EndChangeCheck())
            {
                range.x = min;
                range.y = max;
                property.vector2Value = range;
            }
            EditorGUI.LabelField(position, "");

            Rect minPos = position;
            minPos.x += EditorGUIUtility.labelWidth;
            minPos.width = textFieldWidth;
            Debug.Log(minPos);
            EditorGUI.LabelField(minPos, min.ToString("0.00"));
            Rect maxPos = position;
            maxPos.x += maxPos.width - textFieldWidth;
            maxPos.width = textFieldWidth;
            //Debug.Log(mPos);
            EditorGUI.LabelField(maxPos, max.ToString("0.00"));
        }
        else
        {
            EditorGUI.LabelField(position, label, "Use only with Vector2");
        }
    }
}