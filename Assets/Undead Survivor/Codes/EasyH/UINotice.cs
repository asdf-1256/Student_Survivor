using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINotice : MonoBehaviour {

    [SerializeField] float _waitTime;

    public void Notice()
    {
        StartCoroutine(NoticeRoutine());
    }

    IEnumerator NoticeRoutine()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return _waitTime;

        gameObject.SetActive(false);
    }
}
