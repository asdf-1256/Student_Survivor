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
                    // Fire();
                    FireDDabal();
                }
                break;
        }

        // .. Test Code..
        /*
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
        */
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

        //���߿� �߰��� ���⿡�� ������ ����ǵ���
        //�� �Լ��� �����ִ� �ֵ� �� �����ض� ���
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
                //�� �θ�:Ǯ �Ŵ��� -> �÷��̾�� �ٲ��� �� �ֵ��� Ʈ������ �����
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            // -1�� ������ �����Ű�ڴٴ� ��
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;//���� ���ϱ�

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

        bullet.position = transform.position;//��ġ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//ȸ������
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }

    void FireDDabal()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * 2; // r=2�� �� ���� ������ ��ġ
        Vector3 ddabalRate = new Vector3(randomCircle.x, randomCircle.y, 0); // vector3�� ��ȯ

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position + ddabalRate; // �÷��̾�->�� ���Ϳ� ���߷� ÷��
        dir = dir.normalized;//���� ���ϱ�

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

        bullet.position = transform.position;//��ġ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);//ȸ������
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
