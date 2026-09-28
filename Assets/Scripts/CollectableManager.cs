using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public int Coins = 0;

    public void CoinCollect(int Value)
    {
        Coins += Value;
    }
}
