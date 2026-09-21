using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingTest : MonoBehaviour
{
    public int Level = 1;
    float Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Level = PlayerPrefs.GetInt("Level");
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 30)
        {
            Timer = 0;
            Increase();
        }
    }

    void Increase()
    {
        PlayerPrefs.SetInt("Level", Level);
        Level = PlayerPrefs.GetInt("Level");
    }
}
