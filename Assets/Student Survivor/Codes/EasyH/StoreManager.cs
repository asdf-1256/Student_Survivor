using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;
    public List<StoreData> JoinedDongAris;

    private void Awake()
    {
        instance = this;
        JoinedDongAris = new List<StoreData>();
    }
    public void SelectDongAri(StoreData storeData)
    {
        Debug.Log("µ∑ √Ê∫–«‘!");
        JoinedDongAris.Add(storeData);
        DataManager.Instance.SubMoney(storeData.price);
    }
    public void CancelDongAri(StoreData storeData)
    {
        Debug.Log("µ∑ ∑—πÈ«‘");
        DataManager.Instance.AddMoney(storeData.price);
        JoinedDongAris.Remove(storeData);
    }
}
