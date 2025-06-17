using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    [SerializeField] private GameObject _lights;
    private bool _enabled = false;

    public override void Use(GameObject user, IInventory inventory)
    {
        _lights.SetActive(_enabled = !_enabled);
    }
}
