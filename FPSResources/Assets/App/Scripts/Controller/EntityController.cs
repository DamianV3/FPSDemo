using UnityEngine;
using Unity.Entities;
using Core;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;


public class ECSObject {
    public GameObject obj;
    public Entity entity; 
}

// Sync Position Entity 
// Because anim not working on this version
public class EntityController : Singleton<EntityController>
{
    public ECSObject player = new ECSObject();
    public List<ECSObject> enemies = new List<ECSObject>();
    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void AddEnemy(Entity enemy, GameObject _obj)
    {
        var item = enemies.Find((o) => o.obj == _obj);
        if(item == null)
            enemies.Add(new ECSObject() { obj = _obj, entity = enemy});
    }

    public void RemoveEnemy(Entity enemy)
    {
        var item = enemies.Find((o) => o.entity == enemy );
        if (item != null)
        {
            var target = item.obj;
            enemies.Remove(item);
            Destroy(target, 0.5f);
        }
    }

    public Entity GetEnemy(GameObject enemy)
    {
        var item = enemies.Find((o) => o.obj == enemy);
        if (item != null) return item.entity;
        return Entity.Null;
    }

    private void Update()
    {
        //!Sync Position
        foreach(var item in enemies)
        {
            if (entityManager.HasComponent<Translation>(item.entity))
            {
                var Trans = entityManager.GetComponentData<Translation>(item.entity);
                Trans.Value = item.obj.transform.position;
                entityManager.SetComponentData(item.entity, new Translation { Value = item.obj.transform.position });
            }
        }
    }
}
