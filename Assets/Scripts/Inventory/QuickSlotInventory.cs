using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotInventory : MonoBehaviour, IInventory
{
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private int _maxSlots = 8;
    private int _currentSlotIndex = 0;

    public int CurrentSlotIndex => _currentSlotIndex;
    public List<Item> Items => _items;
    public int MaxSlots => _maxSlots;
    public event Action OnInventoryUpdated;

    public void Initialize()
    {
        while (_items.Count < _maxSlots)
        {
            _items.Add(null);
        }
    }

    public void UseCurrentItem(GameObject user)
    {
        if (_items.Count > 0 && _currentSlotIndex >= 0 && _currentSlotIndex < _items.Count && _items[_currentSlotIndex] != null)
        {
            _items[_currentSlotIndex].Use(user, this);
            OnInventoryUpdated?.Invoke();
        }
    }

    public void SetActiveSlot(int index)
    {
        index = ((index % _maxSlots) + _maxSlots) % _maxSlots;

        if (_items[_currentSlotIndex] != null)
            _items[_currentSlotIndex].gameObject.SetActive(false);

        _currentSlotIndex = index;

        if (_items[_currentSlotIndex] != null)
            _items[_currentSlotIndex].gameObject.SetActive(true);

        OnInventoryUpdated?.Invoke();
    }


    public Item SetItem(Item item, int id)
    {
        if (id < 0 || id >= _items.Count)
            return null;

        Item oldItem = _items[id];
        
        _items[id] = item;

        if (oldItem != null)
        {
            oldItem.gameObject.SetActive(false);
        }

        SetActiveSlot(_currentSlotIndex);

        OnInventoryUpdated?.Invoke();
        return oldItem;
    }

    public bool AddItem(Item item)
    {
        int index = _currentSlotIndex;
        if (_items[index] == null)
        {
            _items[index] = item;
            OnInventoryUpdated?.Invoke();
            return true;
        }
        else
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = item;
                    OnInventoryUpdated?.Invoke();
                    return true;
                }
            }
        }
        return false;
    }
    public void SwapItems(int indexA, int indexB)
    {


        (Items[indexA], Items[indexB]) = (Items[indexB], Items[indexA]);

        if (Items[indexB] == null)
        {
            Items[indexA].gameObject.SetActive(false);
        }

        SetActiveSlot(_currentSlotIndex);

        OnInventoryUpdated?.Invoke();
    }
    public bool RemoveItem(int id = -1)
    {
        int index = id < 0 ? _currentSlotIndex : id;
        if (_currentSlotIndex >= 0 && _currentSlotIndex < _items.Count && _items[_currentSlotIndex] != null)
        {
            _items[_currentSlotIndex] = null;
            OnInventoryUpdated?.Invoke();
            return true;
        }
        return false;
    }
}