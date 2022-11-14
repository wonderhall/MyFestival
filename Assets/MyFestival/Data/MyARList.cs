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
    //�н�
    string saveFileName;
    private string saveFilePath;
    //

    [Header("ui root")]
    public Transform contentsRoot;
    [Header("������ ui")]
    public GameObject AddItem_UIPrefab;
    [Header("��ü ������ ����Ʈ")]
    public GameObject[] SelectedTempletePage;

    private Dictionary<string, CurrentTemplete> DicMyTemp;
    private void Awake()
    {
        if (GameObject.Find("Window_MyList"))
        {
            GetJsonWithMyList();
        }
    }

    public void GetJsonWithMyList()//���̸���Ʈâ�� ���鼭 ���� ����� ���� ������Ʈ ����
    {        //�н�
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
        //�ҷ����� ����
        Debug.Log("myArList Load is sucess");
        //��ųʸ�����
        //Dictionary<string, CurrentTemplete> DicMyTemp = new Dictionary<string, CurrentTemplete>(); 
        DicMyTemp = new Dictionary<string, CurrentTemplete>();
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


                //��ư��� ���̱�
                Newitem.GetComponent<Button>().onClick.AddListener(delegate { ShowSelectedTempleInfo(str, DicMyTemp); });
            }
        }//if����
        else Debug.Log("�̸̹�������ִ�");

    }
    void ShowSelectedTempleInfo(string name, Dictionary<string, CurrentTemplete> dic)
    {
        SelectedTempletePage[0].SetActive(false);//�ش������� ����
        SelectedTempletePage[1].SetActive(true);//�ش������� ����

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        CurrentTemplete thisTempleteInfo = new CurrentTemplete();
        dic.TryGetValue(name, out thisTempleteInfo);//��ųʸ����� �̸����� �߷����ش��ϴ� Ŭ���� �̾ƿ´�.

        string date = thisTempleteInfo.date;
        string previewName = thisTempleteInfo.PreviewName;
        double latitude = thisTempleteInfo.latitude;
        double longitude = thisTempleteInfo.longitude;


        ////�����ֱ�
        GameObject icon = GameObject.Find("IconText");
        icon.GetComponent<Text>().text = name; //�̸�
        //
        GameObject TemDate = GameObject.Find("Text_Date");
        TemDate.GetComponent<Text>().text = date;  //����
                                                   //                                          ////
        GameObject gps = GameObject.Find("Text_GPS");
        gps.GetComponent<Text>().text = latitude.ToString() + "," + longitude.ToString();  //gps


        GameObject preview = GameObject.Find("Preview");
        preview.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //������
        preview.AddComponent<Button>().onClick.AddListener(ShowCurrentTemplet(thisTempleteInfo.items.ToArray));

        GameObject previewSmall = GameObject.Find("PreviewSmall");
        previewSmall.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //������

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
        if (DicMyTemp.ContainsKey(name)) DicMyTemp.Remove(name); //dic���� �̸����� �����

        MyTemplete newTemp = new MyTemplete();  //�ٽ� ���� ���� Ŭ������ ����� �־� �ִ�.
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
        Texture2D tex = null;    //�� �ý��� ������ ����Ʈ�� �ε��ϰ� �־��ش�.
        byte[] filedata;
        filedata = File.ReadAllBytes(path);
        tex = new Texture2D(0, 0);
        tex.LoadImage(filedata);  //�־����.

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 1); //��������Ʈ�� ����

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