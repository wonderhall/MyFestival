using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateByGameObjectList : MonoBehaviour
{
    [Header("�����۸���Ʈ")]
    public List<Transform> items;
    [Header("������ ������Ʈ �⺻ �Ÿ�")]
    public Vector3 distanceFromCamera = new Vector3(0, 0, 3);
    public void CreateByObjectList()
    {
        //�θ𸸵���ֱ�
        GameObject root;
        if (GameObject.Find("---CurrentItemList---")) root = GameObject.Find("---CurrentItemList---");
        else root = new GameObject("---CurrentItemList---");
        root.transform.position = distanceFromCamera;
        GameObject newObject = null;
        foreach (var item in items)
        {
            string[] splitName = item.name.Split('_');
            string str = splitName[0]+"_"+ splitName[1] +"_"+splitName[2];
            newObject = Instantiate(item, root.transform).gameObject;
            newObject.name = str;
        }

        this.transform.parent.parent.parent.parent.gameObject.SetActive(false);
        this.transform.parent.parent.parent.parent.parent.gameObject.SetActive(false);
        Debug.Log(this.transform.parent.parent.parent.parent.gameObject.name);
        //this.transform.parent.parent.parent.parent.gameObject.SetActive(false);
    }
}
