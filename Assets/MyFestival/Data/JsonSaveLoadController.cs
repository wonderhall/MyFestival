using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonSaveLoadController : MonoBehaviour
{
    public string mName;
    public string mDate;
    public string mAmount;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            SaveData character = new SaveData(mName, mDate, mAmount);
            SaveSystem.Save(character, "save_001");
        }
        if (Input.GetKeyDown("l"))
        {
            SaveData loadData = SaveSystem.Load("save_001");
            Debug.Log(string.Format("LoadData Result => name : {0}, Data : {1}, amount : {2}", loadData.name, loadData.date, loadData.amount));
        }
    }
}
