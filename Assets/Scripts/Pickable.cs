using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable {

    public IPool Pool { get; private set; }
    public ISpawner Spawner { get; private set; }

    [SerializeField]
    private GameObject _faultyEffect;
    [SerializeField]
    private GameObject _properEffect;

    private bool _faultyApple = false;
    private bool _canPick = true;

    public void Picked(IPlayer by)
    {
        if (_canPick)
        {
            if (!_faultyApple)
            {
                by.ObjectPicked(this);
                this.GetComponent<MeshRenderer>().enabled = false;
                _canPick = false;
                PlayEffect(_properEffect);
            }
            else
            {
                by.FaultyObjectPicked(this);
                this.GetComponent<MeshRenderer>().enabled = false;
                _canPick = false;
                PlayEffect(_faultyEffect);
            }

            StartCoroutine(Return(by));
        }
    }

    private void PlayEffect(GameObject system)
    {
        GameObject effect = Instantiate(system);
        effect.transform.position = this.transform.position;
        effect.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator Return(IPlayer player)
    {
        yield return new WaitForSeconds(1.0f);
        player.ReturnItem(this);
    }

    public int GetPoints()
    {
        return 1;
    }

    public void SetPool(IPool pool)
    {
        Pool = pool;
    }

    public void SetSpawner(ISpawner spawner)
    {
        Spawner = spawner;
    }


    public IPool GetPool()
    {
        return Pool;
    }

    public ISpawner GetSpawner()
    {
        return Spawner;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Island")
        {
            _faultyApple = true;
        }
    }
}

public interface IPickable
{
    void Picked(IPlayer by);
    int GetPoints();
    void SetPool(IPool pool);
    void SetSpawner(ISpawner spawner);
    IPool GetPool();
    ISpawner GetSpawner();  
}
