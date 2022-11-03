using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [Header("메뉴 온오프 버튼")]
    public Button bt_showMenu;
    public Button bt_hideMenu;
    private Animator _animator;
    [Header("스크린샷,리셋 버튼")]
    public Button bt_ScreenShot;
    public Button bt_Reset;


    [Header("사이드보드 버튼")]
    public Button bt_createNewAR;
    public Button bt_ARTempleteList;
    public Button bt_myARList;
    public Button bt_noticeBoard;
    public Button bt_logOut;
    public Button bt_closeTheApp;

    [Header("하위 버튼")]
    public Button bt_LogOutSub;
    public Button bt_closetheAppSub;
    public Button bt_CreateNewTemplete;


    [Header("메뉴리스트")]
    public GameObject menuList;
    
    [Header("알람창")]
    public GameObject AlarmWindow;

    [Header("창 전환 리스트")]
    public GameObject[] changeWindowList;

    public bool menuClose = false;
    private void Start()
    {
        bt_showMenu.onClick.AddListener(() => MenuOpen());
        bt_hideMenu.onClick.AddListener(() => MenuClose());
        bt_ScreenShot.onClick.AddListener(()=> StartCoroutine(TakeAndSaveScreenshot()));
        bt_Reset.onClick.AddListener(()=> Manager.instance.ResetScene());
        bt_createNewAR.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[1])));
        bt_ARTempleteList.onClick.AddListener(()=> StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[2])));
        bt_myARList.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[3])));
        bt_noticeBoard.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[4])));
        bt_logOut.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[5])));
        bt_closeTheApp.onClick.AddListener(() => StartCoroutine(OpenAfterCloseMenu(changeWindowList[0], changeWindowList[6])));

        bt_LogOutSub.onClick.AddListener(() => Manager.instance.SceneLoad("Login"));
        bt_closetheAppSub.onClick.AddListener(()=>Application.Quit());
        bt_CreateNewTemplete.onClick.AddListener(()=>Manager.instance.SceneLoad("CreateNewTemp"));


    }

    //버튼전용액션--->
   public void MenuOpen()
    {
        _animator = bt_showMenu.transform.parent.GetComponent<Animator>();
        bt_showMenu.gameObject.SetActive(false);
        bt_hideMenu.gameObject.SetActive(true);
        _animator.SetBool("IsOpen", true);
        menuClose = false;

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

    //<--




    IEnumerator TakeAndSaveScreenshot()
    {
        Debug.Log("찰칵");
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //알림창 오픈
        StartCoroutine(SshotAlarm());
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to gallery
        NativeGallery.SaveImageToGallery(imageBytes, "AlbumName", "ScreenshotName.png", null);
    }

    IEnumerator SshotAlarm()
    {
        AlarmWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        AlarmWindow.SetActive(false);
            
    }

    IEnumerator OpenAfterCloseMenu(GameObject preWindow, GameObject nextWindow)
    {
        MenuClose();
        yield return new WaitUntil(() => menuClose);
        nextWindow.SetActive(true);

    }
}
