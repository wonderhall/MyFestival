using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class CreateParsingObj : MonoBehaviour
{
    public CurrentTemplete C_Temp = new CurrentTemplete();
    public string JsonFileName;
    [Space(10)]
    public ScriptableObject_CategoryItems S_object;
    public List<Transform> itemList;


    public List<string> tempObjectList = new List<string>();
    public Transform root;
    private void Start()
    {
        //�����ۻ���//
        //SaveLoadTemplete.SaveTemplete(itemList);


        CreateTempleteFromJson<CreateTemplete>();





        //foreach (var itemlist in templete.items)
        //{
        //    itemDic.Add(itemlist.ID, itemlist.itemTranform);
        //}

        //foreach (KeyValuePair<int,List<ItemTransform>> val in itemDic)
        //{
        //    Debug.Log(val.Value);
        //}
    }
    public void CreateTempleteFromJson<T>()
    {
        //����Ÿ �о����
        string data = SaveLoadTemplete.SavePath + JsonFileName + ".json";
        CurrentTemplete templete = DataFromPath<CurrentTemplete>(data);
        //-->��ųʸ� ����
        Dictionary<string, List<ItemTransform>> itemDic = new Dictionary<string, List<ItemTransform>>();
        foreach (var itemlist in templete.items)
        {
            itemDic.Add(itemlist.ItemName, itemlist.itemTranform);
        }
        //<--
        if (!GameObject.Find("---CurrentItemList---"))
        {
            GameObject rootobj = new GameObject("---CurrentItemList---");
            root = rootobj.transform;
        }
        else root = GameObject.Find("---CurrentItemList---").transform;//������ ������Ʈ �θ�

        //List<int> tempObjecs = new List<int>();
        foreach (var key in itemDic)
        {
            string dicName = key.Key;
            string[] tempName= dicName.Split('_');
            string itemName = tempName[0];
            tempObjectList.Add(itemName);
            //Transform newItem= Instantiate(S_object.placeableObjects.itemName,)

        }






    }

    //�𾾸��������//
    public T DataFromPath<T>(string path)
    {
        try
        {
            string rawJson;
            using (var reader = new StreamReader(path))
            {
                rawJson = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(rawJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return default(T);
        }
    }
}


