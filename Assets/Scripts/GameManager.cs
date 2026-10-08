using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject Player;
    public string SceneName = "Plains 1";
    public int SpawnID = -1;

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
        if (Data.SceneName == "")
        {
            SceneManager.LoadScene("Plains 1");
        }
        else
        {
            SceneManager.LoadScene(Data.SceneName);
        }
        StartCoroutine(LoadScene(Data.SpawnID));
        Debug.Log(Data.SceneName);
        Debug.Log(Data.SpawnID);
    }

    IEnumerator LoadScene(int ID)
    {
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
    }

    public void SceneTransition(string Scene, int ID)
    {
        SceneManager.LoadScene(Scene);
        StartCoroutine(LoadScene(ID));
        SceneName = Scene;
        SpawnID = ID;
        SaveManager.Save();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Plains 1");
            StartCoroutine(LoadScene(-1));
            SceneName = "Plains 1";
            SpawnID = -1;
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