using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Random = UnityEngine.Random;


public class EnemySpawn : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private int spawnCount = 30;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRadius = 30f;
    

    // counter
    private float spawnTimer;
    private bool canSpawn;
    private bool isInArea;

    private EntityManager entityManager;

    private Entity enemyEntityPrefab;
    private BlobAssetStore store;
    void Init()
    {
        // get reference to current EntityManager
        //entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //store = new BlobAssetStore();

        // create entity prefab from the game object prefab, using default conversion settings
        //var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        //enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyPrefab, settings);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn || !isInArea || GameManager.IsGameOver())
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0;
        }
    }

    private void SpawnWave()
    {
        Instantiate(EnemySpawnController.getEnemyPrefab(), (Vector3)RandomPointOnCircle(spawnRadius), Quaternion.identity);
        //Entity enemy = entityManager.Instantiate(enemyEntityPrefab);

        //entityManager.SetComponentData(enemy, new Translation { Value = RandomPointOnCircle(spawnRadius) });
    }

    // get a random point on a circle with given radius
    private float3 RandomPointOnCircle(float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * radius;

        // return random point on circle, centered around the player position
        return new float3(randomPoint.x, 0.5f, randomPoint.y) + (float3)GameManager.GetPlayerPosition();
    }

    // signal from GameManager to begin spawning
    public void StartSpawn()
    {
        //if (entityManager == null)
        //    Init();
        canSpawn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInArea = false;
        }
    }
}
