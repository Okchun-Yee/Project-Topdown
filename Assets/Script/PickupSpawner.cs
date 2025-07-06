using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goidCoinPrefab;
    public void DropItems()
    {
        Instantiate(goidCoinPrefab, transform.position, Quaternion.identity);
    }
}
