using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

public static class SaveLoadTemplete
{

    public static string SavePath => Application.dataPath + "/MyFestival/JsonData/";

    // Start is called before the first frame update


    public static void SaveTemplete(List<Transform> GetItemList)
    {


        Transform itemRoot = GameObject.Find("---CurrentItemList---").transform;

        //for (int i = 0; i < itemRoot.childCount; i++)
        //{
        //    if (itemRoot.childCount == 0) return;

        //    currentItems.Add(itemRoot.GetChild(i));
        //}
        ItemPositon itemPo = new ItemPositon();
        ItemRotate itemRot = new ItemRotate();
        ItemScale itemScale = new ItemScale();
        ItemTransform ItemTransform = new ItemTransform();
        CurItemList itemTrans = new CurItemList(/*itemPo, itemRot, itemScale*/);
        CurrentTemplete curTem = new CurrentTemplete();
        MyTemplete my_templete = new MyTemplete();

        curTem.tempeteName = Manager.instance.date;
        curTem.date = Manager.instance.date;
        curTem.latitude = Manager.instance.latitude;
        curTem.longitude = Manager.instance.longitude;

        for (int i = 0; i < GetItemList.Count; i++)
        {

            CurItemList NewItem = new CurItemList(/*itemPo, itemRot, itemScale*/);//새로운 탬프 아이템 생성
            ItemTransform newItemTransform = new ItemTransform();//새로운 탬프 트랜스폼 생성

            string[] tempname = GetItemList[i].name.Split('_');
            NewItem.Index = int.Parse(tempname[1]);
            NewItem.ItemName = GetItemList[i].name;
            newItemTransform.itemPositon[0] = GetItemList[i].localPosition.x;
            newItemTransform.itemPositon[1] = GetItemList[i].localPosition.y;
            newItemTransform.itemPositon[2] = GetItemList[i].localPosition.z;

            newItemTransform.itemRotate[0] = GetItemList[i].localRotation.x;
            newItemTransform.itemRotate[1] = GetItemList[i].localRotation.y;
            newItemTransform.itemRotate[2] = GetItemList[i].localRotation.z;

            newItemTransform.itemScale[0] = GetItemList[i].localScale.x;
            newItemTransform.itemScale[1] = GetItemList[i].localScale.y;
            newItemTransform.itemScale[2] = GetItemList[i].localScale.z;


            NewItem.itemTranform.Add(newItemTransform);//탬프아이템에 탬프 트랜스폼 넣어줌.
            curTem.items.Add(NewItem);//상위 아이탬에 변경한 탬프아이템을 넣어준다.

            //}
        }// 오프젝트 변형된 값 넣어줌

        my_templete.myTemplete.Add(curTem);
        //패스
        string saveFileName = "myTemplete";
        string saveFilePath = SavePath + saveFileName + ".json";
        //



        bool isDataNone = false;
        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();

        MyTemplete NewMyTemplete = new MyTemplete();

        if (data.Length == 0)
        {
            isDataNone = true;
        }
        else
        {
            isDataNone = false;

            string fromJson = Encoding.UTF8.GetString(data);
            MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
            Dictionary<string, CurrentTemplete> allTemplete = new Dictionary<string, CurrentTemplete>();
            foreach (var item in DeserialJson.myTemplete)
            {
                allTemplete.Add(item.tempeteName, item);
            }
            allTemplete.Add(curTem.tempeteName, curTem);


            foreach (var item in allTemplete)
            {
                NewMyTemplete.myTemplete.Add(item.Value);
            }
        }

        string toJson;
        //저장 file 리드올 라이트올로 사용시 오류가 생겨서 byte로 전환후 저장
        if (isDataNone)
        {
            toJson = JsonConvert.SerializeObject(my_templete);
        }
        else
        {
            toJson = JsonConvert.SerializeObject(NewMyTemplete);
        }

        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();
        //


    }

}

