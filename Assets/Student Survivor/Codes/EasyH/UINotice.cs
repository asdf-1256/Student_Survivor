using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotice : MonoBehaviour {

    [SerializeField] float _waitTime;
    [SerializeField] Text _text;

    public void Notice(string msg)
    {
        gameObject.SetActive(true);
        _text.text = msg;
        StartCoroutine(NoticeRoutine(msg));
    }

    IEnumerator NoticeRoutine(string msg)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return new WaitForSeconds(_waitTime);

        gameObject.SetActive(false);
    }
}
