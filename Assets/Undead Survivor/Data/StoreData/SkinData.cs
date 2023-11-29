using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Scriptable Object/SkinData")]
public class SkinData : StoreData
{
    public enum SkinType
    {
        Nomal, Nerd, Hood, Girl
    }

    public SkinType skinType;
    public int SkinID;
}
