using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour, ISpawner {

    public GameObject ItemToSpawn;
    public SpawnersController controller { get; set; }

    public float LastTimeSpawned { get; private set; }

    public Spawned Spawn(MyPool _pool)
    {
        if (_pool == null)
            _pool = FindObjectOfType<MyPool>();

        GameObject item = _pool.GetItemToPick();

        if (item != null)
        {
            item.transform.position = this.transform.position;
            Spawned spawnedItem = new Spawned(item, Time.realtimeSinceStartup);
            item.GetComponent<IPickable>().SetSpawner(this);
            item.SetActive(true);
            LastTimeSpawned = Time.timeSinceLevelLoad;
            return spawnedItem;
        }
        else
        {
            Debug.Log("Could not get item from pool!");
            return null;
        }
    }

    

    public void ReturnItem(Pickable toReturn)
    {
        controller.ReturnItem(toReturn);
    }
}

public class Spawned
{
    public GameObject @object;
    public float spawnTime;

    public Spawned(GameObject go, float _spawnTime)
    {
        @object = go;
        spawnTime = _spawnTime;
    }
}

public interface ISpawner
{
    void ReturnItem(Pickable toReturn);
}
