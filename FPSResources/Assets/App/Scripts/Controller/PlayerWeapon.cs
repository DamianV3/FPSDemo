using UnityEngine;
using Unity.Entities;
using Unity.Transforms;


/// <summary>
/// Handles player shooting
/// </summary>
public class PlayerWeapon : MonoBehaviour
{

    [Header("Specs")]

    // time between shots
    [SerializeField] private float rateOfFire = 0.15f;

    // where the weapon's bullet appears
    [SerializeField] private Transform muzzleTransform;

    // GameObject prefab 
    [SerializeField] private GameObject bulletPrefab;

    [Header("Effects")]
    [SerializeField] private AudioSource soundFXSource;

    [SerializeField] LayerMask shotLayer;

    private EntityManager entityManager;

    private Entity bulletEntityPrefab;
    private BlobAssetStore store;

    private float shotTimer;

    private bool isFireButtonDown;
    public bool IsFireButtonDown { get { return isFireButtonDown; } set { isFireButtonDown = value; } }

    protected virtual void Start()
    {
        // get reference to current EntityManager
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        store = new BlobAssetStore();
        // create entity prefab from the game object prefab, using default conversion settings
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
    }

    private void OnDestroy()
    {
        store.Dispose();
    }

    public virtual void FireBullet()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hit, 1000f, shotLayer);

        Entity bullet = entityManager.Instantiate(bulletEntityPrefab);

        
        entityManager.SetComponentData(bullet, new Translation { Value = muzzleTransform.position });
        entityManager.SetComponentData(bullet, new Rotation { Value = muzzleTransform.rotation });
        if (isHit)
        {
            if (hit.transform.tag == "Enemy")
            {
                GameManager.HitEnemy(hit.transform.gameObject, hit.point);

            }
            entityManager.AddComponent(bullet, typeof(DestroyPointComponent));
            entityManager.SetComponentData(bullet, new DestroyPointComponent { Value = hit.point });
        }            
        soundFXSource?.Play();
    }

    protected virtual void Update()
    {
        // ignore if the player is dead
        if (GameManager.IsGameOver())
        {
            return;
        }

        shotTimer += Time.deltaTime;
        if (shotTimer >= rateOfFire && isFireButtonDown)
        {
            FireBullet();
            shotTimer = 0f;
        }
    }
}
