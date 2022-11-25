using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MyTemplete 
{

    public List<CurrentTemplete> myTemplete=new List<CurrentTemplete>();
   
}


[Serializable]

public class CurrentTemplete
{
    public string tempeteName;
    public string PreviewName;
    public string date;
    public double latitude;
    public double longitude;
    public List<CurItemList> items = new List<CurItemList>();

    public void print()
    {
        Debug.Log(tempeteName);
        Debug.Log(date);
        Debug.Log(latitude);
        Debug.Log(longitude);
        foreach (var item in items)
        {
            Debug.Log(item.ItemName);
            foreach (var itemlist in item.itemTranform)
            {
                foreach (var po in itemlist.itemPositon)
                {
                    Debug.Log(po);
                }
                foreach (var ro in itemlist.itemRotate)
                {
                    Debug.Log(ro);
                }
                foreach (var sca in itemlist.itemScale)
                {
                    Debug.Log(sca);
                }
            }

        }
    }
}

[Serializable]
public class CurItemList
{
    public int Index;
    public string ItemName;
    public string TextName;
    public List<ItemTransform> itemTranform= new List<ItemTransform>();
}
[Serializable]
public class ItemTransform
{
    public float[] itemPositon;
    public float[] itemRotate;
    public float[] itemScale;
    public ItemTransform(/*ItemPositon po, ItemRotate rot, ItemScale scale*/)
    {
        itemPositon = new float[3];
        itemRotate = new float[3];
        itemScale = new float[3];
    }
}

[Serializable]
public class ItemPositon
{
    public float x;
    public float y;
    public float z;
}
[Serializable]
public class ItemScale
{
    public float x;
    public float y;
    public float z;
}
[Serializable]
public class ItemRotate
{
    public float x;
    public float y;
    public float z;
}
