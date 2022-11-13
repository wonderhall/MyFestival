using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RuntimeHandle;
using ARLocation;
[Serializable]
public class mTemplete
{
    public string userID;
    public string date;
    public string latitude;
    public string longitude;
    public itemLocation[] mItem;
}
[Serializable]
public class itemLocation
{
    public string itemName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[Serializable]
public class itemsInfo
{
    public string itemName;
    public string iName;
    public Transform prefab;
    public Sprite preview;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[Serializable]
public class ScriptableObjectTest1
{
    public itemsInfo[] items;
}
public class CreateTemplete : MonoBehaviour
{
    GameObject mTemplete;
    public string name;
    public string latitude;
    public string longitude;
    [Space(10)]
    //public string[] location = new string[2];
    public ScriptableObjectTest1 items;

    [Header("������ ������")]
    public List<itemsInfo> itemList;

    [Header("ui root")]
    public GameObject contentsRoot;
    [Header("������ ui")]
    public GameObject ui;

    [Header("������ ������")]
    public List<itemsInfo> currentItems;

    public GameObject saveRoot;
    public List<Transform> itemsForSave;
    private ARLocationProvider locationProvider;
    
    public itemLocation[] _itemLocations;
    // Start is called before the first frame update
    private void Awake()
    {


        itemList.Add(items.items[0] as itemsInfo);

        foreach (var item in items.items)
        {
            Debug.Log(item.iName);
        }



        listBachig();
    }
    public void listBachig(/*Array list, GameObject pf, Transform target*/)
    {
        for (int i = 0; i < items.items.Length; i++)
        {

            string _name = "item_" + i;
            GameObject Newitem = Instantiate(ui);
            Newitem.transform.SetParent(contentsRoot.transform, false);
            Newitem.name = _name;
            //Newitem.AddComponent<ItemDrag>();
            Newitem.AddComponent<Button>();
            Newitem.GetComponent<Button>().onClick.AddListener(delegate { createObjectAtScene(items); });

            Newitem.transform.GetChild(0).GetComponent<Image>().sprite = items.items[i].preview;


        }
    }

    public void createObjectAtScene(ScriptableObjectTest1 _items)
    {
        //-->������ ������Ʈ id��������
        GameObject tempBtn = EventSystem.current.currentSelectedGameObject;
        string[] split_name = tempBtn.name.Split('_');
        int num = int.Parse(split_name[1]);
        //<--
        //���ø��� ������ �������Ʈ saveRoot�� �̸����� �����ϰ� �ν��Ͻ����� �� �θ�ȭ
        if (saveRoot == null) saveRoot = new GameObject("CurrentItemList");
        Transform newIns = Instantiate(_items.items[num].prefab, saveRoot.transform);
        //����� ����
        if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
            GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = newIns;
        else
            RuntimeTransformHandle.Create(newIns, 0).name = "handler";
        //����� �Ӽ�����
        RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
        handler.autoScale = true;
        handler.autoScaleFactor = 1.5f;


        //��������۸���Ʈ�߰�.
        currentItems.Add(_items.items[num]);

    }

    public void SaveTemp(string name)
    {
        for (int i = 0; i < saveRoot.transform.childCount; i++)
        {
            itemsForSave.Add(saveRoot.transform.GetChild(i));
            print(saveRoot.transform.GetChild(i));
        }

        //-->�����ǵ� ���� ���� ������ ����Ʈ�ۼ�
        //itemLocation[] _itemLocations = new itemLocation[itemsForSave.Count];
        _itemLocations = new itemLocation[itemsForSave.Count];
        for (int i = 0; i < itemsForSave.Count; i++)
        {
            _itemLocations[i] = new itemLocation();
            _itemLocations[i].itemName = itemsForSave[i].name;
            _itemLocations[i].position.x = itemsForSave[i].transform.position.x;
            _itemLocations[i].position.y = itemsForSave[i].transform.position.y;
            _itemLocations[i].position.z = itemsForSave[i].transform.position.z;

            _itemLocations[i].rotation.x = itemsForSave[i].transform.rotation.x;
            _itemLocations[i].rotation.y = itemsForSave[i].transform.rotation.y;
            _itemLocations[i].rotation.z = itemsForSave[i].transform.rotation.z;

            _itemLocations[i].scale.x = itemsForSave[i].transform.localScale.x;
            _itemLocations[i].scale.y = itemsForSave[i].transform.localScale.y;
            _itemLocations[i].scale.z = itemsForSave[i].transform.localScale.z;
        }
        //--<


        mTemplete _templete = new mTemplete();
        _templete.userID = name;
        _templete.date = DateTime.Now.ToString("yy-MM-dd-HH-mm");
        //_templete.latitude= locationProvider.CurrentLocation.latitude.ToString();
        //_templete.longitude = locationProvider.CurrentLocation.longitude.ToString();
        _templete.latitude = "15.1564";
        _templete.longitude = "4156.41564";
        _templete.mItem = _itemLocations;



        //���̽� ��ƿ��Ƽ�δ� �ٸ�������Ÿ���� ����Ʈ�� ���� �ʾƼ� wrapper���(JsonHelper)
        //string toJson = JsonHelper.ToJson(_templete, prettyprint: true);

        string FinalToJson = JsonUtility.ToJson(_templete, true);
        string JsonPath = Application.persistentDataPath + "/JsonData";

        //json����Ÿ������
        //if (!File.Exists(JsonPath))
        //{
        //    Directory.CreateDirectory(JsonPath);
        //    File.WriteAllText(JsonPath + "/itemData.json", toJson);
        //}
        //    File.WriteAllText(JsonPath + "/itemData.json", FinalToJson);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveTemp("user1");
            Debug.Log("json�����δ� " + Application.persistentDataPath + "/Saves/data.json");
        }
    }
}
