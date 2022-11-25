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
    //�н�
    string saveFileName;
    private string saveFilePath;
    //

    [Header("ui root")]
    public Transform contentsRoot;
    [Header("������ ui")]
    public GameObject AddItem_UIPrefab;
    [Header("��ü ������ ����Ʈ")]
    public GameObject[] SelectedTempletePage;
    [Header("������ ������Ʈ �⺻ �Ÿ�")]
    public Vector3 distanceFromCamera = new Vector3(0, 0, 3);

    private Dictionary<string, CurrentTemplete> DicMyTemp;

    private Text _tempNname;

    private CurItemList[] ct;

    private Button renamer;
    private InputField inputField;

    public void GetJsonWithMyList()//���̸���Ʈâ�� ���鼭 ���� ����� ���� ������Ʈ ����
    {        //�н�
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

        //�����̿�����Ʈ �����
        if (contentsRoot.childCount < DicMyTemp.Count)//�̹� ��������ִ��� üũ
        {
            //Debug.Log("����Ʈ �����");
            for (int i = 0; i < DicMyTemp.Count; i++)
            {
                if (i < contentsRoot.childCount) continue;
                string str = DeserialJson.myTemplete[i].tempeteName;
                string date = DeserialJson.myTemplete[i].date;
                GameObject Newitem = Instantiate(AddItem_UIPrefab);
                Newitem.transform.SetParent(contentsRoot, false);
                Newitem.name = str + "_" + i;

                Newitem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = PreviewTextrueLoad(date); //������
                Newitem.transform.GetChild(1).GetComponent<Text>().text = str;

                Text MenuTextName = Newitem.transform.GetChild(1).GetComponent<Text>();

                //��ư��� ���̱� ����Ʈ ����â �����ֱ�
                Newitem.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(ShowSelectedTempleInfo(MenuTextName, str, DicMyTemp)); });
            }
        }//if����
        else Debug.Log("�̸̹�������ִ�");

    }
    //����Ʈ ����â_ �ۼ� �� ����
    IEnumerator ShowSelectedTempleInfo(Text menutext, string name, Dictionary<string, CurrentTemplete> dic)
    {
        SelectedTempletePage[0].SetActive(false);//�ش������� ����
        SelectedTempletePage[1].SetActive(true);//�ش������� ����
        yield return new WaitForSeconds(0.1f);
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        CurrentTemplete thisTempleteInfo = new CurrentTemplete();
        dic.TryGetValue(name, out thisTempleteInfo);//��ųʸ����� �̸����� �߷����ش��ϴ� Ŭ���� �̾ƿ´�.

        //��ųʸ����� �ʿ� ���� �̱�
        string date = thisTempleteInfo.date;
        string previewName = thisTempleteInfo.PreviewName;
        double latitude = thisTempleteInfo.latitude;
        double longitude = thisTempleteInfo.longitude;

        ct = thisTempleteInfo.items.ToArray();
        //List<CurItemList> curItems = thisTempleteInfo.items;




        ////�����ֱ�
        GameObject icon = GameObject.Find("IconText");
        icon.GetComponent<Text>().text = name; //�̸�
        //
        _tempNname = GameObject.Find("Text_TempName").GetComponent<Text>();
        _tempNname.text = name;  //����
                                 //                                          ////
        GameObject gps = GameObject.Find("Text_GPS");
        gps.GetComponent<Text>().text = latitude.ToString() + "," + longitude.ToString();  //gps

        Button preview = GameObject.Find("Preview").GetComponent<Button>();
        preview.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //������
        //preview.onClick.AddListener(() => CreateObjFromData(curItems.ToArray())); ����� ���̴� ������ �־ ��ư���� ��������

        GameObject previewSmall = GameObject.Find("PreviewSmall");
        previewSmall.GetComponent<Image>().sprite = PreviewTextrueLoad(previewName); //������

        //�̸� �ٲٱ�
        renamer = GameObject.Find("Bt_Renamer").GetComponent<Button>();
        inputField = renamer.transform.GetComponentInChildren<InputField>();
        renamer.onClick.AddListener(delegate { ShowRenameEditor(menutext, _tempNname.text, renamer.transform, thisTempleteInfo, name); });


        //����Ʈ ����
        Button Bt_deleteTemp = GameObject.Find("Bt_Delete").GetComponent<Button>();
        Bt_deleteTemp.onClick.AddListener(() => deleteConfirm(name, SelectedTempletePage[2], selectedObject));

        ////�ۺ���
        //Button Bt_Publish = GameObject.Find("Bt_Publish").GetComponent<Button>();// ����� ���̴� ������ �־ ��ư���� ��������
        ////Bt_Publish.onClick.AddListener(() => Publish(thisTempleteInfo));


        //���ư TempleteIcon
        Button Bt_Back = GameObject.Find("Back").GetComponent<Button>();
        Bt_Back.onClick.AddListener(() => SelectedTempletePage[1].SetActive(false));
        Button Bt_TempleteIcon = GameObject.Find("TempleteIcon").GetComponent<Button>();
        Bt_TempleteIcon.onClick.AddListener(() => fromToPage(SelectedTempletePage[1], SelectedTempletePage[0]));

    }

    //���ø� ����â_���ø� ����

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
        //�켱 �̹������� ����
        CurrentTemplete ct = new CurrentTemplete();
        DicMyTemp.TryGetValue(name, out ct);
        string savepriviewPath = SaveLoadTemplete.SavePath + ct.date + ".png";
        File.Delete(savepriviewPath);

        //�񿡼� ����
        if (DicMyTemp.ContainsKey(name)) DicMyTemp.Remove(name); //dic���� �̸����� �����

        MyTemplete newTemp = new MyTemplete();  //�ٽ� ���� ���� Ŭ������ ����� �־� �ִ�.
        foreach (var item in DicMyTemp)
        {
            newTemp.myTemplete.Add(item.Value);
        }
        Destroy(selectedObject);


        SaveLoadTemplete.SaveTemplete(newTemp, page);
    }
    //���ø� ����â-������ �����
    Sprite PreviewTextrueLoad(string name)
    {

        string path = SaveLoadTemplete.SavePath + name + ".png";
        Texture2D tex = null;    //�� �ý��� ������ ����Ʈ�� �ε��ϰ� �־��ش�.
        byte[] filedata;
        filedata = File.ReadAllBytes(path);
        tex = new Texture2D(0, 0);
        tex.LoadImage(filedata);  //�־����.

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, new Vector2(0, 0), 1); //��������Ʈ�� ����

        return sprite;

    }
    //���ø� ����â_���ø� �̸� ����
    #region �̸��ٲٱ⿡ ���� �Լ�
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
            }//������ �̸��ִ��� üũ
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


        MyTemplete reNameTemplete = new MyTemplete();  //�ٽ� ���� ���� Ŭ������ ����� �־� �ִ�.
        //�̸���
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

        Dictionary<string, CurrentTemplete> dic = SaveLoadTemplete.DicByJson(saveFilePath);//���̽��� �о ��ųʸ�ȭ

        CurrentTemplete ct;
        ct = CompairName(curTemp, dic);//�̸��� ��ġ�� �ʴ��� üũ
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
        //Debug.Log($"{from}���� {to}�� ü����");
        from.SetActive(false);
        to.SetActive(true);
    }
    //���ø� ������ �а� ������Ʈ ����
    public void CreateObjFromData()/*CurItemList[] ct*/
    {

        //�θ𸸵���ֱ�
        GameObject root;
        if (GameObject.Find("---CurrentItemList---")) root = GameObject.Find("---CurrentItemList---");
        else root = new GameObject("---CurrentItemList---");

        root.transform.position = distanceFromCamera;
        //

        //// so����Ʈ �޾Ƽ� ����
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
                string str = splitName[0];//������ ������Ʈ �̸�  
                                          //Debug.Log(CgItem.name);
                for (int i = 0; i < CgItem.placeableObjects.Length; i++)
                {
                    if (str == CgItem.placeableObjects[i].prefab.name)//�о�� �ƾ����̸��� ��ũ���ͺ������Ʈ �������� ��
                    {
                        Debug.Log(CgItem.placeableObjects[i].prefab.name);
                        newObject = Instantiate(CgItem.placeableObjects[i].prefab, root.transform).gameObject;
                        //newObject.name = CgItem.placeableObjects[i].prefab.name;
                        newObject.name = item.ItemName;

                    }
                }
            }//so��Ʈ���� ������ �̸� ���ؼ� �������ش�.

            #region �̵��� �־��ֱ�

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
            //�������ݱ�


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
    }//�̸� ��ĥ�� ���

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