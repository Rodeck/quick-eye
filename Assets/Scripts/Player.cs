using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer {

    public int Points { get; private set; }

    private IPlayerUI _playerUI;

	// Use this for initialization
	void Start () {
        Points = 0;
        _playerUI = FindObjectOfType<PlayerUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ObjectPicked(Pickable picked)
    {
        AddPoint(picked.GetPoints());
    }

    public void FaultyObjectPicked(Pickable picked)
    {
        AddPoint(-1 * picked.GetPoints());
    }

    public void ReturnItem(Pickable picked)
    {
        picked.GetSpawner().ReturnItem(picked);
    }

    private void AddPoint(int amount)
    {
        ChangePointsAmount(Points + amount);
    }

    private void ChangePointsAmount(int finalValue)
    {
        Points = finalValue;
        _playerUI.RefreshUI(this, true);
    }


    public string GetPoints()
    {
        return Points.ToString();
    }
}
