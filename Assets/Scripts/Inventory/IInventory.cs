using System;
using System.Collections.Generic;

public interface IInventory
{
    List<Item> Items { get; }
    int MaxSlots { get; }

    event Action OnInventoryUpdated;
    public void Initialize();
    Item SetItem(Item item, int id);
    bool AddItem(Item item);
    bool RemoveItem(int id = -1);
    void SwapItems(int indexA, int indexB);
}