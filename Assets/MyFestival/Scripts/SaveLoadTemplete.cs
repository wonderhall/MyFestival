using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

public static class SaveLoadTemplete
{

    public static string SavePath => Application.persistentDataPath + "/JsonData/"; //�������� ��ġ


    /// <summary>
    /// ����� �ִ� �����۵��� json templete���� ����
    /// </summary>
    /// <param name="GetItemList"></param>

    public static void SaveTemplete(List<Transform> GetItemList)
    {

        #region ���� �� ������Ʈ���� CurrentTemplete Ŭ������ ���� ��� out curTem
        //���� �� ������Ʈ���� CurrentTemplete Ŭ������ ���� ���-->
        ItemPositon itemPo = new ItemPositon();
        ItemRotate itemRot = new ItemRotate();
        ItemScale itemScale = new ItemScale();
        ItemTransform ItemTransform = new ItemTransform();
        CurItemList itemTrans = new CurItemList(/*itemPo, itemRot, itemScale*/);
        CurrentTemplete curTem = new CurrentTemplete();
        MyTemplete my_templete = new MyTemplete();

        curTem.tempeteName = Manager.instance.TempName;
        curTem.PreviewName = Manager.instance.date;
        curTem.date = Manager.instance.date;
        curTem.latitude = Manager.instance.latitude;
        curTem.longitude = Manager.instance.longitude;

        for (int i = 0; i < GetItemList.Count; i++)
        {

            CurItemList NewItem = new CurItemList(/*itemPo, itemRot, itemScale*/);//���ο� ���� ������ ����
            ItemTransform newItemTransform = new ItemTransform();//���ο� ���� Ʈ������ ����

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


            NewItem.itemTranform.Add(newItemTransform);//���������ۿ� ���� Ʈ������ �־���.
            curTem.items.Add(NewItem);//���� �����ƿ� ������ ������������ �־��ش�.

            //}
        }// ������Ʈ ������ �� �־���

        my_templete.myTemplete.Add(curTem);
        //<---- 
        #endregion

        //�н�
        string saveFileName = "myTemplete";
        string saveFilePath = SavePath + saveFileName + ".json";
        //

        Dictionary<string, CurrentTemplete> allTemplete = DicByJson(saveFilePath);//���̽��� �о ��ųʸ�ȭ
         allTemplete.Add(curTem.tempeteName, curTem);//��ųʸ��� ���� ���� ���ø� ���ϱ�
        //<---

        //}
        MyTemplete NewMyTemplete = new MyTemplete();
        foreach (var item in allTemplete) NewMyTemplete.myTemplete.Add(item.Value);//������ ���� ���� ���ø��� ����� ��ųʸ���  �־���.
                                                                                   
        #region  before
        //string toJson = JsonConvert.SerializeObject(NewMyTemplete);
        //Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        //FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        //streamSave.Write(data2, 0, data2.Length);
        //streamSave.Close();
        #endregion

        TempleteWriteToJson(NewMyTemplete, saveFilePath);//���� ������� ���ø��� ����Ʈȭ�Ͽ� ����.-->
        //<--


    }

    /// <summary>
    /// ���� ����ϴ� �������� �ݰ� �����Ѵ�
    /// </summary>
    public static void SaveTemplete(MyTemplete myTemp, GameObject page)
    {

        page.SetActive(false);
        string toJson = JsonConvert.SerializeObject(myTemp);
        Byte[] data = Encoding.UTF8.GetBytes(toJson);
        FileStream streamSave = new FileStream(SavePath + "myTemplete.json", FileMode.Create);
        streamSave.Write(data, 0, data.Length);
        streamSave.Close();

    }

    /// <summary>
    /// ���� ���ø��� ������ �ø���.�ϴ� ��Į�� �������� ���� ������ ����
    /// </summary>
    /// <param name="publishTemplete"></param>
    public static void SaveTempleteToShop(CurrentTemplete publishTemplete, GameObject page,string saveFilePath)
    {
        page.SetActive(false);
 
        //

        #region before
        //if (!File.Exists(saveFilePath)) newEmptyData(saveFileName);
        //����Ʈ�� �о�´�.
        //FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        //Byte[] data = new byte[streamOpen.Length];
        //streamOpen.Read(data, 0, data.Length);
        //streamOpen.Close();
        ////

        ////����
        //string fromJson = Encoding.UTF8.GetString(data);
        //MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        //Dictionary<string, CurrentTemplete> allTemplete = new Dictionary<string, CurrentTemplete>();
        //foreach (var item in DeserialJson.myTemplete) //���̽����� �о�� ����Ÿ Ǯ���ֱ�
        //{
        //    allTemplete.Add(item.tempeteName, item);
        //}
        #endregion

        Dictionary<string, CurrentTemplete> allTemplete = DicByJson(saveFilePath);//���̽��� �о ��ųʸ�ȭ
        allTemplete.Add(publishTemplete.tempeteName, publishTemplete);//���� �� ����Ÿ

        MyTemplete NewMyTemplete = new MyTemplete(); // ���̽����� ������ֱ� ���� Ŭ���� ���� ����� �־��ش�
        foreach (var item in allTemplete) NewMyTemplete.myTemplete.Add(item.Value);

        #region before

        //MyTemplete NewMyTemplete = new MyTemplete(); // ���̽����� ������ֱ� ���� Ŭ���� ���� ����� �־��ش�
        //string toJson = JsonConvert.SerializeObject(NewMyTemplete);
        //Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        //FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        //streamSave.Write(data2, 0, data2.Length);
        //streamSave.Close();
        #endregion

        TempleteWriteToJson(NewMyTemplete, saveFilePath);
        Debug.Log("���ø� �ۺ��� �Ϸ�");
    }





    /// <summary>
    /// ���丮�� ������ ���ϰ��Բ� �����Ѵ�
    /// </summary>
    public static void newEmptyData(string FileName)
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath)) Directory.CreateDirectory(SavePath);

        FileStream streamSave = new FileStream(FileName, FileMode.Create);
        MyTemplete newMyTemplete = new MyTemplete();
        string toJson = JsonConvert.SerializeObject(newMyTemplete);
        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();
    }
    /// <summary>
    /// ���̽� �ε� �� 
    /// </summary>
    public static Dictionary<string, CurrentTemplete> DicByJson(string Filepath)
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath) || !File.Exists(Filepath))
            newEmptyData(Filepath);//������ ������� ��ü ����

        //���� �б�-->
        FileStream streamOpen = new FileStream(Filepath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        //<--��ųʸ��� ���� ��ȯ-->
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);//���̽����κ��� ���ø����� Ŭ��������
        Dictionary<string, CurrentTemplete> allTemplete = new Dictionary<string, CurrentTemplete>();//���ο� �� ��ųʸ� ����
        foreach (var item in DeserialJson.myTemplete) allTemplete.Add(item.tempeteName, item);//��ųʸ��� �ҷ��� ���ø� ���ϱ�

        return allTemplete;
    }

    public static MyTemplete MyTempByJson(string Filepath)
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath) || !File.Exists(Filepath))
            newEmptyData(Filepath);//������ ������� ��ü ����

        //���� �б�-->
        FileStream streamOpen = new FileStream(Filepath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        //<--��ųʸ��� ���� ��ȯ-->
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);//���̽����κ��� ���ø����� Ŭ��������
        return DeserialJson;
    }
    public static void TempleteWriteToJson(MyTemplete Templete,string path)
    {
        string toJson = JsonConvert.SerializeObject(Templete);
        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        FileStream streamSave = new FileStream(path, FileMode.Create);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();
    }
}

