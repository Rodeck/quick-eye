using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MyPool : MonoBehaviour, IPool {


    public GameObject _toInit;

    [Range(1, 1000)]
    public int NumberOfItems = 100;

    private List<GameObject> _pickers = new List<GameObject>();

	// Use this for initialization
	void Start () {
        Init();
	}
	
	public void Init()
    {
        for (int i = 0; i < NumberOfItems; i++)
		{
            GameObject go = Instantiate<GameObject>(_toInit); 
            go.transform.SetParent(this.transform);
            go.transform.position = Vector3.zero;
            go.SetActive(false);
            _pickers.Add(go);
		}
    }

    public GameObject GetItemToPick()
    {
        return _pickers.Where(s => !s.activeInHierarchy).FirstOrDefault();
    }

    public void ReturnToPool(GameObject item)
    {
        item.SetActive(false);
        item.transform.position = Vector3.zero;

        if(item.GetComponent<Rigidbody>())
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void ReturnToPool(Pickable item)
    {
        item.gameObject.SetActive(false);
    }


    public ISpawner GetSpawner(Pickable item)
    {
        return item.GetSpawner();
    }
}

public interface IPool
{
    void ReturnToPool(Pickable item);
    ISpawner GetSpawner(Pickable item);
}
