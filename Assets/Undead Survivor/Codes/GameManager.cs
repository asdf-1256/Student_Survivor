using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static - 정적, 메모리에 얹겠다
    //inspector에 나타나지 않음
    public static GameManager Instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    //public float maxGameTime = 2 * 10f;
    public readonly float[] semesterDifficulty = { 1, 1.2f, 1.4f, 1.6f, 1.8f, 2f, 3f, 3.5f }; //난이도 상수

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public readonly int maxLevel = 100;
    public int kill;
    public Dictionary<int, int> killByType;
    public int exp;
    public float manBoGi;
    public float expRate = 1.0f;

    [Header("# Game Info")]
    public int[] levelPerPhase; // 페이즈 당 필요레벨
    public int currentPhase; //현재 난이도
    public int[] nextExp;
    public int[] levelForBoss;
    //public int money;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public AI_Player ai_Player;
    public LevelUp uiLevelUp;
    public LevelUpSkill uiLevelUpSkill; // 임시 추가
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;
    public GameObject QuestBox;
    public GameObject bossSet;
    public GameObject HealthInHUD;

    public int MaxQuestCount = 3;
    public List<UIQuest> freeQuestUI;

    int questCount = 0;

    private IEnumerator currentBossSpawn;
    private Enemy _boss;
    public Enemy SpawnedBoss
    {
        get { return _boss; }
        set { _boss = value; }
    }
    public bool CanAddQuest() {
        if (questCount < MaxQuestCount) return true;
        return false;
    }

    public UIQuest AddQuest(string skillName, int level, QuestChecker checker, QuestData data) {
        UIQuest questUI = freeQuestUI[0];
        freeQuestUI.RemoveAt(0);
        questCount++;

        questUI.QuestSet(skillName, level, checker, data);

        return questUI;
    }

    public void EndQuest(UIQuest endQuestUI) {
        endQuestUI.gameObject.SetActive(false);
        freeQuestUI.Add(endQuestUI);
        questCount--;

    }

    private void Awake()
    {
        Instance = this;
        killByType = new();
        for(int i = 0; i < 3; i++)
            killByType.Add(i, 0);
        Application.targetFrameRate = 60;
    }
    public void GameStart(int id)
    {
        playerId = id;

        playerId = DataManager.Instance.GetSelectedSkinID();

        health = maxHealth;

        player.gameObject.SetActive(true);

        //첫번째 캐릭터 선택
        // uiLevelUp.Select(playerId % 2); // 캐릭터 선택하면 무기 지급했던거 주석처리 함
        Resume();

        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);

        currentBossSpawn = SpawnBoss();
        StoreManager.instance.ApplyAllDongAri();
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry()
    {
        DataManager.Instance.Save();
        SceneManager.LoadScene(0); // build setting에 scene 번호
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;
        gameTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetExp(10);
        }
    }
    public void GetExp() //.. 1만큼 증가하는 경험치 획득 함수
    {
        GetExp(1);
    }
    public void GetExp(int e) //... e만큼 증가하는 경험치 획득 함수
    {
        if (!isLive)
            return;
        if(expRate != 1.0f) //경험치 증가 교양스킬이 추가되면 이 부분이 실행됨. - 실수값을 곱해서 반올림한 정수형
            e = Convert.ToInt32(expRate * e);
        exp += e;
        int nextexp = nextExp[Mathf.Min(level, nextExp.Length - 1)]; //index bound error 안 나오도록
        if (exp >= nextexp)
        {
            level++;
            exp -= nextexp;
            uiLevelUpSkill.Show();
            UpdatePhase();
            if (level == levelForBoss[0] || level == levelForBoss[1])
            {
                Debug.Log("보스 소환");
                currentBossSpawn.MoveNext();
            }
            SkillRateManager.instance.updateSkillRate(currentPhase);
        }
    }
    void UpdatePhase()
    {
        int requestLevel = levelPerPhase[currentPhase];
        if (level >= requestLevel)
        {
            currentPhase++;
        }
    }
    public float getRateForNextPhase()
    {
        float curLevel, goalLevel;
        if (currentPhase == 0)
        {
            curLevel = level;
            goalLevel = levelPerPhase[currentPhase];
        }
        else
        {
            curLevel = level - levelPerPhase[currentPhase - 1];
            goalLevel = levelPerPhase[currentPhase] - levelPerPhase[currentPhase - 1];
        }
        return curLevel / goalLevel;
    }
    public void GetHealth(int h) //.. h만큼 체력 회복
    {
        if (!isLive)
            return;
        health = Mathf.Min(maxHealth, health + h);
    }

    public void AddManBogi(float distance)
    {
        manBoGi += distance;
    }

    //각 스크립트의 Update 계열 로직에 isLive 조건 추가
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; //유니티의 시간 속도 배율
        uiJoy.localScale = Vector3.zero;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; //만약 2면 시간이 그만큼 빨리 흘러감.
        uiJoy.localScale = Vector3.one;
    }
    private IEnumerator SpawnBoss()
    {
        int currentBoss = 0;

        while ( currentBoss != bossSet.transform.childCount - 1 ) {

            StartCoroutine(BossSpawnEffector( () => {
                Transform nextBoss = bossSet.transform.GetChild(currentBoss);
                _boss = nextBoss.GetComponent<Enemy>();
                nextBoss.localPosition = player.transform.position + Vector3.up * 10;
                nextBoss.gameObject.SetActive(true);
                Debug.Log(string.Format("보스 소환 {0}번째", currentBoss));
                currentBoss++;
            } ));
            yield return null;
            /*
            Transform nextBoss = bossSet.transform.GetChild(currentBoss);
            _boss = nextBoss.GetComponent<Enemy>();
            nextBoss.localPosition = player.transform.position + Vector3.up * 10;
            nextBoss.gameObject.SetActive(true);
            Debug.Log(string.Format("보스 소환 {0}번째",currentBoss));
            currentBoss++;
            yield return null;*/
        }
    }
    private IEnumerator BossSpawnEffector(System.Action done)
    {
        //effect에 관한 코드를 추가해야함.

        yield return new WaitUntil( () => { return Time.timeScale == 1; });

        done.Invoke();
    }
}
