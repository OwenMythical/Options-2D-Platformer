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
    public Vector3 TransitionPos = new Vector3(-3, 0.5f, 0);

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
        Data.SceneName = SceneManager.GetActiveScene().name;
        Data.TransitionPos = TransitionPos;
        Debug.Log(Data.SceneName);
        Debug.Log(Data.TransitionPos);
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
        StartCoroutine(LoadScene(Data.TransitionPos));
        Debug.Log(Data.SceneName);
        Debug.Log(Data.TransitionPos);
    }

    IEnumerator LoadScene(Vector3 Pos)
    {
        yield return new WaitForSeconds(0f);
        Player.transform.position = Pos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveManager.Save();
        }
    }
}

[System.Serializable]
public struct GameSaveData
{
    public string SceneName;
    public Vector3 TransitionPos;
}