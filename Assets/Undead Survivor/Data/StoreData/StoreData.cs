using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreData", menuName = "Scriptable Object/StoreData")]
public class StoreData : ScriptableObject
{
    public enum DataType
    {
        DongAri, Skin
    }

    public Sprite Icon;
    public DataType dataType;
    public string Name;
    public string Descript;
    public int price;
}