using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DongAriData", menuName = "Scriptable Object/DongAriData")]
public class DongAriData : StoreData
{
    public enum DongAriType
    {
        PNC, SSOS, EXPERT
    }

    public DongAriType dongAriType;
    public float degree;
}
