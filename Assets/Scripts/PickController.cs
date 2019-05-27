using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController : MonoBehaviour, IPlayerComponent {

    public Player Player { get; set; }

	// Use this for initialization
	void Start () {
        Player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                if (hit.collider.gameObject.GetComponent<IPickable>() != null)
                {
                    IPickable hitted = hit.collider.gameObject.GetComponent<IPickable>();
                    hitted.Picked(Player);
                }
        }
	}

    public IPlayer GetPlayer()
    {
        return Player;
    }
}
