using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MyARList : MonoBehaviour
{
    //패스
    string saveFileName;
    private string saveFilePath;
    //

    [Header("ui root")]
    public Transform contentsRoot;
    [Header("생성할 ui")]
    public GameObject AddItem_UIPrefab;
    [Header("교체 페이지 리스트")]
    public GameObject[] SelectedTempletePage;
    [Header("생성될 오브젝트 기본 거리")]
    public Vector3 distanceFromCamera = new Vector3(0, 0, 3);

    private Dictionary<string, CurrentTemplete> DicMyTemp;

    private Text _tempNname;

    private CurItemList[] ct;

    private Button renamer;
    private InputField inputField;

    public void GetJsonWithMyList()//마이리스트창을 열면서 전에 만들어 놓은 오브젝트 삭제
    {        //패스
        string saveFileName = "myTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
        //

        if (GameObject.Find("---CurrentItemList---"))
        {
            GameObject root = GameObject.Find("---CurrentItemList---");
            for (int i = 0; i < root.transform.childCount; i++)
            {
                Destroy(root.transform.GetChild(i).gameObject);
            }
            Destroy(root.gameObject);
            GameObject handle = GameObject.Find("handler");
            Destroy(handle);
        }
        if (!Directory.Exists(SaveLoadTemplete.SavePath)) SaveLoadTemplete.newEmptyData(saveFileName);

        MyTemplete DeserialJson = SaveLoadTemplete.MyTempByJson(saveFilePath);
        DicMyTemp = new Dictionary<string, CurrentTemplete>();
        foreach (var item in DeserialJson.myTemplete) DicMyTemp.Add(item.tempeteName, item);

        //유아이오브젝트 만들기
        if (contentsRoot.childCount < DicMyTemp.Count)//이미 만들어져있는지 체크
        {
            //Debug.Log("리스트 만들기");
            for (int i = 0; i < DicMyTemp.Count; i++)
            {
                if (i < contentsRoot.childCount) continue;
                string str = DeserialJson.myTemplete[i].tempeteName;
                string date = DeserialJson.myTemplete[i].date;
                GameObject Newitem = Instantiate(AddItem_UIPrefab);
                Newitem.transform.SetParent(contentsRoot, false);
                Newitem.name = str + "_" + i;

                Newitem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = PreviewTextrueLoad(date); //프리뷰
                Newitem.transform.GetChild(1).GetComponent<Text>().text = str;

                Text MenuTextName = Newitem.transform.GetChild(1).GetComponent<Text>();

                //버튼기능 붙이기 리스트 정보창 보여주기
                Newitem.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ShowSelectedTempleInfo(MenuTextName, str, DicMyTemp)); });
            }
        }//if종료
        else Debug.Log("이미만들어져있다");

    }
    //리스트 정보창_ 작성 및 열기
    IEnumerator ShowSelectedTempleInfo(Text menutext, string name, Dictionary<string, CurrentTemplete> dic)
    {
        SelectedTempletePage[0].SetActive(false);//해당페이지 오픈
        SelectedTempletePage[1].SetActive(true);//해당페이지 오픈
        yield return new WaitForSeconds(0.1f);
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        CurrentTemplete thisTempleteInfo = new CurrentTemplete();
        dic.TryGetValue(name, out thisTempleteInfo);//딕셔너리에서 이름으로 발류에해당하는 클리스 뽑아온다.

        //딕셔너리에서 필요 정보 뽑기
        string date = thisTempleteInfo.date;
        string previewName = thisTempleteInfo.PreviewName;
        double latitude = thisTempleteInfo.latitude;
        double longitude = thisTempleteInfo.longitude;

        ct = thisTempleteInfo.items.ToArray();
        //List<CurItemList> curItems = thisTempleteInfo.items;




        ////정보넣기
        GameObject icon = GameObject.Find("IconText");
        icon.GetComponent<Text>().text = name; //이름
        //
        _tempNname = GameObject.Find("Text_TempName").GetComponent<Text>();
        _tempNname.text = name;  //날자
                                 //                                          ////
        GameObject gps = GameObject.Find("Text_GPS");
        gps.GetComponent<Text>().text = latitude.ToString() + "," + longitude.ToString();  //gps

        Button preview = GameObject.Find("Preview").GetComponent<Button>();
        preview.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //프리뷰
        //preview.onClick.AddListener(() => CreateObjFromData(curItems.ToArray())); 명령이 쌓이는 문제가 있어서 버튼으로 직접연결

        GameObject previewSmall = GameObject.Find("PreviewSmall");
        previewSmall.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //프리뷰

        //이름 바꾸기
        renamer = GameObject.Find("Bt_Renamer").GetComponent<Button>();
        inputField = renamer.transform.GetComponentInChildren<InputField>();
        renamer.onClick.AddListener(delegate { ShowRenameEditor(menutext, _tempNname.text, renamer.transform, thisTempleteInfo, name); });


        //리스트 삭제
        Button Bt_deleteTemp = GameObject.Find("Bt_Delete").GetComponent<Button>();
        Bt_deleteTemp.onClick.AddListener(() => deleteConfirm(name, SelectedTempletePage[2], selectedObject));

        ////퍼블리쉬
        //Button Bt_Publish = GameObject.Find("Bt_Publish").GetComponent<Button>();// 명령이 쌓이는 문제가 있어서 버튼으로 직접연결
        ////Bt_Publish.onClick.AddListener(() => Publish(thisTempleteInfo));


        //백버튼 TempleteIcon
        Button Bt_Back = GameObject.Find("Back").GetComponent<Button>();
        Bt_Back.onClick.AddListener(() => SelectedTempletePage[1].SetActive(false));
        Button Bt_TempleteIcon = GameObject.Find("TempleteIcon").GetComponent<Button>();
        Bt_TempleteIcon.onClick.AddListener(() => fromToPage(SelectedTempletePage[1], SelectedTempletePage[0]));

    }

    //템플릿 정보창_템플릿 삭제

    void deleteConfirm(string name, GameObject page, GameObject selectedObject)
    {
        fromToPage(SelectedTempletePage[1], SelectedTempletePage[2]);
        Button yes = GameObject.Find("Button_Yes").GetComponent<Button>();
        yes.onClick.AddListener(() => deleteTempFromList(name, page, selectedObject));
        Button no = GameObject.Find("Button_No").GetComponent<Button>();
        no.onClick.AddListener(() => fromToPage(SelectedTempletePage[2], SelectedTempletePage[1]));
    }
    void deleteTempFromList(string name, GameObject page, GameObject selectedObject)
    {
        //우선 이미지부터 삭제
        CurrentTemplete ct = new CurrentTemplete();
        DicMyTemp.TryGetValue(name, out ct);
        string savepriviewPath = SaveLoadTemplete.SavePath + ct.date + ".png";
        File.Delete(savepriviewPath);

        //딕에서 삭제
        if (DicMyTemp.ContainsKey(name)) DicMyTemp.Remove(name); //dic에서 이름으로 지우기

        MyTemplete newTemp = new MyTemplete();  //다시 쓰기 위해 클래스를 만들고 넣어 주다.
        foreach (var item in DicMyTemp)
        {
            newTemp.myTemplete.Add(item.Value);
        }
        Destroy(selectedObject);


        SaveLoadTemplete.SaveTemplete(newTemp, page);
    }
    //템플릿 정보창-프리뷰 만들기
    Sprite PreviewTextrueLoad(string name)
    {

        string path = SaveLoadTemplete.SavePath + name + ".png";
        Texture2D tex = null;    //빈 택스쳐 생성후 바이트로 로드하고 넣어준다.
        byte[] filedata;
        filedata = File.ReadAllBytes(path);
        tex = new Texture2D(0, 0);
        tex.LoadImage(filedata);  //넣어줬다.

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 1); //스프라이트로 변경

        return sprite;

    }
    //템플릿 정보창_템플릿 이름 삭제
    #region 이름바꾸기에 쓰인 함수
    public void ShowRenameEditor(Text menutext, string _tempNname, Transform renamer, CurrentTemplete thisTempleteInfo, string Tname)
    {
        Debug.Log(renamer.name);
        Debug.Log(_tempNname);
        renamer.transform.GetChild(0).gameObject.SetActive(false);
        renamer.transform.GetChild(1).gameObject.SetActive(true);

        InputField inputField = renamer.GetComponentInChildren<InputField>();
        inputField.onEndEdit.AddListener(delegate { LockInput(menutext, _tempNname, inputField, thisTempleteInfo, Tname); });
    }
    void LockInput(Text menutext, string _tempNname, InputField input, CurrentTemplete thisTempleteInfo, string Tname)
    {
        bool isSameName = false;
        if (input.text.Length > 0)
        {
            foreach (var item in DicMyTemp)
            {
                if (input.text == item.Key)
                    isSameName = true;
            }//동일한 이름있는지 체크
            if (!isSameName)
            {
                _tempNname = input.text;
                Manager.instance.TempName = _tempNname;
                renamer.transform.GetChild(1).gameObject.SetActive(false);
                renamer.transform.GetChild(0).GetComponent<Text>().text = _tempNname;
                renamer.transform.GetChild(0).gameObject.SetActive(true);
                menutext.text = input.text;
                ReSaveTempletName(input.text, Tname, thisTempleteInfo);
                Debug.Log("get enter");
            }
            else
            {
                Debug.Log("sameName");
                Debug.Log("sameName");
                StartCoroutine(showingWarningSamename());
            }
        }
        else if (input.text.Length == 0)
        {
            input.placeholder.GetComponent<Text>().text = Manager.instance.TempName;
            Debug.Log("input empty");
        }
    }

    public void ReSaveTempletName(string rename, string Tname, CurrentTemplete thisTempleteInfo)
    {
        thisTempleteInfo.tempeteName = rename;
        DicMyTemp[Tname] = thisTempleteInfo;


        MyTemplete reNameTemplete = new MyTemplete();  //다시 쓰기 위해 클래스를 만들고 넣어 주다.
        //이름비교
        foreach (var item in DicMyTemp)
        {
            reNameTemplete.myTemplete.Add(item.Value);
        }
        SaveLoadTemplete.SaveTemplete(reNameTemplete, SelectedTempletePage[1]);

        //foreach (var item in DicMyTemp)
        //{
        //    newTemp.myTemplete.Add(item.Value);
        //}
        //Destroy(selectedObject);
        //SaveLoadTemplete.SaveTemplete(newTemp, page);
    }
    #endregion

    public void Publish(/*CurrentTemplete curtemp*/)
    {
        Text currentTempName = GameObject.Find("Text_TempName").GetComponent<Text>();

        CurrentTemplete curTemp = new CurrentTemplete();
        DicMyTemp.TryGetValue(currentTempName.text, out curTemp);

        string saveFileName = "PublishTemplete";
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";
        //

        Dictionary<string, CurrentTemplete> dic = SaveLoadTemplete.DicByJson(saveFilePath);//제이슨을 읽어서 딕셔너리화

        CurrentTemplete ct;
        ct = CompairName(curTemp, dic);//이름이 겹치지 않는지 체크
        if (ct.tempeteName != null)
        {
            SelectedTempletePage[1].SetActive(false);
            SelectedTempletePage[3].SetActive(true);
            Button yes = GameObject.Find("Button_Yes").GetComponent<Button>();
            yes.onClick.AddListener(() => SaveLoadTemplete.SaveTempleteToShop(curTemp, SelectedTempletePage[3], saveFilePath));
            Button no = GameObject.Find("Button_No").GetComponent<Button>();
            no.onClick.AddListener(() => fromToPage(SelectedTempletePage[3], SelectedTempletePage[1]));
        }
        else
        {
            Debug.Log("samename!!");
            StartCoroutine(showingWarningSamename());
        }
    }
    void fromToPage(GameObject from, GameObject to)
    {
        //Debug.Log($"{from}에서 {to}로 체인지");
        from.SetActive(false);
        to.SetActive(true);
    }
    //템플릿 정보를 읽고 오브젝트 생성
    public void CreateObjFromData()/*CurItemList[] ct*/
    {

        //부모만들어주기
        GameObject root;
        if (GameObject.Find("---CurrentItemList---")) root = GameObject.Find("---CurrentItemList---");
        else root = new GameObject("---CurrentItemList---");

        root.transform.position = distanceFromCamera;
        //

        //// so리스트 받아서 생성
        ScriptableObject_CategoryItems[] SO = this.GetComponent<ButtonAction_SelectCategory>().SO_List;

        GameObject newObject = null;
        foreach (var item in ct)
        {

            float[] nPo = new float[3];
            float[] nRot = new float[3];
            float[] nSca = new float[3];

            foreach (ScriptableObject_CategoryItems CgItem in SO)
            {
                string[] splitName = item.ItemName.Split('_');
                string str = splitName[0];//선택한 오브젝트 이름  
                                          //Debug.Log(CgItem.name);
                for (int i = 0; i < CgItem.placeableObjects.Length; i++)
                {
                    if (str == CgItem.placeableObjects[i].prefab.name)//읽어온 아아템이름과 스크랩터블오브젝트 아이템을 비교
                    {
                        Debug.Log(CgItem.placeableObjects[i].prefab.name);
                        newObject = Instantiate(CgItem.placeableObjects[i].prefab, root.transform).gameObject;
                        //newObject.name = CgItem.placeableObjects[i].prefab.name;
                        newObject.name = item.ItemName;

                    }
                }
            }//so리트스와 아이템 이름 비교해서 생성해준다.

            #region 이동값 넣어주기

            foreach (ItemTransform itemV in item.itemTranform)
            {
                for (int i = 0; i < 3; i++)
                {
                    nPo[i] = itemV.itemPositon[i];
                    nRot[i] = itemV.itemRotate[i];
                    nSca[i] = itemV.itemScale[i];
                }
            }
            Vector3 nPosition = new Vector3(nPo[0], nPo[1], nPo[2]);
            Vector3 nRotation = new Vector3(nRot[0], nRot[1], nRot[2]);
            Vector3 nScale = new Vector3(nSca[0], nSca[1], nSca[2]);

            newObject.transform.localPosition = nPosition;
            newObject.transform.localEulerAngles = nRotation;
            newObject.transform.localScale = nScale;
            #endregion

            if (newObject.tag == "Text")
                newObject.GetComponent<Banner>().text.text = item.TextName;

            //}
            //페이지닫기


        }
        SelectedTempletePage[1].SetActive(false);
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).GetComponent<MoveController>())
                root.transform.GetChild(i).GetComponent<MoveController>().Moving = true;
        }
    }

    IEnumerator showingWarningSamename()
    {
        SelectedTempletePage[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SelectedTempletePage[4].SetActive(false);
    }//이름 겹칠때 경고

    CurrentTemplete CompairName(CurrentTemplete _new, Dictionary<string, CurrentTemplete> _old)
    {
        foreach (var item in _old)
        {
            if (_new.tempeteName == item.Key)
                _new = new CurrentTemplete();
        }
        return _new;
    }
}