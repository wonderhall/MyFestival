using System.Collections.Generic;
using UnityEngine;
using Gpm.WebView;
using EasyUI.Toast;
using System.Threading;
using System;
using UnityEngine.UI;

public class WebViewHandler : MonoBehaviour
{
    public Button webViewButton;

    private Queue<Action> m_queueAction = new Queue<Action>();
    public void ShowUrlFullScreen()
    {
        GpmWebView.ShowUrl(
            "https://daxib.8hlab.com/map.html",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.LANDSCAPE,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                navigationBarColor = "#00002D",
                title = "���� ��ġ�� �����ϼ���..",
                isBackButtonVisible = false,
                isForwardButtonVisible = false,
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE
#endif
        },
            OnCallback,
            new List<string>()
            {
                "daxib",
                "https://location/"
            });
    }

    private void OnCallback(
    GpmWebViewCallback.CallbackType callbackType,
    string data,
    GpmWebViewError error)
    {
        Debug.Log("OnCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                if (error == null)
                {
                    Toast.Show("scheme : " + data);
                    if (data.StartsWith("https://location/") == true)
                    {
                        GpmWebView.Close();
                        Debug.Log(string.Format("scheme:{0}", data));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
                }
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
        }
    }

    public void GoCall()
    {
        //���� ������ �ƴ� ������ ���� ������
        Thread thread = new Thread(() =>
        {
            //ť�� �׼��� �ִ´�.
            m_queueAction.Enqueue(() =>
            {
                Toast.Show("WebView Loaded");
                ShowUrlFullScreen();
            });
        });
        thread.Start();
    }

    public void OnClickWebView()
    {
        Toast.Show("WebView Loaded");
        ShowUrlFullScreen();
    }

    // Start is called before the first frame update
    void Start()
    {
        webViewButton.onClick.AddListener(OnClickWebView);

        GoCall();
    }

    // Update is called once per frame
    void Update()
    {
        while (m_queueAction.Count > 0)
        {
            m_queueAction.Dequeue().Invoke();
        }
    }
}
