using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public int Coins = 0;
    public int Score = 0;
    public TextMeshProUGUI CoinCounter;
    public TextMeshProUGUI ScoreCounter;

    public void CoinCollect(int Value)
    {
        Coins += Value;
        CoinCounter.text = Coins.ToString();
    }

    public void ScoreChange(int Value)
    {
        Score += Value;
        ScoreCounter.text = Score.ToString();
    }
}
