using RuntimeHandle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ARLocation;
using System.IO;

public class ButtonAction_SelectCategory : MonoBehaviour
{
    [Header("����â")]
    public GameObject defaultPages;
    [Header("����â")]
    public GameObject[] PagesToChange = new GameObject[5];
    [Header("ī�װ����ù�ư")]
    public Button[] Bt_SelectType = new Button[5];
    [Header("ī�װ������� Ż��")]
    public Button bt_caterogryExit;



    [Header("ui root")]
    public GameObject contentsRoot;
    [Header("������ ui")]
    public GameObject AddItem_UIPrefab;

    public ScriptableObject_CategoryItems SO_3DObject;
    public List<ScOBJ_ItemInfo> List3DObject;

    //temp
    public GameObject saveRoot;
    private int idx = new int();
    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);

        foreach (var item in PagesToChange)
        {
            item.SetActive(false);
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

        ////ī�װ� ��ư
        //Bt_SelectType[0].onClick.AddListener(() =>ShowCategoryList(0));

    }
    //-->��ư�׼�
    void ShowCategoryList(int idx, ScriptableObject_CategoryItems scriptableObject)
    {
        switchPage(idx);
        Transform parent = PagesToChange[idx].transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        Placeable_ObjectList(parent, scriptableObject);
        print(parent);
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
        this.gameObject.SetActive(false);
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
        Manager.instance.date = DateTime.Now.ToString("yyyy.MM.dd_hh:mm:ss");
        //Manager.instance.date = DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss");


        Transform itemlist = GameObject.Find("---CurrentItemList---").transform;
        List<Transform> tempList = new List<Transform>();

        for (int i = 0; i < itemlist.childCount; i++)
        {
            tempList.Add(itemlist.GetChild(i).transform);
        }

        ////���� ���� üũ
        //if (!Directory.Exists(SaveLoadTemplete.SavePath))
        //{
        //    Directory.CreateDirectory(SaveLoadTemplete.SavePath);

        //    File.WriteAllText(SaveLoadTemplete.SavePath + "myTemplete.json",null);
        //}


        SaveLoadTemplete.SaveTemplete(tempList);
    }
}
