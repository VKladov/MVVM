using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ViewBinding
{
    public ViewBase ValueHolder;
    public string PropertyName;

    public TProperty? GetProperty<TProperty>() where TProperty : class
    {
        var holderType = ValueHolder.GetType();
        var propertyInfo = holderType.GetField(PropertyName);
        var value = propertyInfo?.GetValue(ValueHolder);
        try
        {
            return (TProperty) value;
        }
        catch (Exception e)
        {
            
        }

        return null;
    }

    public ListPropertyBase? GetListProperty()
    {
        var holderType = ValueHolder.GetType();
        var propertyInfo = holderType.GetField(PropertyName);
        var value = propertyInfo?.GetValue(ValueHolder);
        return value as ListPropertyBase;
    }

    public Type? GetListItemType()
    {
        var holderType = ValueHolder.GetType();
        var propertyInfo = holderType.GetField(PropertyName);
        var value = propertyInfo?.GetValue(ValueHolder);
        return value?.GetType().GetGenericArguments().Single();
    }
}