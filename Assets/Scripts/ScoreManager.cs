using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _score;
    private bool gameOver = false;

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
        LevelManager.Instance.hazardHit.AddListener(() => { gameOver = true; });
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
            if (gameOver) yield break;
            AddToScore(LevelManager.Instance.currentDifficulty + 1);
            yield return new WaitForSeconds(0.5f);
        }
    }
}