using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameStateController : MonoBehaviour {

    UIController uiController;

    [SerializeField]
    GameState _GameState;

    SpawnersController _spawners;

	// Use this for initialization
	void Start () {
        _GameState = global::GameState.Preparations;
        uiController = FindObjectOfType<UIController>();
        _spawners = FindObjectOfType<SpawnersController>();
        _GameState = global::GameState.PreInfo;
        StartCoroutine(PreInfo());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PreInfo()
    {
        //GetReady
        yield return new WaitForSeconds(1.0f);
        EasyTween getReady = GetAnimation("GetReady");
        getReady.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.5f);
        getReady.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(0.3f);
        EasyTween countdown3 = GetAnimation("Countdown3");
        countdown3.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.0f);
        EasyTween countdown2 = GetAnimation("Countdown2");
        countdown2.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.0f);
        EasyTween countdown1 = GetAnimation("Countdown1");
        countdown1.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.0f);
        EasyTween countdown0 = GetAnimation("Countdown0");
        countdown0.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.0f);
        EasyTween go = GetAnimation("Go");
        go.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1.0f);
        Spawning();
    }

    private EasyTween GetAnimation(string componentName)
    {
        return FindObjectsOfType<UIComponent>()
            .Where(x => x.GetName().Equals(componentName))
            .FirstOrDefault()
            .gameObject
            .GetComponent<EasyTween>();
    }

    void Spawning()
    {
        _spawners.Spawn(10, 0.2f);
    }

    public void SpawningFinished()
    {
        _GameState = GameState.PickingTime;
    }

    public void StartAnim()
    {
        Debug.Log("Starting animation");
    }
}

public enum GameState
{
    Preparations,
    Tutorial,
    PreInfo,
    Spawning,
    PickingTime
}
