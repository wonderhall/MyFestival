using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class SaveData
{
    public SaveData(string _Name, string _Date, string _Amount)
    {
        name = _Name;
        date = _Date;
        amount = _Amount;
    }

    public string name;
    public string date;
    public string amount;

}

public static class SaveSystem
{
    //private static string SavePath => Application.persistentDataPath + "/MyFestival/Data/";
    private static string SavePath => Application.dataPath + "/MyFestival/Data/";

    public static void Save(SaveData saveData,string saveFileName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        string saveJson = JsonUtility.ToJson(saveData);
        string saveFilePath = SavePath + saveFileName + ".json";

        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log(saveFileName+"에다 저장 성공");
    }

    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("파일이 경로에 없다");
            return null;
        }
        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);

        return saveData;
    }
}
