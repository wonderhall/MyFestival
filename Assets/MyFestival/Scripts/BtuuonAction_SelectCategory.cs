using RuntimeHandle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtuuonAction_SelectCategory : MonoBehaviour
{
    [Header("상위창")]
    public GameObject defaultPages;
    [Header("하위창")]
    public GameObject[] PagesToChange = new GameObject[5];
    [Header("카테고리선택버튼")]
    public Button[] Bt_SelectType = new Button[5];
    [Header("카테고리페이지 탈출")]
    public Button bt_caterogryExit;



    [Header("ui root")]
    public GameObject contentsRoot;
    [Header("생성할 ui")]
    public GameObject AddItem_UIPrefab;

    public ScriptableObject_CategoryItems SO_3DObject;
    public List<ScOBJ_ItemInfo> List3DObject;

    //temp
    public GameObject saveRoot;
    private int idx = new int();
    private void Awake()
    {
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

        ////카테고리 버튼
        //Bt_SelectType[0].onClick.AddListener(() =>ShowCategoryList(0));

    }
    //-->버튼액션
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

        for (int i = 0; i < scriptableObject.placeableObjects.Length; i++)
        {

            string str = scriptableObject.placeableObjects[i].itemName;

            GameObject Newitem = Instantiate(AddItem_UIPrefab);
            Newitem.transform.SetParent(parent, false);
            Newitem.name = str + "_" + i;
            //Newitem.AddComponent<ItemDrag>();
            Newitem.AddComponent<Button>();
            //스크럽터블 이미지 가져와서  넣기
            Sprite img = scriptableObject.placeableObjects[i].preview;
            Newitem.transform.GetChild(0).GetComponent<Image>().sprite = img;
            //스크럽터블 이름 가져와서  넣기
            Newitem.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                str;
            //버튼기능 붙이기
            Newitem.GetComponent<Button>().onClick.AddListener(delegate { createObjectAtScene(scriptableObject); });


        }
    }
    /// <summary>
    /// 기즈모 부착된 오브젝트 생성
    /// </summary>
    public void createObjectAtScene(ScriptableObject_CategoryItems scriptableObject)
    {
        categoryExit();
        //-->선택한 오브젝트 id가져오기
        GameObject tempBtn = EventSystem.current.currentSelectedGameObject;
        string[] split_name = tempBtn.name.Split('_');
        //string str = split_name[0];//선택한 오브젝트 이름
        int num = int.Parse(split_name[1]);

        Manager.instance.scriptableObject = scriptableObject;
        Manager.instance.SeletedObjectIndex = num;
        //<--
        //    //템플릿을 저장할 빈오브젝트 saveRoot란 이름으로 생성하고 인스턴스생성 후 부모화
        GameObject saveRoot = new GameObject("CurrentItemList");
        Transform newIns = Instantiate(scriptableObject.placeableObjects[num].prefab, saveRoot.transform);

        //기즈모 부착
        if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
            GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = newIns;
        else
            RuntimeTransformHandle.Create(newIns, 0).name = "handler";
        //기즈모 속성변경
        RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
        handler.autoScale = true;
        handler.autoScaleFactor = 1.5f;
    }
   
}
