using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    public int Coins = 0;
    public int Score = 0;
    public TextMeshProUGUI CoinCounter;
    public TextMeshProUGUI ScoreCounter;
    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

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