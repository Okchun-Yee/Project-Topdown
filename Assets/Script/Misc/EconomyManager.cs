using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string GOLD_AMOUNT_TEXT = "Gold Amount Text";
    public void UpdateCurrentGold()
    {
        currentGold += 1;
        if (goldText == null)
        {
            goldText = GameObject.Find(GOLD_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        goldText.text = currentGold.ToString("D3");
    }
}
