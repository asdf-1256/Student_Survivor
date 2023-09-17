using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed // 加己 累己
    {
        get { return GameManager.Instance.playerId == 0 ? 1.1f : 1f; }
    }
    public static float WeaponSpeed // 加己 累己
    {
        get { return GameManager.Instance.playerId == 1 ? 1.1f : 1f; }
    }
    public static float WeaponRate // 加己 累己
    {
        get { return GameManager.Instance.playerId == 1 ? 0.9f : 1f; }
    }
    public static float Damage // 加己 累己
    {
        get { return GameManager.Instance.playerId == 2 ? 1.2f : 1f; }
    }
    public static int Count // 加己 累己
    {
        get { return GameManager.Instance.playerId == 3 ? 1 : 0; }
    }
}
