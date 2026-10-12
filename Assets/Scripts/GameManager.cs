using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject Player;
    PlayerController PC;
    public string SceneName = "Plains 1";
    public int SpawnID = -1;

    public AudioClip PlainsClip;
    public AudioClip ForestClip;
    AudioSource AS;

    public Image TransitionUI;

    private void Start()
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

    public void Save(ref GameSaveData Data)
    {
        Data.SceneName = SceneName;
        Data.SpawnID = SpawnID;
        Debug.Log(Data.SceneName);
        Debug.Log(Data.SpawnID);
    }

    public void Load(GameSaveData Data)
    {
        SceneName = Data.SceneName;
        SpawnID = Data.SpawnID;
        StartCoroutine(LoadScene(Data.SpawnID,Data.SceneName,true));
        Debug.Log(Data.SceneName);
        Debug.Log(Data.SpawnID);
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

        PC.TransitionStun = true;
        if (Scene == "")
        {
            SceneManager.LoadScene("Plains 1");
        }
        else
        {
            SceneManager.LoadScene(Scene);
        }
        yield return new WaitForSeconds(0f);
        GameObject SpawnObject = null;
        foreach (GameObject SObj in GameObject.FindGameObjectsWithTag("SpawnObject"))
        {
            if (SObj.GetComponent<TransitionScript>().ID == ID)
            {
                SpawnObject = SObj;
            }
        }
        if (SpawnObject == null)
        {
            SpawnObject = GameObject.FindGameObjectWithTag("DefaultSpawnObject");
        }
        Player.transform.position = SpawnObject.transform.position;

        if (SceneName.Contains("Plains"))
        {
            if (AS.clip != PlainsClip)
            {
                AS.clip = PlainsClip;
                AS.Play();
            }
        }
        if (SceneName.Contains("Forest"))
        {
            if (AS.clip != ForestClip)
            {
                AS.clip = ForestClip;
                AS.Play();
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
        SceneName = Scene;
        SpawnID = ID;
        SaveManager.Save();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneName = "Plains 1";
            SpawnID = -1;
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
}