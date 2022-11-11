using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.UI;

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
    private string saveSshotFilePath;
    public Sprite targetSprite;
    public Texture2D snapshot;
    public Image screenImage;

    private void Start()
    {
        //string TextureName = DateTime.Now.ToString("yyyy.MM.dd_hh:mm:ss");
        //string TextureName = "testSshot";

        //saveFilePath = SaveLoadTemplete.SavePath + TextureName + ".json";
        saveFilePath = SaveLoadTemplete.SavePath + "/" + DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss")+".json";
        saveSshotFilePath = SaveLoadTemplete.SavePath + "/"+DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss") + ".png";




        //프리뷰로드
        if (File.Exists(saveSshotFilePath))//파일유무체크
        {
        Texture2D tex = null;    //빈 택스쳐 생성후 바이트로 로드하고 넣어준다.
        byte[] filedata;
        filedata = File.ReadAllBytes(saveSshotFilePath);
        tex.LoadImage(filedata);  //넣어줬다.

        Rect rect = new Rect(320, 180, snapshot.width, snapshot.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 1); //스프라이트로 변경

        screenImage.sprite = sprite;//유아이에 입력
        }
        //

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S)) saveJsonTest1();
        //if (Input.GetKeyDown(KeyCode.O)) overRideJson();
        if (Input.GetKeyDown(KeyCode.R)) SaveAtPersistant();
        if (Input.GetKeyDown(KeyCode.L)) loadPersistant();



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

    void SaveAtPersistant()
    {
        //스크린샷 시작
        if (!Directory.Exists(SaveLoadTemplete.SavePath))
            Directory.CreateDirectory(SaveLoadTemplete.SavePath);

        StartCoroutine(TakeSnapshot(/*Screen.width, Screen.height,*/ saveSshotFilePath));
        //

        //제이슨 쓰기
        MyTemplete newMyTemplete = new MyTemplete();
        FileStream streamSave = new FileStream(saveFilePath, FileMode.Create);
        string toJson = JsonConvert.SerializeObject(newMyTemplete);
        Byte[] data2 = Encoding.UTF8.GetBytes(toJson);
        streamSave.Write(data2, 0, data2.Length);
        streamSave.Close();

        Debug.Log(toJson);
        Debug.Log(saveFilePath);
    }


    void loadPersistant()
    {


        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        Debug.Log(fromJson);
        Debug.Log(DeserialJson);
        Debug.Log("dd");
    }//로드


    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public  IEnumerator TakeSnapshot(/*int width, int height,*/ string fileName)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.LoadRawTextureData(texture.GetRawTextureData());
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes); //name은 path + name
 
    }
}

