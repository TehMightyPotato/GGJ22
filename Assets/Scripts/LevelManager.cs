using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<GameObject> _hazardPrefabs;
    [SerializeField] private List<GameObject> _collectiblePrefabs;
    [SerializeField] private List<GameObject> _levelHazards = new List<GameObject>();
    [SerializeField] private List<GameObject> _levelCollectibles = new List<GameObject>();

    [SerializeField] private float _upperLimit;
    [SerializeField] private float _lowerLimit;
    public float xDestroyThreshold;
    [SerializeField] private int _minDifficulty;
    [SerializeField] private int _maxDifficulty;
    public int currentDifficulty;

    public UnityEvent hazardHit;
    public UnityEvent collectibleHit;
    public bool doSpawn;


    private void Start()
    {
        StartCoroutine(SpawnIntervalRoutine());
        ScoreManager.Instance.scoreChanged.AddListener((score) =>
        {
            if (score > currentDifficulty * 100 + 100)
            {
                RaiseDifficulty();
            }
        });
        hazardHit.AddListener(Pause);
    }


    [ButtonMethod]
    private void SpawnHazard()
    {
        var prefab = _hazardPrefabs.GetRandom();
        var obj = Instantiate(prefab, new Vector3(20, UnityEngine.Random.Range(_lowerLimit, _upperLimit), 0),
            quaternion.identity);
        AddHazard(obj);
    }

    private void SpawnCollectible()
    {
        var prefab = _collectiblePrefabs.GetRandom();
        var obj = Instantiate(prefab, new Vector3(20, UnityEngine.Random.Range(_lowerLimit, _upperLimit), 0),
            quaternion.identity);
        AddCollectible(obj);
    }


    public void AddHazard(GameObject obj)
    {
        _levelHazards.Add(obj);
        obj.GetComponent<LevelHazard>().Init();
    }

    public void RemoveHazard(GameObject obj)
    {
        _levelHazards.Remove(obj);
    }

    public void AddCollectible(GameObject obj)
    {
        _levelCollectibles.Add(obj);
        obj.GetComponent<Collectible>().Init();
    }

    public void RemoveCollectible(GameObject obj)
    {
        _levelCollectibles.Remove(obj);
    }

    [ButtonMethod]
    public void Pause()
    {
        doSpawn = false;
        foreach (var hazard in _levelHazards)
        {
            hazard.GetComponent<LevelHazard>().Pause();
        }

        foreach (var collectible in _levelCollectibles)
        {
            collectible.GetComponent<Collectible>().Pause();
        }
    }

    [ButtonMethod]
    public void Unpause()
    {
        doSpawn = true;
        foreach (var hazard in _levelHazards)
        {
            hazard.GetComponent<LevelHazard>().Unpause();
        }

        foreach (var collectible in _levelCollectibles)
        {
            collectible.GetComponent<Collectible>().Unpause();
        }
    }

    [ButtonMethod]
    public void RaiseDifficulty()
    {
        if (currentDifficulty < _maxDifficulty)
        {
            currentDifficulty++;
        }
    }

    [ButtonMethod]
    public void LowerDifficulty()
    {
        if (currentDifficulty > _minDifficulty)
        {
            currentDifficulty--;
        }
    }

    private IEnumerator SpawnIntervalRoutine()
    {
        while (true)
        {
            var spawnAmount = UnityEngine.Random.Range(1, currentDifficulty + 2);
            for (int i = 0; i < spawnAmount; i++)
            {
                if (!doSpawn) continue;
                SpawnHazard();
            }

            yield return new WaitForSeconds((_maxDifficulty - currentDifficulty + 1) / 2f);
            var collectibleAmount = UnityEngine.Random.Range(0, 2);
            for (int i = 0; i < collectibleAmount; i++)
            {
                if (!doSpawn) continue;
                SpawnCollectible();
            }

            yield return new WaitForSeconds((_maxDifficulty - currentDifficulty + 1) / 2f);
        }
    }
}