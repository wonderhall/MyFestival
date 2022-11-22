using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScOBJ_TempleteInfo
{
    public string TemppleteName;
    public Sprite preview;
    public Transform[] item;
}

[CreateAssetMenu(fileName = "Templete", menuName = "Templete")]
public class ScriptableObject_TempleteItems : ScriptableObject
{
    public ScOBJ_TempleteInfo[] ItemTemplete;
}
