using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    void Update()
    {
        if (Input.GetButtonDown("UseItem"))
        {
            _inventoryManager.QuickSlotInventory.UseCurrentItem(gameObject);
        }

        if (Input.GetButtonDown("PickupItem"))
        {
            _inventoryManager.PickupItem();
        }

        if (Input.GetButtonDown("DropItem"))
        {
            _inventoryManager.DropItem();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                _inventoryManager.QuickSlotInventory.SetActiveSlot(_inventoryManager.QuickSlotInventory.CurrentSlotIndex + 1);
            }
            else
            {
                _inventoryManager.QuickSlotInventory.SetActiveSlot(_inventoryManager.QuickSlotInventory.CurrentSlotIndex - 1);
            }
        }
    }
}
