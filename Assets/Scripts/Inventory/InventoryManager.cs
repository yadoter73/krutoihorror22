using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private Transform _itemHolder;

    private QuickSlotInventory _quickSlotInventory;
    

    public QuickSlotInventory QuickSlotInventory => _quickSlotInventory;


    private void Awake()
    {
        _quickSlotInventory = GetComponentInChildren<QuickSlotInventory>();
 
        _quickSlotInventory.Initialize();

    }
    public void PickupItem()
    {
        Vector3 cameraPoint = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(cameraPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                if (_quickSlotInventory.AddItem(item))
                    item.PickupItem(_itemHolder);
            }
        }
        
    }

    public void DropItem()
    {
       
        Item item = _quickSlotInventory.Items[_quickSlotInventory.CurrentSlotIndex];
        Rigidbody rigidbody = item.GetComponent<Rigidbody>();
        if ( _quickSlotInventory.RemoveItem())
        {
            item.DropItem();
            rigidbody.AddForce(Camera.main.transform.forward * 100);
        }
    }
}
