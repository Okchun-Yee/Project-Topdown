using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Weaponinfo weaponInfo;
    public Weaponinfo GetWeaponinfo()
    {
        return weaponInfo;
    }
}
