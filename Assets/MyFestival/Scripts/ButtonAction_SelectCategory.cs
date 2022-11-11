using RuntimeHandle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ARLocation;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public class ButtonAction_SelectCategory : MonoBehaviour
{
    [Header("����â")]
    public GameObject defaultPages;
    [Header("elementPage")]
    public GameObject ElementPages;
    [Header("����â")]
    public GameObject[] PagesToChange = new GameObject[6];
    [Header("ī�װ����ù�ư")]
    public Button[] Bt_SelectType = new Button[5];
    [Header("ī�װ������� Ż��")]
    public Button bt_caterogryExit;
    [Header("���̵� �г�")]
    public GameObject[] sidePanel= new GameObject[2];


    [Header("ui root")]
    public GameObject contentsRoot;
    [Header("������ ui")]
    public GameObject AddItem_UIPrefab;

    public ScriptableObject_CategoryItems SO_3DObject;
    public List<ScOBJ_ItemInfo> List3DObject;

    //temp
    private GameObject saveRoot;
    private int idx = new int();
    private void Awake()
    {
        //������ ������ ������ ���� ���� �������Ʈ ����
        if (!Directory.Exists(SaveLoadTemplete.SavePath)) SaveLoadTemplete.newEmptyData();
      
  //������ �ʱ� �¿���
        foreach (var item in PagesToChange)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < PagesToChange[5].transform.childCount; i++)
        {
            PagesToChange[5].transform.GetChild(i).gameObject.SetActive(true);
        }
     }
    private void Start()
    {
        Bt_SelectType[0].onClick.AddListener(() => ShowCategoryList(0, SO_3DObject));
        Bt_SelectType[1].onClick.AddListener(() => ShowCategoryList(1, SO_3DObject));
        Bt_SelectType[2].onClick.AddListener(() => ShowCategoryList(2, SO_3DObject));
        Bt_SelectType[3].onClick.AddListener(() => ShowCategoryList(3, SO_3DObject));
        Bt_SelectType[4].onClick.AddListener(() => ShowCategoryList(4, SO_3DObject));

        bt_caterogryExit.onClick.AddListener(categoryExit);


    }
    //-->��ư�׼�
    void ShowCategoryList(int idx, ScriptableObject_CategoryItems scriptableObject)
    {
        switchPage(idx);
        Placeable_ObjectList(contentsRoot.transform, scriptableObject);
    }
    public void switchPage(int idx)
    {
        bool ison = defaultPages.activeSelf;
        PagesToChange[idx].SetActive(ison);
        defaultPages.SetActive(!ison);
        ison = !ison;
    }
    public void categoryExit()
    {
        defaultPages.SetActive(true);
        foreach (var page in PagesToChange)
        {
            page.SetActive(false);
        }
    }

    //<--
    public void Placeable_ObjectList(Transform parent, ScriptableObject_CategoryItems scriptableObject)
    {
        if (parent.childCount == 0)
        {

            for (int i = 0; i < scriptableObject.placeableObjects.Length; i++)
            {

                string str = scriptableObject.placeableObjects[i].itemName;

                GameObject Newitem = Instantiate(AddItem_UIPrefab);
                Newitem.transform.SetParent(parent, false);
                Newitem.name = str + "_" + i;
                //Newitem.AddComponent<ItemDrag>();
                Newitem.AddComponent<Button>();
                //��ũ���ͺ� �̹��� �����ͼ�  �ֱ�
                Sprite img = scriptableObject.placeableObjects[i].preview;
                Newitem.transform.GetChild(0).GetComponent<Image>().sprite = img;
                //��ũ���ͺ� �̸� �����ͼ�  �ֱ�
                Newitem.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                    str;
                //��ư��� ���̱�
                Newitem.GetComponent<Button>().onClick.AddListener(delegate { createObjectAtScene(scriptableObject); });


            }
        }//if����
    }
    /// <summary>
    /// ����� ������ ������Ʈ ����
    /// </summary>
    public void createObjectAtScene(ScriptableObject_CategoryItems scriptableObject)
    {
        categoryExit();
        //-->������ ������Ʈ id��������
        GameObject tempBtn = EventSystem.current.currentSelectedGameObject;
        string[] split_name = tempBtn.name.Split('_');
        //string str = split_name[0];//������ ������Ʈ �̸�
        int num = int.Parse(split_name[1]);

        Manager.instance.scriptableObject = scriptableObject;
        Manager.instance.SeletedObjectIndex = num;
        //<--
        //    //���ø��� ������ �������Ʈ saveRoot�� �̸����� �����ϰ� �ν��Ͻ����� �� �θ�ȭ
        if (!GameObject.Find("---CurrentItemList---"))
        {
            GameObject rootobj = new GameObject("---CurrentItemList---");
            Vector3 positionOffset = new Vector3(0, 0, 2);
            rootobj.transform.position = positionOffset;
            saveRoot = rootobj;
        }
        else
            saveRoot = GameObject.Find("---CurrentItemList---");


        Transform newIns = Instantiate(scriptableObject.placeableObjects[num].prefab, saveRoot.transform);
        newIns.name = tempBtn.name;

        //����� ����
        if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
            GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = newIns;
        else
            RuntimeTransformHandle.Create(newIns, 0).name = "handler";
        //����� �Ӽ�����
        RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
        handler.autoScale = true;
        handler.autoScaleFactor = 1.5f;
    }

    public void saveTemple()
    {

        ARLocationProvider aRLocationProvider = GameObject.FindObjectOfType<ARLocationProvider>();
        Manager.instance.latitude = aRLocationProvider.CurrentLocation.latitude;
        Manager.instance.longitude = aRLocationProvider.CurrentLocation.longitude;
        Manager.instance.date = DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss");


            List<Transform> tempList = new List<Transform>();
        if (GameObject.Find("---CurrentItemList---"))
        {
            Transform itemlist = GameObject.Find("---CurrentItemList---").transform;
            for (int i = 0; i < itemlist.childCount; i++)
            {
                tempList.Add(itemlist.GetChild(i).transform);
            }
        }

        // SaveLoadTemplete.SaveTemplete(tempList);

        //�������������
        string savepriviewPath = SaveLoadTemplete.SavePath + Manager.instance.date + ".png";
        SaveLoadTemplete.SaveTemplete(tempList);
        StartCoroutine(TakePreview(savepriviewPath));

    }
  
    IEnumerator TakePreview(string fileName)
    {
        foreach (GameObject page in sidePanel) page.SetActive(false); //������� ���� �����°� ġ���

        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.LoadRawTextureData(texture.GetRawTextureData());
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes); //filename�� path + name
        yield return new WaitForChangedResult();

        foreach (GameObject page in sidePanel) page.SetActive(true);//��� ���� ���̱�

    }



}
