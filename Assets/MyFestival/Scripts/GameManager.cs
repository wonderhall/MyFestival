using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using ARLocation;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�޴� �¿��� ��ư")]
    public Button bt_showMenu;
    public Button bt_hideMenu;

    public Button bt_CreateAREvent, bt_EventList, bt_SavedList, bt_PurchaseList, bt_Settings;

    [Header("����������Ʈ")]
    public List<GameObject> paseList;
    [Header("��������������Ʈ")]
    public List<GameObject> subPaseList;
    [Header("����AR ��������Ʈ")]
    public List<GameObject> ARFestvalSubList;
    [Header("�Է¹��")]
    public GameObject[] banner = new GameObject[2];
    [Header("���ϸ��̼� ĵ�ٽ�")]
    public Animator[] rectAnim;
    [Header("ar�̺�Ʈ����� â")]
    public GameObject AREventCanvas;
    [Header("�������ø�")]
    public string SeletTemplete;
    [Header("�Է�������|GPS��ǥ")]
    public string EnterTitleName, CurrentLatitude, CurrentLongitude;
    [Header("�����ý�Ʈ�Է��ʵ�")]
    public InputField inputField;


    private Animator _animator;
    public bool opendedMenu = false;


    //����gps�޾ƿ���
    private ARLocationProvider locationProvider;
    public Text testText;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < paseList.Count; i++)
        {
            if (i == 0)
                paseList[i].gameObject.SetActive(true);
            else
                paseList[i].gameObject.SetActive(false);
        }
        foreach (var item in ARFestvalSubList)
        {
            item.SetActive(false);
        }

        AREventCanvas.SetActive(false);

    }
    void Start()
    {

        locationProvider = this.GetComponent<ARLocationProvider>();
        _animator = GameObject.Find("SideBoard").GetComponent<Animator>();
        foreach (var item in subPaseList)
        {
            item.gameObject.SetActive(false);
        }

        //bt_showMenu.onClick.AddListener(() => mAction.Instance.MenuOpen(_animator,rectAnim,bt_showMenu,opendedMenu));
        //bt_hideMenu.onClick.AddListener(() => mAction.Instance.MenuClose(_animator, rectAnim, bt_showMenu,opendedMenu));
        bt_showMenu.onClick.AddListener(() => MenuOpen());
        bt_hideMenu.onClick.AddListener(() => MenuClose());
        bt_CreateAREvent.onClick.AddListener(() => ShowCanvas(1));
        bt_EventList.onClick.AddListener(() => ShowCanvas(2));
        bt_SavedList.onClick.AddListener(() => ShowCanvas(3));
        bt_PurchaseList.onClick.AddListener(() => ShowCanvas(4));
        bt_Settings.onClick.AddListener(() => ShowCanvas(5));


    }
    // Update is called once per frame
    void Update()
    {
        float interval = 0.1f;
        float _time = 0;
        _time += Time.deltaTime;
        if (_time > interval)
        {
            //print("do");
            //mAction.Instance.ChangeUiRotation();
            _time = 0;
        }
    }
    void MenuOpen()
    {
        opendedMenu = true;
        bt_showMenu.gameObject.SetActive(false);
        _animator.SetBool("IsOpen", true);
        foreach (var item in rectAnim) item.SetBool("IsOpen", true);
        Debug.Log("�޴��� ����");
    }
    void MenuClose()
    {
        opendedMenu = false;
        bt_showMenu.gameObject.SetActive(true);
        _animator.SetBool("IsOpen", false);
        foreach (var item in rectAnim) item.SetBool("IsOpen", false);
        Debug.Log("�޴��� �ݴ´�");
    }
    void ShowCanvas(int index)
    {
        //mAction.Instance.MenuClose(_animator, rectAnim, bt_showMenu,opendedMenu);
        MenuClose();
        //rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 750,750);
        SwitchPage(paseList.ToArray(), index);
    }
    public void GoMainUI()
    {
        for (int i = 0; i < paseList.Count; i++)
        {
            if (i == 0)
                paseList[i].gameObject.SetActive(true);
            else
                paseList[i].gameObject.SetActive(false);
        }
    }

    public void showSubPage(int id)
    {
        if (!opendedMenu)
            SwitchPage(subPaseList.ToArray(), id - 1);
        else
            return;
    }
    public void Go_ArFestivaRowPage(int id)
    {
        SwitchPage(ARFestvalSubList.ToArray(), id);
    }

    public void SwitchPage(GameObject[] mList, int index)
    {
        for (int i = 0; i < mList.Length; i++)
        {
            if (i == index)
                mList[i].SetActive(true);
            else
                mList[i].SetActive(false);
        }
    }

    public void CreateAREvent()
    {
        //������ ��ư�� �ý�Ʈ���ϰ�������//
        SeletTemplete = GameObject.Find(EventSystem.current.currentSelectedGameObject.name).GetComponent<Button>().GetComponentInChildren<Text>().text;
        AREventCanvas.SetActive(true);
        ARFestvalSubList[1].SetActive(false);

        int id = 0; //���ʸ���Ʈ �̸��Է¹�� �ε���
        SwitchPage(banner, id);
    }
    public void ClearInputFiled()
    {
        EnterTitleName = SeletTemplete;
        int id = 1; //���ʸ���Ʈ ��ġ�����ε���
        SwitchPage(banner, id);
    }
    public void EnterTitle()
    {
        EnterTitleName = inputField.text;

        int id = 1; //���ʸ���Ʈ ��ġ�����ε���
        SwitchPage(banner, id);
    }
    public void CancleSelcetLocation()
    {
        int id = 2; //���ʸ���Ʈ �̹������ε�â �ε���
        SwitchPage(banner, id);



        //����Ÿ����//
        string _User; //get user name
        string _title = EnterTitleName;
        string _latitude;  //get ������ ���÷����� �������� ��������
        string _longitude; //get ������ ���÷����� �浵���� ��������
    }
    public void EnterLocation()
    {
        int id = 2; //���ʸ���Ʈ �̹������ε�â �ε���
        SwitchPage(banner, id);


        CurrentLatitude = locationProvider.CurrentLocation.latitude.ToString();
        CurrentLongitude = locationProvider.CurrentLocation.latitude.ToString();


        //test
        testText.text = "latitude : " + CurrentLongitude + "longititude : " + CurrentLongitude;

        //����Ÿ����//
        string _User; //get user name
        string _title = EnterTitleName;
        string _latitude = locationProvider.CurrentLocation.latitude.ToString();
        string _longitude = locationProvider.CurrentLocation.latitude.ToString();
    }
    public void SceneLoad(string ScName)
    {
        SceneManager.LoadSceneAsync(ScName,LoadSceneMode.Single);
    }
}
