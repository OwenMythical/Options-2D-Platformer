using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject Player;
    PlayerController PC;
    public string SceneName = "Plains 1";
    public int SpawnID = -1;
    //public List<GlobalObjectId> CollectedCoins = new List<GlobalObjectId>();
    public List<int> SecretCoins = new List<int>();

    public AudioClip PlainsClip;
    public AudioClip ForestClip;
    AudioSource AS;

    public int Coins = 0;
    public int Score = 0;
    public TextMeshProUGUI CoinCounter;
    public TextMeshProUGUI ScoreCounter;

    public TextMeshProUGUI SavingText;
    public Image TransitionUI;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PC = Player.GetComponent<PlayerController>();
            AS = gameObject.GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Player);

            SaveManager.Load();
        }
    }

    public void CoinCollect(int Value, GameObject Coin, int CoinID)
    {
        Coins += Value;
        CoinCounter.text = Coins.ToString();
        //GlobalObjectId ID = GlobalObjectId.GetGlobalObjectIdSlow(Coin);

        if (CoinID > 0)
        {
            SecretCoins.Add(CoinID);
        }
        else
        {
            //CollectedCoins.Add(ID);
        }
    }

    public void ScoreChange(int Value)
    {
        Score += Value;
        ScoreCounter.text = Score.ToString();
    }

    public void Save(ref GameSaveData Data)
    {
        SavingText.text = "Saving...";
        SavingText.enabled = true;
        Data.SceneName = SceneName;
        Data.SpawnID = SpawnID;
        Data.Score = Score;
        Data.Coins = Coins;
        Data.SecretCoins = SecretCoins;
        Debug.Log(Data.SceneName);
        StartCoroutine(SaveText());
    }

    public void Load(GameSaveData Data)
    {
        SceneName = Data.SceneName;
        SpawnID = Data.SpawnID;
        Score = Data.Score;
        ScoreCounter.text = Score.ToString();
        Coins = Data.Coins;
        CoinCounter.text = Coins.ToString();
        SecretCoins = Data.SecretCoins;
        StartCoroutine(LoadScene(Data.SpawnID, Data.SceneName, true));
        Debug.Log(Data.SceneName);
    }

    public void RespawnLoad()
    {
        StartCoroutine(LoadScene(SpawnID, SceneName, false));
    }

    IEnumerator SaveText()
    {
        SavingText.text = "Saved!";
        yield return new WaitForSeconds(2f);
        SavingText.enabled = false;
    }

    IEnumerator LoadScene(int ID, string Scene, bool InstantTransition)
    {
        byte A = 0;
        if (InstantTransition == false)
        {
            A = 0;
            for (int i = 0; i < 10; i++)
            {
                A += 25;
                TransitionUI.color = new Color32(0, 0, 0, A);
                yield return new WaitForSeconds(0.1f);
            }
        }
        TransitionUI.color = new Color32(0, 0, 0, 255);

        PC.Health = PC.MaxHealth;
        PC.TransitionStun = true;
        //Scene Loading
        if (Scene == "")
        {
            SceneManager.LoadScene("Plains 1");
        }
        else
        {
            SceneManager.LoadScene(Scene);
        }
        yield return new WaitForSeconds(0f);
        //Player Position
        GameObject SpawnObject = null;
        foreach (GameObject SObj in GameObject.FindGameObjectsWithTag("SpawnObject"))
        {
            if (SObj.GetComponent<TransitionScript>().ID == ID)
            {
                SpawnObject = SObj;
            }
        }
        foreach (GameObject SObj in GameObject.FindGameObjectsWithTag("SaveObject"))
        {
            if (SObj.GetComponent<SaveScript>().ID == ID)
            {
                SpawnObject = SObj;
            }
        }
        if (SpawnObject == null)
        {
            SpawnObject = GameObject.FindGameObjectWithTag("DefaultSpawnObject");
        }
        Player.transform.position = SpawnObject.transform.position;
        //Music
        if (Scene.Contains("Plains"))
        {
            if (AS.clip != PlainsClip)
            {
                AS.clip = PlainsClip;
                AS.Play();
            }
        }
        if (Scene.Contains("Forest"))
        {
            if (AS.clip != ForestClip)
            {
                AS.clip = ForestClip;
                AS.Play();
            }
        }
        //Collected Coins
        //if (CollectedCoins != null)
        //{
        //    foreach (GameObject C in GameObject.FindGameObjectsWithTag("Coin"))
        //    {
        //        foreach (GlobalObjectId CID in CollectedCoins)
        //        {
        //            if (CID.Equals(GlobalObjectId.GetGlobalObjectIdSlow(C)))
        //            {
        //                C.GetComponent<SpriteRenderer>().enabled = false;
        //                C.GetComponent<Collider2D>().enabled = false;
        //            }
        //        }
        //    }
        //}
        if (SecretCoins != null)
        {
            foreach (GameObject C in GameObject.FindGameObjectsWithTag("SecretCoin"))
            {
                foreach (int CID in SecretCoins)
                {
                    if (CID.Equals(C.GetComponent<CoinScript>().ID))
                    {
                        C.GetComponent<SpriteRenderer>().enabled = false;
                        C.GetComponent<Collider2D>().enabled = false;
                    }
                }
            }
        }

        A = 255;
        for (int i = 10; i > 0; i--)
        {
            A -= 25;
            TransitionUI.color = new Color32(0, 0, 0, A);
            yield return new WaitForSeconds(0.1f);
        }
        TransitionUI.color = new Color32(0, 0, 0, 0);
        PC.TransitionStun = false;
    }

    public void SceneTransition(string Scene, int ID)
    {
        StartCoroutine(LoadScene(ID,Scene,false));
    }

    public void SaveGame(string Scene, int ID)
    {
        SceneName = Scene;
        SpawnID = ID;
        //CollectedCoins = new List<GlobalObjectId>();
        foreach (GameObject C in GameObject.FindGameObjectsWithTag("Coin"))
        {
            if (C.GetComponent<SpriteRenderer>().enabled == false)
            {
                //GlobalObjectId CID = GlobalObjectId.GetGlobalObjectIdSlow(C);
                //CollectedCoins.Add(CID);
            }
        }
        PC.Health = PC.MaxHealth;
        SaveManager.Save();
    }

    public void DemoEnd()
    {
        SceneManager.LoadScene("DemoEnd");
        GameObject.Destroy(Player);
        GameObject.Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneName = "Plains 1";
            SpawnID = -1;
            //CollectedCoins = new List<GlobalObjectId>();

            Coins = 0;
            Score = 0;
            CoinCounter.text = Coins.ToString();
            ScoreCounter.text = Score.ToString();
            StartCoroutine(LoadScene(-1,"Plains 1",true));
            SaveManager.Save();
        }
    }
}

[System.Serializable]
public struct GameSaveData
{
    public string SceneName;
    public int SpawnID;
    public int Coins;
    public int Score;
    public List<int> SecretCoins;
}