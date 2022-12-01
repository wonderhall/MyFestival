using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


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
    //퍼블리쉬페이지 정보넣기
    public Text cur_name;
    public InputField if_name;
    public Text cur_GPS;
    public Dropdown dropdown;
    public double ChoiceLatitude;
    public double ChoiceLongitude;
    Dictionary<int, tourGps> dic = new Dictionary<int, tourGps>();
    public Button Bt_save;

    //
    [Tooltip("페이지 열릴때 받아옮")]
    public double gotLatitude;
    public double gotlongitude;
    public CurrentTemplete cTemp;
    public Dictionary<string, CurrentTemplete> pubDic;


    private void OnEnable()
    {
        #region 관광지 gps가져와서 옵션 넣어주기
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
        if_name.onEndEdit.AddListener(delegate { NameValuedChanged(if_name); });//이름바꾸기
        dropdown.onValueChanged.AddListener(delegate { GpsValuedChanged(dropdown, dic); });//gps바꾸기
        Bt_save.onClick.AddListener(delegate { TryPublish(); });//퍼블리쉬

    }

    void NameValuedChanged(InputField thisInputField) //이름바꾸기
    {
        if (thisInputField.text.Length > 0)
            cur_name.text = thisInputField.text;
    }
    void GpsValuedChanged(Dropdown change, Dictionary<int, tourGps> dic)//gps 바꾸기
    {
        tourGps tg = new tourGps();
        dic.TryGetValue(change.value, out tg);
        if (change.value == 0)
        {
            ChoiceLatitude = gotLatitude;
            ChoiceLongitude = gotlongitude;
        }
        else
        {
            ChoiceLatitude = tg.latitude;
            ChoiceLongitude = tg.longitude;
        }

        string changeGps = ChoiceLatitude + "," + ChoiceLongitude;
        cur_GPS.text = changeGps;
    }


    public void TryPublish()//퍼블리쉬
    {
        //변경한 값으로 조정
        if (if_name.text.Length > 0)
            cTemp.tempeteName = if_name.text;
        cTemp.latitude = ChoiceLatitude;
        cTemp.longitude = ChoiceLongitude;
        //
        string saveFileName = "PublishTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";

        Dictionary<string, CurrentTemplete> pubDic = SaveLoadTemplete.DicByJson(saveFilePath);//제이슨을 읽어서 딕셔너리화

        CurrentTemplete ct;
        ct = GameObject.FindObjectOfType<MyARList>().CompairName(cTemp, pubDic);//이름이 겹치지 않는지 체크
        if (ct.tempeteName != null)
        {
            SaveLoadTemplete.SaveTempleteToShop(ct, this.transform.parent.gameObject, saveFilePath);//저장
            Destroy(this);
        }
        else
        {
            Debug.Log("samename!!");
            StartCoroutine(GameObject.FindObjectOfType<MyARList>().showingWarningSamename());//경고
        }



    }



}
