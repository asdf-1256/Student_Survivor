using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;
    private void Awake()
    {
        player = GameManager.Instance.player;
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // .. Test Code..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
        {
            Arrange();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        // Basic Setting
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Setting
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++) 
        {
            if (data.projectile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Arrange();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        //나중에 추가된 무기에도 버프가 적용되도록
        //이 함수를 갖고있는 애들 다 실행해라 방송
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    void Arrange()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //현 부모:풀 매니저 -> 플레이어로 바꿔줄 수 있도록 트랜스폼 갖고옴
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            // -1은 무한히 관통시키겠다는 뜻
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;//방향 구하기

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

        bullet.position = transform.position;//위치결정
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//회전결정
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }

}
