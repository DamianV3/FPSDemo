using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Unity.Entities;

public enum GameState
{
    Ready,
    Starting,
    Playing,
    Over
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float delay = 2f;

    private GameState gameState;
    private int score;
    private Transform player;
    private float timeElapsed;

    private EnemySpawn[] spawnList;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        spawnList = FindObjectsOfType<EnemySpawn>();
        gameState = GameState.Ready;
    }

    void Start()
    {
        gameState = GameState.Starting;
        StartCoroutine(MainGameLoopRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Starting || gameState == GameState.Playing)
        {
            UpdateTime();
        }
    }

    private IEnumerator MainGameLoopRoutine()
    {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(EndGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        timeElapsed = 0f;

        PlayerController.EnablePlayer(true);
        UIGameController.Instance.SetFade(false, delay);

        yield return new WaitForSeconds(delay);
        gameState = GameState.Playing;
    }

    private IEnumerator PlayGameRoutine()
    {
        if (spawnList != null)
        {
            for (int i = 0; i < spawnList.Length; i++)
            {
                if(spawnList[i] != null)
                    spawnList[i].StartSpawn();
            }
        }

        while (gameState == GameState.Playing)
        {
            yield return null;
        }
    }
    private void UpdateTime()
    {
        timeElapsed += Time.deltaTime;
        
        if (UIGameController.Instance != null)
        {
            UIGameController.Instance.SetTime(timeElapsed);
        }
    }

    private IEnumerator EndGameRoutine()
    {
        // fade to black and wait
        UIGameController.Instance.SetFade(true, delay);
        yield return new WaitForSeconds(delay);
        UIGameController.Instance.SetLose(true);
        gameState = GameState.Ready;

        // restart the game
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static Vector3 GetPlayerPosition()
    {
        if (Instance == null)
        {
            return Vector3.zero;
        }

        return (Instance.player != null) ? Instance.player.position : Vector3.zero;
    }

    public static void EndGame()
    {
        if (Instance == null)
        {
            return;
        }
        PlayerController.EnablePlayer(false);
        Instance.gameState = GameState.Over;
    }
    public static bool IsGameOver()
    {
        if (Instance == null)
        {
            return false;
        }

        return (Instance.gameState == GameState.Over);
    }

    public static void AddScore(int scoreValue)
    {
        Instance.score += scoreValue;
        if (UIGameController.Instance != null)
        {
            UIGameController.Instance.SetScore(Instance.score);
        }
    }

    public static void HitEnemy(GameObject enemy, Vector3 hitpoint)
    {
        if (Instance == null)
        {
            return;
        }
        var e = EntityController.Instance.GetEnemy(enemy);
        if (e != Entity.Null)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            if (entityManager.HasComponent<EnemyData>(e))
            {
                var data = entityManager.GetComponentData<EnemyData>(e);
                data.Health -= 1;
                entityManager.SetComponentData(e, new EnemyData { Health = data.Health });
            }
        }
    }

    public static void HitPlayer()
    {
        if (Instance == null)
        {
            return;
        }
        GameManager.EndGame();
    }
}
