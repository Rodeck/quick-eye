using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnersController : MonoBehaviour {

    private List<Spawner> _spawners = new List<Spawner>();

    [Range(0.0f, 1.0f)]
    public float SpawnRate = .5f;

    [Range(0.1f, 3.0f)]
    public float MinSpawnDelay = 0.5f;

    [Range(1, 1000)]
    public int MaxItems = 50;

    private List<Spawned> _active = new List<Spawned>();
    private MyPool _pool;

    [SerializeField]
    private SpawnState _spawnState = SpawnState.Pre;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _timespanJump = 0.14f;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _minTimespan = 0.2f;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _timeSpanDecreaseSpeed = 0.2f;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _timeSpanIncreaseSpeed = 0.05f;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _timeRestorationDelay = 1.0f;

    [SerializeField]
    [Range(1, 100)]
    private int _NumberOfItemsToSpawn = 10;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _totalSpawnTime = 0.2f;

    private List<float> _times;
    private GameStateController _gameController;

	// Use this for initialization
	void Start () 
    {
        _spawners = FindObjectsOfType<Spawner>().ToList();
        _spawners.ForEach(s => s.controller = this);
        _pool = FindObjectOfType<MyPool>();
        _gameController = FindObjectOfType<GameStateController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.G) && (_spawnState == SpawnState.Pre || _spawnState == SpawnState.Past))
            Spawn(_NumberOfItemsToSpawn, _totalSpawnTime);
	}

    public void Spawn(int totalNumber, float spawnTime)
    {
        _spawnState = SpawnState.Spawning;

        _times = CreateQuantums(spawnTime, totalNumber);

        StartCoroutine(SpawnItems());
    }

    private List<float> CreateQuantums(float _totalSpawnTime, int _totalNumber)
    {
        float[] array = new float[_totalNumber];

        for (int i = 0; i < _totalNumber; i++)
        {
            array[i] = 0;
        }

        for (int i = 0; i < _totalNumber - 1; i++)
        {
            float maxValue = _totalSpawnTime - array.Sum();
            array[i] = Random.Range(0.0f, maxValue);
        }

        array[_totalNumber - 1] = _totalSpawnTime - array.Sum();

        var newArray = array.OrderBy(x => x).ToList();

        string arrayString = string.Join(",", newArray.Select(x => x.ToString()).ToArray());

        Debug.Log(string.Format("Total time: {0}; Array: [{1}]", array.Sum(), arrayString));

        return newArray;
    }

    IEnumerator SpawnItems()
    {
        for (int i = 0; i < _times.Count; i++)
        {
            //Spawn item
            Spawner spawner = _spawners.OrderBy(x => x.LastTimeSpawned).FirstOrDefault();
            _active.Add(spawner.Spawn(_pool));

            //Wait for time
            yield return new WaitForSeconds(_times[i]);
        }

        EndSpawning();
    }

    private void EndSpawning()
    {
        _spawnState = SpawnState.SlowMotion;
        _gameController.SpawningFinished();
        BeginSlowMotion();
    }

    private void BeginSlowMotion()
    {
        StartCoroutine(DecreaseTimeSpan());
    }

    IEnumerator DecreaseTimeSpan()
    {
        while(Time.timeScale >= _minTimespan)
        {
            Time.timeScale -= _timespanJump;
            yield return new WaitForSeconds(Random.Range(0.0f, _timeSpanDecreaseSpeed));
        }

        yield return new WaitForSeconds(_timeRestorationDelay);

        StartCoroutine(IncereaseTimeSpan());
    }

    IEnumerator IncereaseTimeSpan()
    {
        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += _timespanJump;
            yield return new WaitForSeconds(Random.Range(0.0f, _timeSpanIncreaseSpeed));
        }

        if (Time.timeScale > 1.0f)
            Time.timeScale = 1.0f;

        Finish();
    }

    private void Finish()
    {
        _spawnState = SpawnState.Past;
    }

    IEnumerator ClearActive()
    {
        while (true)
        {
            if (_active.Count >= MaxItems)
            {
                int toDisable = _active.Count - MaxItems;

                List<Spawned> copy = _active.OrderByDescending((x => x.spawnTime)).ToList().Take(toDisable).ToList();

                foreach (Spawned item in copy)
                {
                    _active.Remove(item);
                    _pool.ReturnToPool(item.@object);
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void ReturnItem(Pickable toReturn)
    {
        Spawned spawned = _active.Where(s => s.@object = toReturn.gameObject).FirstOrDefault();

        if (spawned == null)
        {
            Debug.Log("Could not find item!");
            return;
        }

        _active.Remove(spawned);
        spawned.@object.SetActive(false);
    }
}

public enum SpawnState
{
    Pre,
    Spawning,
    SlowMotion,
    Past
}
