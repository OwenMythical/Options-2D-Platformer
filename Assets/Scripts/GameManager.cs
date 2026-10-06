using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string CurrentScene = "Plains 1";
    public Vector3 TransitionPos = new Vector3(0, 0, 0);

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(gameObject);
    }

    public void Save(ref GameSaveData Data)
    {
        Data.SceneName = CurrentScene;
        Data.TransitionPos = TransitionPos;
        Debug.Log(Data.SceneName);
        Debug.Log(Data.TransitionPos);
    }

    public void Load(GameSaveData Data)
    {
        SceneManager.LoadScene(Data.SceneName);
        StartCoroutine(LoadPlayerPos(Data.TransitionPos));
        Debug.Log(Data.SceneName);
        Debug.Log(Data.TransitionPos);
    }

    IEnumerator LoadPlayerPos(Vector3 Pos)
    {
        yield return new WaitForSeconds(0f);
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = Pos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveManager.Save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SaveManager.Load();
        }
    }

    public PlayerController PC {get; set;}
}

[System.Serializable]
public struct GameSaveData
{
    public string SceneName;
    public Vector3 TransitionPos;
}