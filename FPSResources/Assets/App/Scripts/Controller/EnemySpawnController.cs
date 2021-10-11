using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class EnemySpawnController : Singleton<EnemySpawnController>
{
    [SerializeField] private string enemyURL;
    private GameObject enemyPrefab;

    private void Awake()
    {
        StartCoroutine(loadPrefab());
    }

    IEnumerator loadPrefab()
    {
        var request = Resources.LoadAsync(enemyURL);
        yield return request;
        enemyPrefab = request.asset as GameObject;
    }

    public static GameObject getEnemyPrefab()
    {
        if (Instance == null) return null;
        return Instance.enemyPrefab;
    }
}
