using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MyARList : MonoBehaviour
{
    //패스
    string saveFileName;
    private string saveFilePath;
    //

    [Header("ui root")]
    public Transform contentsRoot;
    [Header("생성할 ui")]
    public GameObject AddItem_UIPrefab;



    public void GetJsonWithMyList()
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath))
        {
            Directory.CreateDirectory(SaveLoadTemplete.SavePath);

            File.WriteAllText(SaveLoadTemplete.SavePath + "myTemplete.json", null);
        }


        //패스
        string saveFileName = "myTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
        //
        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        //불러오기 성공
        Debug.Log("myArList Load is sucess");
        //딕셔너리저장
        Dictionary<string, CurrentTemplete> DicMyTemp = new Dictionary<string, CurrentTemplete>();
        if (DeserialJson != null)
            foreach (var item in DeserialJson.myTemplete)
            {
                DicMyTemp.Add(item.tempeteName, item);
            }

        //유아이오브젝트 만들기
        if (contentsRoot.childCount < DicMyTemp.Count)//이미 만들어져있는지 체크
        {
            Debug.Log("리스트 만들기");
            for (int i = 0; i < DicMyTemp.Count; i++)
            {
                if (i < contentsRoot.childCount) continue;
                string str = DeserialJson.myTemplete[i].tempeteName;
                GameObject Newitem = Instantiate(AddItem_UIPrefab);
                Newitem.transform.SetParent(contentsRoot, false);
                Newitem.name = str;
                Newitem.transform.GetChild(1).GetComponent<Text>().text = str;


                ////버튼기능 붙이기
                //Newitem.GetComponent<Button>().onClick.AddListener(delegate { createObjectAtScene(scriptableObject); });


            }
        }//if종료
        else Debug.Log("이미만들어져있다");

    }

    ////창열고 닫기

    //public void switchPage(int idx)
    //{
    //    switch (idx)
    //    {
    //        case (0):
    //            PagesToChange[0].SetActive(true);
    //            PagesToChange[1].SetActive(false);
    //            break;

    //        case (1):
    //            PagesToChange[0].SetActive(false);
    //            PagesToChange[1].SetActive(true);
    //            break;

    //        default:
    //            PagesToChange[0].SetActive(true);
    //            PagesToChange[1].SetActive(false);
    //            break;
    //    }
    //}


}