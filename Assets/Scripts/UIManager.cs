using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;


    private void OnEnable()
    {
        ScoreManager.Instance.scoreChanged.AddListener(UpdateScoreText);
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.scoreChanged.RemoveListener(UpdateScoreText);
        }
    }

    private void UpdateScoreText(int currentScore)
    {
        _scoreText.text = "Score: " + currentScore;
    }
}