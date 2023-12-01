using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextMesh : MonoBehaviour
{
    private TextMeshPro text;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float disappearanceTime;
    Color alpha;
    float timer = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        alpha.a = Mathf.Lerp(alpha.a, 0, disappearanceTime * Time.deltaTime);
        text.color = alpha;
        timer += Time.deltaTime;
        if(timer > disappearanceTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        timer = 0f;
        alpha.a = 1;
    }
    public void Init(Transform hudTransform, float damage)
    {
        transform.position = hudTransform.position;
        text.text = (damage % 1 == 0) ? ((int)damage).ToString() : string.Format("{0:F1}", damage);
    }
}
