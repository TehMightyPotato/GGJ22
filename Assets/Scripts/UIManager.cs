using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _deathMenu;
    
    private void OnEnable()
    {
        ScoreManager.Instance.scoreChanged.AddListener(UpdateScoreText);
        LevelManager.Instance.hazardHit.AddListener(()=>{_deathMenu.SetActive(true);});
    }
    

    private void UpdateScoreText(int currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}