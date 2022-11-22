using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

public static class SaveLoadTemplete
{

    public static string SavePath => Application.persistentDataPath + "/JsonData/"; //저장폴더 위치


    /// <summary>
    /// 만들고 있는 아이템들을 json templete으로 저장
    /// </summary>
    /// <param name="GetItemList"></param>

    public static void SaveTemplete(List<Transform> GetItemList)
    {

        #region 현재 씬 오브젝트들을 CurrentTemplete 클래스에 정보 담기 out curTem
        //현재 씬 오브젝트들을 CurrentTemplete 클래스에 정보 담기-->
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
        //<---- 
        #endregion

        //패스
        string saveFileName = "myTemplete";
        string saveFilePath = SavePath + saveFileName + ".json";
        //

        Dictionary<string, CurrentTemplete> allTemplete = DicByJson(saveFilePath);//제이슨을 읽어서 딕셔너리화
         allTemplete.Add(curTem.tempeteName, curTem);//딕셔너리에 현재 만든 템플릿 더하기
        //<---

        //}
        MyTemplete NewMyTemplete = new MyTemplete();
        foreach (var item in allTemplete) NewMyTemplete.myTemplete.Add(item.Value);//저장을 위해 새로 템플릿을 만들고 딕셔너리를  넣어줌.
                                                                                   
        #region  before
        //string toJson = JsonConvert.SerializeObject(NewMyTemplete);
        //Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        //FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        //streamSave.Write(data2, 0, data2.Length);
        //streamSave.Close();
        #endregion

        TempleteWriteToJson(NewMyTemplete, saveFilePath);//새로 만들어진 탬플릿을 바이트화하여 저장.-->
        //<--


    }

    /// <summary>
    /// 현재 사용하던 페이지를 닫고 저장한다
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
    /// 현재 탬플릿을 서버에 올린다.일단 로칼로 만들지만 이후 서버로 변경
    /// </summary>
    /// <param name="publishTemplete"></param>
    public static void SaveTempleteToShop(CurrentTemplete publishTemplete, GameObject page,string saveFilePath)
    {
        page.SetActive(false);
 
        //

        #region before
        //if (!File.Exists(saveFilePath)) newEmptyData(saveFileName);
        //바이트로 읽어온다.
        //FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        //Byte[] data = new byte[streamOpen.Length];
        //streamOpen.Read(data, 0, data.Length);
        //streamOpen.Close();
        ////

        ////쓰기
        //string fromJson = Encoding.UTF8.GetString(data);
        //MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        //Dictionary<string, CurrentTemplete> allTemplete = new Dictionary<string, CurrentTemplete>();
        //foreach (var item in DeserialJson.myTemplete) //제이슨에서 읽어온 데이타 풀어주기
        //{
        //    allTemplete.Add(item.tempeteName, item);
        //}
        #endregion

        Dictionary<string, CurrentTemplete> allTemplete = DicByJson(saveFilePath);//제이슨을 읽어서 딕셔너리화
        allTemplete.Add(publishTemplete.tempeteName, publishTemplete);//새로 쓸 데이타

        MyTemplete NewMyTemplete = new MyTemplete(); // 제이슨으로 만들어주기 위해 클래스 새로 만들고 넣어준다
        foreach (var item in allTemplete) NewMyTemplete.myTemplete.Add(item.Value);

        #region before

        //MyTemplete NewMyTemplete = new MyTemplete(); // 제이슨으로 만들어주기 위해 클래스 새로 만들고 넣어준다
        //string toJson = JsonConvert.SerializeObject(NewMyTemplete);
        //Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        //FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        //streamSave.Write(data2, 0, data2.Length);
        //streamSave.Close();
        #endregion

        TempleteWriteToJson(NewMyTemplete, saveFilePath);
        Debug.Log("템플릿 퍼블리싱 완료");
    }





    /// <summary>
    /// 디렉토리가 없으면 파일과함께 생성한다
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
    /// 제이슨 로드 후 
    /// </summary>
    public static Dictionary<string, CurrentTemplete> DicByJson(string Filepath)
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath) || !File.Exists(Filepath))
            newEmptyData(Filepath);//파일이 없을경우 대체 생성

        //파일 읽기-->
        FileStream streamOpen = new FileStream(Filepath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        //<--딕셔너리로 파일 변환-->
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);//제이슨으로부터 템플릿모음 클래스생성
        Dictionary<string, CurrentTemplete> allTemplete = new Dictionary<string, CurrentTemplete>();//새로운 빈 딕셔너리 생성
        foreach (var item in DeserialJson.myTemplete) allTemplete.Add(item.tempeteName, item);//딕셔너리에 불러온 템플릿 더하기

        return allTemplete;
    }

    public static MyTemplete MyTempByJson(string Filepath)
    {
        if (!Directory.Exists(SaveLoadTemplete.SavePath) || !File.Exists(Filepath))
            newEmptyData(Filepath);//파일이 없을경우 대체 생성

        //파일 읽기-->
        FileStream streamOpen = new FileStream(Filepath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        //<--딕셔너리로 파일 변환-->
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);//제이슨으로부터 템플릿모음 클래스생성
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

