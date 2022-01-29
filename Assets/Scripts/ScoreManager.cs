using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _score;

    public int Score
    {
        get => _score;
        private set
        {
            scoreChanged?.Invoke(Score);
            _score = value;
        }
    }

    public UnityEvent<int> scoreChanged;

    private void Start()
    {
        Score = 0;
        StartCoroutine(ScoreOverTimeRoutine());
    }

    public void AddToScore(int amount)
    {
        Score += amount;
    }


    private IEnumerator ScoreOverTimeRoutine()
    {
        while (true)
        {
            AddToScore(LevelManager.Instance.currentDifficulty + 1);
            yield return new WaitForSeconds(0.5f);
        }
    }
}