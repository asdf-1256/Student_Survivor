using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] private UINotice _uiNotice;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Notice(string msg)
    {
        _uiNotice.Notice(msg);
    }
}
