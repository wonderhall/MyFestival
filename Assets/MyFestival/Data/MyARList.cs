using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

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
    [Header("교체 페이지 리스트")]
    public GameObject[] SelectedTempletePage;

    private Dictionary<string, CurrentTemplete> DicMyTemp;
    private void Awake()
    {
        if (GameObject.Find("Window_MyList"))
        {
            GetJsonWithMyList();
        }
    }

    public void GetJsonWithMyList()//마이리스트창을 열면서 전에 만들어 놓은 오브젝트 삭제
    {        //패스
        string saveFileName = "myTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
        //

        if (GameObject.Find("---CurrentItemList---"))
        {
            GameObject root = GameObject.Find("---CurrentItemList---");
            for (int i = 0; i < root.transform.childCount; i++)
            {
                Destroy(root.transform.GetChild(i).gameObject);
            }
            GameObject handle = GameObject.Find("handler");
            Destroy(handle);
        }
        if (!Directory.Exists(SaveLoadTemplete.SavePath)) Directory.CreateDirectory(saveFilePath);

        FileStream streamOpen = new FileStream(saveFilePath, FileMode.Open);
        Byte[] data = new byte[streamOpen.Length];
        streamOpen.Read(data, 0, data.Length);
        streamOpen.Close();
        string fromJson = Encoding.UTF8.GetString(data);
        MyTemplete DeserialJson = JsonConvert.DeserializeObject<MyTemplete>(fromJson);
        //불러오기 성공
        Debug.Log("myArList Load is sucess");
        //딕셔너리저장
        //Dictionary<string, CurrentTemplete> DicMyTemp = new Dictionary<string, CurrentTemplete>(); 
        DicMyTemp = new Dictionary<string, CurrentTemplete>();
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


                //버튼기능 붙이기
                Newitem.GetComponent<Button>().onClick.AddListener(delegate { ShowSelectedTempleInfo(str, DicMyTemp); });
            }
        }//if종료
        else Debug.Log("이미만들어져있다");

    }
    void ShowSelectedTempleInfo(string name, Dictionary<string, CurrentTemplete> dic)
    {
        SelectedTempletePage[0].SetActive(false);//해당페이지 오픈
        SelectedTempletePage[1].SetActive(true);//해당페이지 오픈

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        CurrentTemplete thisTempleteInfo = new CurrentTemplete();
        dic.TryGetValue(name, out thisTempleteInfo);//딕셔너리에서 이름으로 발류에해당하는 클리스 뽑아온다.

        string date = thisTempleteInfo.date;
        string previewName = thisTempleteInfo.PreviewName;
        double latitude = thisTempleteInfo.latitude;
        double longitude = thisTempleteInfo.longitude;


        ////정보넣기
        GameObject icon = GameObject.Find("IconText");
        icon.GetComponent<Text>().text = name; //이름
        //
        GameObject TemDate = GameObject.Find("Text_Date");
        TemDate.GetComponent<Text>().text = date;  //날자
                                                   //                                          ////
        GameObject gps = GameObject.Find("Text_GPS");
        gps.GetComponent<Text>().text = latitude.ToString() + "," + longitude.ToString();  //gps


        GameObject preview = GameObject.Find("Preview");
        preview.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //프리뷰
        preview.AddComponent<Button>().onClick.AddListener(ShowCurrentTemplet(thisTempleteInfo.items.ToArray));

        GameObject previewSmall = GameObject.Find("PreviewSmall");
        previewSmall.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //프리뷰

        Button renamer = GameObject.Find("Bt_Renamer").GetComponent<Button>();
        renamer.onClick.AddListener(ShowRenameEditor);



        //func
        Button Bt_deleteTemp = GameObject.Find("Bt_Delete").GetComponent<Button>();
        Bt_deleteTemp.onClick.AddListener(() => deleteTempFromList(name, SelectedTempletePage[1], selectedObject));



    }

    private UnityAction ShowCurrentTemplet(Func<CurItemList[]> toArray)
    {
        throw new NotImplementedException();
    }

    void deleteTempFromList(string name, GameObject page, GameObject selectedObject)
    {
        if (DicMyTemp.ContainsKey(name)) DicMyTemp.Remove(name); //dic에서 이름으로 지우기

        MyTemplete newTemp = new MyTemplete();  //다시 쓰기 위해 클래스를 만들고 넣어 주다.
        foreach (var item in DicMyTemp)
        {
            newTemp.myTemplete.Add(item.Value);
        }
        Destroy(selectedObject);
        SaveLoadTemplete.SaveTemplete(newTemp, page);
    }

    Sprite PreviewTextrueLoad(string name)
    {

        string path = SaveLoadTemplete.SavePath + name + ".png";
        Texture2D tex = null;    //빈 택스쳐 생성후 바이트로 로드하고 넣어준다.
        byte[] filedata;
        filedata = File.ReadAllBytes(path);
        tex = new Texture2D(0, 0);
        tex.LoadImage(filedata);  //넣어줬다.

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 1); //스프라이트로 변경

        return sprite;

    }

    void ShowRenameEditor()
    {

    }
    void ShowCurrentTemplet(CurItemList[] items)
    {
        foreach (var item in items)
        {
          Debug.Log(item.ItemName);
        }
    }
}