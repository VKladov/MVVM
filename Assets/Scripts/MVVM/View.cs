using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class ViewBase : UIBehaviour
{
    public abstract void SubscribeToValueChange();
    public abstract void Unbind();
    public abstract void SetModel();
    public abstract Type GetValueType();
    public abstract Type GetFieldType();
    public abstract void TrySetValue(object? value);
}

public interface IModelView
{
    Transform GameObjectTransform { get; }
    ViewBase ViewBase { get; }
}

public abstract class ViewModel<TModel> : View<TModel>, IModelView
{
    public abstract void OnModelChange(TModel model);
    public override void OnValueChanged(TModel newValue)
    {
        OnModelChange(newValue);
    }

    public Transform GameObjectTransform => transform;
    public ViewBase ViewBase => this;
}

public abstract class View<TValue> : ViewBase
{
    [SerializeField] private ViewBinding _binding = new();

    public ValueEditorHelper<TValue> ValueHelper;
    private Property<TValue>? _property;
    
    public override void SubscribeToValueChange()
    {
        Unbind();
        if (_binding.ValueHolder == null)
        {
            Debug.LogWarning("No model holder for " + gameObject.name);
            return;
        }
        
        _property = _binding.GetProperty<Property<TValue>>();
        if (_property != null)
        {
            _property.Changed += OnValueChanged;
            OnValueChanged(_property.Value);
        }
        else
        {
            // Лист предоставляет значения сам, поэтому не показывам варнинг
            if (_binding.ValueHolder is ListView)
            {
                return;
            }
            
            Debug.LogWarning($"Property with name {_binding.PropertyName} of type {typeof(TValue)} is NULL");
        }
    }

    public override void Unbind()
    {
        if (_property != null)
        {
            _property.Changed -= OnValueChanged;
        }
    }

    protected override void OnDestroy()
    {
        Unbind();
    }

    public override void SetModel()
    {
        OnValueChanged(ValueHelper.Value);
    }

    public override Type GetValueType()
    {
        return typeof(TValue);
    }

    public override Type GetFieldType()
    {
        return typeof(Property<>);
    }

    public abstract void OnValueChanged(TValue? newValue);

    public override void TrySetValue(object? newValue)
    {
        try
        {
            OnValueChanged((TValue) newValue!);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }
}