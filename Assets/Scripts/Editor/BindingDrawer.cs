using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(ViewBinding))]
public class BindingDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsPrefabRoot(property.serializedObject.targetObject as Component))
        {
            return;
        }
        
        var view = property.serializedObject.targetObject as ViewBase;
        if (view != null)
        {
            var valueHolderProperty = property.FindPropertyRelative("ValueHolder");
            valueHolderProperty.SetUnderlyingValue(GetValueHolder(view));
            // if (valueHolderProperty.objectReferenceValue == null)
            // {
            // }
            // else if (view.transform.parent.TryGetComponent(out ListView listView))
            // {
            //     valueHolderProperty.SetUnderlyingValue(listView);
            // }

            var valueHolder = valueHolderProperty.objectReferenceValue as ViewBase;
            position.height = 20;
            EditorGUI.PropertyField(position, valueHolderProperty);
            position.y += 20;

            var popupRect = new Rect(
                position.x + EditorGUIUtility.labelWidth + 2, 
                position.y + 2, 
                position.width - EditorGUIUtility.labelWidth - 2,
                position.height);
            
            var labelRect = new Rect(
                position.x, 
                position.y, 
                EditorGUIUtility.labelWidth,
                position.height);
            
            var valueType = view.GetValueType();
            if (valueHolder == null || valueType == null)
            {
                return;
            }
            
            EditorGUI.LabelField(labelRect, "Property");
            var fieldType = view.GetFieldType();
            var options = GetProperties(valueHolder, fieldType, valueType);
            if (options.Length > 0)
            {
                var propertyName = property.FindPropertyRelative("PropertyName");
                if (string.IsNullOrEmpty(propertyName.stringValue))
                {
                    propertyName.SetUnderlyingValue(options.FirstOrDefault());
                }

                var selected = Array.IndexOf(options, propertyName.stringValue);
                var newIndex = EditorGUI.Popup(popupRect, selected, options);
                if (newIndex != selected)
                {
                    propertyName.SetUnderlyingValue(options[newIndex]);
                    valueHolder.SetModel();
                    view.SubscribeToValueChange();
                    EditorUtility.SetDirty(view);
                }
            }
            else
            {
                EditorGUI.LabelField(popupRect, "No values of type " + valueType.Name);
            }
        }
    }

    public static bool IsPrefabRoot(Component? component)
    {
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            return false;
        }
        return prefabStage.prefabContentsRoot == component?.gameObject;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (IsPrefabRoot(property.serializedObject.targetObject as Component))
        {
            return 0f;
        }
        
        return 40f;
    }

    private ViewBase? GetValueHolder(Component component)
    {
        if (component.transform.parent == null)
        {
            return null;
        }
        
        var views = component.transform.parent.GetComponentInParent<IModelView>();
        return views?.ViewBase;
    }

    private string[] GetProperties(ViewBase valueHolder, Type fieldType, Type valueType)
    {
        return valueHolder
            .GetType()
            .GetFields()
            .Where(field => {
                if (!field.FieldType.IsGenericType || field.FieldType.GetGenericTypeDefinition() != fieldType)
                {
                    return false;
                }
                var genericTypes = field.FieldType.GetGenericArguments();
                if (genericTypes.Length == 0)
                {
                    return false;
                }
                return genericTypes.Single() == valueType;
            })
            .Select(fieldInfo => fieldInfo.Name)
            .ToArray();
    }
}