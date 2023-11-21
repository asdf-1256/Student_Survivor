using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private UINotice _uiNotice;

    public void Notice() {
        _uiNotice.Notice();
    }
}
