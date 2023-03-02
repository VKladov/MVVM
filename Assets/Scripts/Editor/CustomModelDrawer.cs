using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ValueEditorHelper<>))]
public class ModelHolderDrawer : PropertyDrawer
{
    private float _height = 20;
    private SerializedProperty _property;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var component = property.serializedObject.targetObject as ViewBase;
        if (!BindingDrawer.IsPrefabRoot(component))
        {
            return;
        }
        
        _property = property.FindPropertyRelative("Value");
        var height = EditorGUI.GetPropertyHeight(_property, true);
        EditorGUI.PropertyField(position, _property, true);

        var buttonRect = new Rect(position.x, position.y + height + 5, position.width, 25);
        if (GUI.Button(buttonRect, "Refresh"))
        {
            var views = component.gameObject.GetComponentsInChildren<ViewBase>(true);
            if (views != null)
            {
                foreach (var view in views.Where(view => view != component))
                {
                    if (view)
                    {
                        view.Unbind();
                    }
                }
                component.SetModel();
                foreach (var view in views.Where(view => view != component))
                {
                    if (view)
                    {
                        view.SubscribeToValueChange();
                    }
                }
            }
            EditorUtility.SetDirty(component);
        }

    }

    private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (_property == null)
        {
            return 0;
        }
        return EditorGUI.GetPropertyHeight(_property, true) + 30;
    }
}