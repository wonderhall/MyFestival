using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScOBJ_ItemInfo
{
    public string itemName;
    public Sprite preview;
    public Transform prefab;
}
[CreateAssetMenu(fileName = "Category", menuName = "Category")]
public class ScriptableObject_CategoryItems : ScriptableObject
{
    public ScOBJ_ItemInfo[] placeableObjects;

}