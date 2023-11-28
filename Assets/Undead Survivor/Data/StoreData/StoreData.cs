using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreData", menuName = "Scriptable Object/StoreData")]
public class StoreData : ScriptableObject
{
    public enum DongAriType
    {
        PNC, SSOS, EXPERT
    }

    public DongAriType Type;
    public string Name;
    public int price;
    public float degree;
}