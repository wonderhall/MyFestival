using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARLocation;
using System.IO;

public class CreateHotSopt : MonoBehaviour
{

    #region test
    //public List<GameObject> tempObj;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //    GameObject newObject= new GameObject();
    //        tempObj.Add(newObject);
    //        newObject.name = "HsObject_" +i;
    //    }

    //    GameObject HsObject= new GameObject("HsObject");

    //    foreach (var item in tempObj)
    //    {
    //        item.transform.parent = HsObject.transform;
    //    }
    //    HsObject.AddComponent<PlaceAtLocation>();
    //    PlaceAtLocation pl = HsObject.GetComponent<PlaceAtLocation>();
    //    pl.LocationOptions.LocationInput = new LocationPropertyData();
    //    pl.LocationOptions.LocationInput.Location.Latitude = 0;
    //    pl.LocationOptions.LocationInput.Location.Longitude = 0;

    //    HsObject.transform.position = new Vector3(0,0,0);
    //} 
    #endregion

    //// so����Ʈ �޾Ƽ� ����
    public ScriptableObject_CategoryItems[] SO;
    //�о�
    private string saveFileName = "myTemplete";


    private CurrentTemplete[] ct;

    public float defaultHeight;
    public float defaultDistance;

    private void OnEnable()
    {
        //�н�
        string saveFilePath = SaveLoadTemplete.SavePath + saveFileName + ".json";

        if (File.Exists(saveFilePath))
        {
            MyTemplete allTemplete = SaveLoadTemplete.MyTempByJson(saveFilePath);//���̽��� ���ø����� �б�

            foreach (var ct in allTemplete.myTemplete)
            {
                GameObject HsObject = new GameObject("HsObject_" + ct.tempeteName);
                HsObject.transform.position = new Vector3(0, 0, 0);
                HsObject.AddComponent<PlaceAtLocation>();
                PlaceAtLocation pl = HsObject.GetComponent<PlaceAtLocation>();
                pl.LocationOptions.LocationInput = new LocationPropertyData();
                pl.LocationOptions.LocationInput.Location.Latitude = ct.latitude;
                pl.LocationOptions.LocationInput.Location.Longitude = ct.longitude;

                GameObject newObject = null;
                foreach (var item in ct.items)
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
                                newObject = Instantiate(CgItem.placeableObjects[i].prefab, HsObject.transform).gameObject;
                                if (newObject.GetComponent<MoveController>())
                                    newObject.GetComponent<MoveController>().Moving = true;
                                newObject.transform.GetChild(newObject.transform.childCount - 1).gameObject.SetActive(false);
                                //newObject.name = CgItem.placeableObjects[i].prefab.name;
                                newObject.name = item.ItemName;
                            }
                        }
                    }//so��Ʈ���� ������ �̸� ���ؼ� �������ش�.


                    foreach (ItemTransform itemV in item.itemTranform)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            nPo[i] = itemV.itemPositon[i];
                            nRot[i] = itemV.itemRotate[i];
                            nSca[i] = itemV.itemScale[i];
                        }
                    }
                    Vector3 nPosition = new Vector3(nPo[0], nPo[1]+ defaultHeight, nPo[2]+defaultDistance);
                    Vector3 nRotation = new Vector3(nRot[0], nRot[1], nRot[2]);
                    Vector3 nScale = new Vector3(nSca[0], nSca[1], nSca[2]);

                    newObject.transform.localPosition = nPosition;
                    newObject.transform.localEulerAngles = nRotation;
                    newObject.transform.localScale = nScale;
                }
            }//so��Ʈ���� ������ �̸� ���ؼ� �������ش�.
        }//if�ݱ�
    }//onEnable�ݱ�

}

