using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goidCoin, heathGlobe, staminaGlobe;
    public void DropItems()
    {
        int randomNum = Random.Range(1, 5);
        if (randomNum == 1)
        {
            Instantiate(heathGlobe, transform.position, Quaternion.identity);
        }
        else if (randomNum == 2)
        {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        else if (randomNum == 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);
            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goidCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
