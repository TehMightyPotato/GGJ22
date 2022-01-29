using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int score;
    private LevelManager _levelManager;

    [SerializeField] private float _moveSpeed;

    private Vector3 _targetPosition;
    private bool _initialized;
    private bool _paused = false;

    public void Init(LevelManager manager)
    {
        _levelManager = manager;
    }
    
    private void Update()
    {
        if (!_initialized || _paused) return;
        float step = _moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
    }

    private void FixedUpdate()
    {
        if (!_initialized || _paused) return;
        if (!(transform.position.x <= _levelManager.xDestroyThreshold)) return;
        _levelManager.RemoveCollectible(gameObject);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddToScore(score);
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        
    }

    public void Unpause()
    {
        
    }
}
