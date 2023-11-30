using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Windows : BulletBase
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _coll;
    public Sprite[] windowSprites;

    //[SerializeField] private float[] breakTime;

    float timer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _coll = GetComponent<BoxCollider2D>();
        //breakTime = new float[windowSprites.Length];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Init(bool isAI, SkillData skillData, int level)
    {
        base.Init(isAI, skillData, level);
        transform.position = GameManager.Instance.player.scanner.nearestTarget.position;
        transform.rotation = Quaternion.identity;
    }
    IEnumerator VibrateRoutine()
    {
        float breakTimer = 0f;
        int phase = 0;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            breakTimer += timer;

            float noise = Mathf.PerlinNoise(timer, 0f);
            noise = (noise - 0.5f) * 0.1f;
            Vector3 nextpos = transform.position;
            nextpos.x += noise;
            transform.position = nextpos;

            if (breakTimer > lifeTime / 3)
            {
                phase++;
                if(phase == windowSprites.Length - 1)
                {
                    _coll.enabled = true;
                }
                _spriteRenderer.sprite = windowSprites[Mathf.Min(phase, windowSprites.Length)];
            }
            yield return null;
        }
        timer = 0;
    }
    private void OnDisable()
    {
        _spriteRenderer.sprite = windowSprites[0];
        _coll.enabled = false;
    }
}
