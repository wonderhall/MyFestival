using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction_CreateTemp : MonoBehaviour
{
    [Header("메뉴 온오프 버튼")]
    public Button bt_showMenu;
    public Button bt_hideMenu;
    private Animator _animator;
    public bool menuClose = false;

    [Header("왼쪽사이드바 버튼들")]
    public Button bt_BackToMain;
    public Button bt_ResetFirst;
    public Button bt_Save;
    public Button bt_SaveAsCopy;
    public Button bt_MyArList;

    [Header("오른쪽사이드바 버튼들")]
    public Button bt_ScreenShot;
    public Button bt_ResetFocus;
    public Button bt_AddObject;

    [Header("하위 확인 버튼")]
    public Button bt_YesBackToMain;
    public Button bt_YesResetFirst;
    public Button bt_YesMyArList;

    [Header("창 전환 리스트")]
    public GameObject[] changeWindowList;
    [Header("알람창")]
    public GameObject[] AlarmWindow;


    // Start is called before the first frame update
    void Start()
    {
        //메뉴온오프액션
        bt_showMenu.onClick.AddListener(() => MenuOpen());
        bt_hideMenu.onClick.AddListener(() => MenuClose());
        //메뉴외쪽액션
        bt_BackToMain.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[1])));
        bt_ResetFirst.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[2])));
        bt_Save.onClick.AddListener(()=>StartCoroutine(SshotAlarm(AlarmWindow[1])));
        bt_SaveAsCopy.onClick.AddListener(() => StartCoroutine(SshotAlarm(AlarmWindow[2])));
        bt_MyArList.onClick.AddListener(()=>StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[3])));
        //bt_SaveAsCopy.onClick.AddListener(() =>);
        //bt_MyArList.onClick.AddListener(() =>);

        //메뉴 오른쪽 액션
        bt_ScreenShot.onClick.AddListener(() => StartCoroutine(TakeAndSaveScreenshot()));
        bt_ResetFocus.onClick.AddListener(() => Manager.instance.ResetScene());
        bt_AddObject.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[4])));

        //하위 확인번트
        bt_YesBackToMain.onClick.AddListener(() => Manager.instance.SceneLoad("Main"));
        bt_YesResetFirst.onClick.AddListener(() => Manager.instance.ResetScene());
        bt_YesMyArList.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[3], changeWindowList[5])));

    }

    // Update is called once per frame
    void MenuOpen()
    {
        _animator = bt_showMenu.transform.parent.GetComponent<Animator>();
        bt_showMenu.gameObject.SetActive(false);
        bt_hideMenu.gameObject.SetActive(true);
        _animator.SetBool("IsOpen", true);
        menuClose = false;
        changeWindowList[0].SetActive(true);
        Debug.Log("메뉴를 연다");
    }
    void MenuClose()
    {
        _animator = bt_showMenu.transform.parent.GetComponent<Animator>();
        bt_showMenu.gameObject.SetActive(true);
        bt_hideMenu.gameObject.SetActive(false);
        _animator.SetBool("IsOpen", false);
        menuClose = true;
        Debug.Log("메뉴를 닫는다");
    }
    IEnumerator TakeAndSaveScreenshot()
    {
        Debug.Log("찰칵");
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //알림창 오픈
        StartCoroutine(SshotAlarm(AlarmWindow[0]));
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to gallery
        NativeGallery.SaveImageToGallery(imageBytes, "AlbumName", "ScreenshotName.png", null);
    }

    IEnumerator SshotAlarm(GameObject alarmWindow)
    {
        alarmWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        alarmWindow.SetActive(false);

    }

    IEnumerator OpenAfterCloseMenu(GameObject preWindow, GameObject nextWindow)
    {
        MenuClose();
        preWindow.SetActive(false);
        yield return new WaitUntil(() => menuClose);
        nextWindow.SetActive(true);

    }
}
