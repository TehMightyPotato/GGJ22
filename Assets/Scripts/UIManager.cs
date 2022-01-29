using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _restartButton;
    
    private void OnEnable()
    {
        ScoreManager.Instance.scoreChanged.AddListener(UpdateScoreText);
        LevelManager.Instance.hazardHit.AddListener(()=>{_restartButton.SetActive(true);});
    }
    

    private void UpdateScoreText(int currentScore)
    {
        _scoreText.text = "Score: " + currentScore;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}