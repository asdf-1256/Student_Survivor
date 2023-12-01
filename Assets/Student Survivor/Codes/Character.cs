using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed // �Ӽ� �ۼ�
    {
        get { return GameManager.Instance.playerId == 0 ? 1.1f : 1f; }
    }
    public static float WeaponSpeed // �Ӽ� �ۼ�
    {
        get { return GameManager.Instance.playerId == 1 ? 1.1f : 1f; }
    }
    public static float WeaponRate // �Ӽ� �ۼ�
    {
        get { return GameManager.Instance.playerId == 1 ? 0.9f : 1f; }
    }
    public static float Damage // �Ӽ� �ۼ�
    {
        get { return GameManager.Instance.playerId == 2 ? 1.2f : 1f; }
    }
    public static int Count // �Ӽ� �ۼ�
    {
        get { return GameManager.Instance.playerId == 3 ? 1 : 0; }
    }
}
