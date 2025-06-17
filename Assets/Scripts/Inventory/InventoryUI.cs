using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    private List<GameObject> _slots = new List<GameObject>();
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameObject _slotBackPrefab;
    [SerializeField] private MonoBehaviour _inventory;
    private IInventory _inventoryInterface;

    void Start()
    {
        _inventoryInterface = _inventory as IInventory;

        for (int i = 0; i < _inventoryInterface.MaxSlots; i++)
        {
            GameObject slotBack = Instantiate(_slotBackPrefab, transform);
            GameObject slot = Instantiate(_slotPrefab, slotBack.transform);
            slotBack.name = $"Slot{i}Back";
            slot.name = $"Slot{i}";

            var inventorySlot = slot.GetComponent<InventorySlot>();
            inventorySlot.InventoryReference = _inventoryInterface;
            inventorySlot.Index = i;

            _slots.Add(slot);
        }

        UpdateSlots();
        _inventoryInterface.OnInventoryUpdated += UpdateSlots;
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            var slot = _slots[i].GetComponent<InventorySlot>();
            slot.Item = _inventoryInterface.Items[i];
            slot.UpdateSlotImage();

            if (_inventoryInterface is QuickSlotInventory quickSlotInventory)
            {
                if (i == quickSlotInventory.CurrentSlotIndex)
                {
                    slot.transform.parent.GetComponent<Outline>().enabled= true;
                }
                else
                {
                    slot.transform.parent.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}