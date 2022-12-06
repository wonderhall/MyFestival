using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using System;
using Gpm.WebView;

[Serializable]

public class tourGps
{
    public int idx;
    public string Name;
    public double latitude;
    public double longitude;
}
[Serializable]
public class tourGpsList
{
    public List<tourGps> tourlist = new List<tourGps>();
}

public class Publish : MonoBehaviour
{
    //�ۺ��������� �����ֱ�
    public Text cur_name;
    public InputField if_name;
    public Text cur_GPS;
    public Dropdown dropdown;
    public double ChoiceLatitude;
    public double ChoiceLongitude;
    Dictionary<int, tourGps> dic = new Dictionary<int, tourGps>();
    public Button Bt_save;

    //
    [Tooltip("������ ������ �޾ƿ�")]
    public double gotLatitude;
    public double gotlongitude;
    public CurrentTemplete cTemp;
    public Dictionary<string, CurrentTemplete> pubDic;


    private void OnEnable()
    {
        #region ������ gps�����ͼ� �ɼ� �־��ֱ�
        TextAsset textData = Resources.Load<TextAsset>("tourGpsList") as TextAsset;
        tourGpsList mTourGpsList = JsonUtility.FromJson<tourGpsList>(textData.ToString());

        foreach (var item in mTourGpsList.tourlist)
        {
            dic.Add(item.idx, item);
        }

        dropdown.options.Clear();
        for (int i = 0; i < dic.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = dic[i].Name;
            dropdown.options.Add(option);

            if (i == dic.Count - 1) dropdown.options.Add(option);
        }
        #endregion
        //
        if_name.onEndEdit.AddListener(delegate { NameValuedChanged(if_name); });//�̸��ٲٱ�
        dropdown.onValueChanged.AddListener(delegate { GpsValuedChanged(dropdown, dic); });//gps�ٲٱ�
        Bt_save.onClick.AddListener(delegate { TryPublish(); });//�ۺ���

    }

    void NameValuedChanged(InputField thisInputField) //�̸��ٲٱ�
    {
        if (thisInputField.text.Length > 0)
            cur_name.text = thisInputField.text;
    }
    void GpsValuedChanged(Dropdown change, Dictionary<int, tourGps> dic)//gps �ٲٱ�
    {
        tourGps tg = new tourGps();
        dic.TryGetValue(change.value, out tg);
        if (change.value == 0)
        {
            ChoiceLatitude = gotLatitude;
            ChoiceLongitude = gotlongitude;
        }
        else if (change.value == 1)
        {
            ChoiceLatitude = gotLatitude;
            ChoiceLongitude = gotlongitude;
            ShowUrlFullScreen();
        }
        else
        {
            ChoiceLatitude = tg.latitude;
            ChoiceLongitude = tg.longitude;
        }

        showLocationInfo();
    }

    private void showLocationInfo()
    {
        string changeGps = ChoiceLatitude + "," + ChoiceLongitude;
        cur_GPS.text = changeGps;
    }

    public void ShowUrlFullScreen()
    {
        GpmWebView.ShowUrl(
            "https://daxib.8hlab.com/map.html?lat=" + gotLatitude + "&lng=" + gotlongitude,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
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
                    if (data.StartsWith("https://location/") == true)
                    {
                        var query = new Uri(data).Query;
                        Debug.Log("query: " + query);

                        NameValueCollection nameValueCollection = new NameValueCollection();
                        StringUtil.ParseQueryString(query, System.Text.Encoding.Unicode, nameValueCollection);

                        var lat = nameValueCollection["lat"];
                        var lng = nameValueCollection["lng"];

                        ChoiceLatitude = Double.Parse(lat);
                        ChoiceLongitude = Double.Parse(lng);

                        showLocationInfo();
                        GpmWebView.Close();
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

    public void TryPublish()//�ۺ���
    {
        //������ ������ ����
        if (if_name.text.Length > 0)
            cTemp.tempeteName = if_name.text;
        cTemp.latitude = ChoiceLatitude;
        cTemp.longitude = ChoiceLongitude;
        //
        string saveFileName = "PublishTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";

        Dictionary<string, CurrentTemplete> pubDic = SaveLoadTemplete.DicByJson(saveFilePath);//���̽��� �о ��ųʸ�ȭ

        CurrentTemplete ct;
        ct = GameObject.FindObjectOfType<MyARList>().CompairName(cTemp, pubDic);//�̸��� ��ġ�� �ʴ��� üũ
        if (ct.tempeteName != null)
        {
            SaveLoadTemplete.SaveTempleteToShop(ct, this.transform.parent.gameObject, saveFilePath);//����
            Destroy(this);
        }
        else
        {
            Debug.Log("samename!!");
            StartCoroutine(GameObject.FindObjectOfType<MyARList>().showingWarningSamename());//���
        }



    }



}
