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
    //�н�
    string saveFileName;
    private string saveFilePath;
    //

    [Header("ui root")]
    public Transform contentsRoot;
    [Header("������ ui")]
    public GameObject AddItem_UIPrefab;



    public void GetJsonWithMyList()
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath))
        {
            Directory.CreateDirectory(SaveLoadTemplete.SavePath);

            File.WriteAllText(SaveLoadTemplete.SavePath + "myTemplete.json", null);
        }


        //�н�
        string saveFileName = "myTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
        //
        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        //�ҷ����� ����
        Debug.Log("myArList Load is sucess");
        //��ųʸ�����
        Dictionary<string, CurrentTemplete> DicMyTemp = new Dictionary<string, CurrentTemplete>();
        if (DeserialJson != null)
            foreach (var item in DeserialJson.myTemplete)
            {
                DicMyTemp.Add(item.tempeteName, item);
            }

        //�����̿�����Ʈ �����
        if (contentsRoot.childCount < DicMyTemp.Count)//�̹� ��������ִ��� üũ
        {
            Debug.Log("����Ʈ �����");
            for (int i = 0; i < DicMyTemp.Count; i++)
            {
                if (i < contentsRoot.childCount) continue;
                string str = DeserialJson.myTemplete[i].tempeteName;
                GameObject Newitem = Instantiate(AddItem_UIPrefab);
                Newitem.transform.SetParent(contentsRoot, false);
                Newitem.name = str;
                Newitem.transform.GetChild(1).GetComponent<Text>().text = str;


                ////��ư��� ���̱�
                //Newitem.GetComponent<Button>().onClick.AddListener(delegate { createObjectAtScene(scriptableObject); });


            }
        }//if����
        else Debug.Log("�̸̹�������ִ�");

    }

    ////â���� �ݱ�

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