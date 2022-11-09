using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

[Serializable]
public class JtestList
{
    public List<jtest1> jtestLsit = new List<jtest1>();
}
[Serializable]
public class jtest1
{
    public string jtest1Name;
    public int num1;
    public int num2;
    public jtest1()
    {
        jtest1Name = DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss");
        num1 = 1;
        num2 = 2;
    }

}
public class SaveData : MonoBehaviour
{
    string saveFileName;
    private string saveFilePath;

    private void Start()
    {
        saveFileName = "jTest1";
        saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) saveJsonTest1();
        if (Input.GetKeyDown(KeyCode.O)) overRideJson();


    }
    void saveJsonTest1()
    {
        jtest1 jt = new jtest1();
        JtestList jtList = new JtestList();


        jtList.jtestLsit.Add(jt);
        jtList.jtestLsit.Add(jt);



        FileStream streamSave = new FileStream(saveFilePath, FileMode.OpenOrCreate);
        string toJson = JsonConvert.SerializeObject(jtList);
        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();
        Debug.Log(toJson);
    }
    void overRideJson()
    {
        jtest1 _jtest1 = new jtest1();

        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        string fromJson = Encoding.UTF8.GetString(data);
        JtestList DeserialJson = JsonConvert.DeserializeObject<JtestList>(fromJson);


        Dictionary<string, jtest1> allTemplete = new Dictionary<string, jtest1>();
        //Dictionary<string, JtestList> allTemplete = new Dictionary<string,JtestList>();
        foreach (var item in DeserialJson.jtestLsit)
        {
            allTemplete.Add(item.jtest1Name, item);
        }

        allTemplete.Add(_jtest1.jtest1Name, _jtest1);

        JtestList NewJtestList = new JtestList();
        foreach (var item in allTemplete)
        {
            NewJtestList.jtestLsit.Add(item.Value);
        }

        Debug.Log(allTemplete);
        string toJson = JsonConvert.SerializeObject(NewJtestList);
        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        FileStream streamSave = new FileStream(saveFilePath, FileMode.OpenOrCreate);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();
    }
}

