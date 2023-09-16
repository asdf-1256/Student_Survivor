using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //static - 정적, 메모리에 얹겠다
    //inspector에 나타나지 않음
    public static GameManager Instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    public PoolManager pool;
    public Player player;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

    }
}
