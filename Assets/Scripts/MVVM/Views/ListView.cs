using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListView : ViewBase, IModelView
{
    [SerializeField] private ViewBase? _itemPrefab;
    [SerializeField] private ViewBinding _binding = new();

    private ListPropertyBase? _listProperty;
    private List<ViewBase> _items = new List<ViewBase>();

    public override void SubscribeToValueChange()
    {
        Unbind();
        var propertyItemType = _binding.GetListItemType();
        if (propertyItemType == null)
        {
            Debug.LogWarning("Missing binding for " + gameObject.name);
            return;
        }
        
        if (_itemPrefab == null)
        {
            Debug.LogWarning("Missing item prefab in ListView " + gameObject.name);
            return;
        }
        
        var prefabValueType = _itemPrefab.GetValueType();
        if (propertyItemType != prefabValueType)
        {
            Debug.LogWarning($"Property item type ({propertyItemType.Name}) not equal to prefab value type ({prefabValueType.Name})");
            return;
        }

        _listProperty = _binding.GetListProperty();
        if (_listProperty != null)
        {
            _listProperty.ValuesChanged += UpdateList;
            UpdateList();
        }
    }

    private void UpdateList()
    {
        if (_listProperty == null)
        {
            Debug.LogWarning("Missing property fto read from");
            return;
        }
        
        if (_itemPrefab == null)
        {
            Debug.LogWarning("Missing item prefab for ListView " + gameObject.name);
            return;
        }
        
        if (_itemPrefab.transform.parent != transform)
        {
            Debug.LogWarning("Item prefab must be child");
            return;
        }


        if (Application.isPlaying)
        {
            if (_items.Count == 0)
            {
                _items.Add(_itemPrefab);
                var firstItemTransform = _itemPrefab.transform;
                foreach (Transform child in transform)
                {
                    if (child == firstItemTransform)
                    {
                        continue;
                    }
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            _items.Clear();
            _items.Add(_itemPrefab);
            _itemPrefab.transform.SetSiblingIndex(0);
            while (transform.childCount > 1)
            {
                var child = transform.GetChild(transform.childCount - 1);
                if (child.TryGetComponent(out ViewBase viewBase))
                {
                    viewBase.Unbind();
                }
                DestroyImmediate(child.gameObject);
            }
        }

        var max = Math.Max(_items.Count, _listProperty.Count);
        var itemsToRemove = new List<ViewBase>();
        for (var i = 0; i < max; i++)
        {
            var existInProperty = i < _listProperty.Count;
            var existInItems = i < _items.Count;
            if (existInItems && !existInProperty)
            {
                if (i > 0)
                {
                    itemsToRemove.Add(_items[i]);
                }
                else
                {
                    _items[i].gameObject.SetActive(false);
                }
            }
            else if (existInProperty && !existInItems)
            {
                var newItem = Instantiate(_itemPrefab, transform);
                var itemViews = newItem.transform.GetComponentsInChildren<ViewBase>();
                if (itemViews != null)
                {
                    foreach (var itemView in itemViews)
                    {
                        itemView.SubscribeToValueChange();
                    }
                }
                _items.Add(newItem);
            }
        }

        foreach (var itemToRemove in itemsToRemove)
        {
            _items.Remove(itemToRemove);
            itemToRemove.Unbind();
            if (Application.isPlaying)
            {
                Destroy(itemToRemove.gameObject);
            }
            else
            {
                DestroyImmediate(itemToRemove.gameObject);
            }
        }

        for (var i = 0; i < _listProperty.Count; i++)
        {
            _items[i].gameObject.SetActive(true);
            _items[i].TrySetValue(_listProperty.GetValue(i));
        }
    }

    public override void Unbind()
    {
        if (_listProperty != null)
        {
            _listProperty.ValuesChanged -= UpdateList;
        }
    }

    public override void SetModel()
    {
        
    }

    public override Type GetValueType()
    {
        return _itemPrefab == null ? null : _itemPrefab.GetValueType();
    }

    public override Type GetFieldType()
    {
        return typeof(ListProperty<>);
    }


    public override void TrySetValue(object? value)
    {
        
    }

    public Transform GameObjectTransform => transform;
    public ViewBase ViewBase => this;
}