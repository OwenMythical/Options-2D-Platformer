using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveData SData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public GameSaveData GSD;
    }

    public static string SaveFileName()
    {
        string SaveFile = Application.persistentDataPath + "/save" + ".save";
        return SaveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(SData, true));
    }

    static void HandleSaveData()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GM.Save(ref SData.GSD);
    }

    public static void Load()
    {
        string SaveContent = File.ReadAllText(SaveFileName());

        SData = JsonUtility.FromJson<SaveData>(SaveContent);
        HandleLoadData();
    }

    static void HandleLoadData()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GM.Load(SData.GSD);
    }
}
