using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class itemsInfo{
    //public string iName;
    public Sprite preview;
    public Transform prefab;
}

[CreateAssetMenu(fileName ="item",menuName ="Item")]

public class ScriptableObjectTest1 : ScriptableObject
{
    //public float InitialValue;

    //[NonSerialized]
    //public float RuntimeValue;
    ////public Vector3[] spawnPoints;

    //public void OnAfterDesirialize()
    //{
    //    RuntimeValue = InitialValue;
    //}

    //public void OnBeforeSerialize() { }
    public itemsInfo[] items;
}
