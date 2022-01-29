using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int score;

    [SerializeField] private float _moveSpeed;

    private Vector3 _targetPosition;
    private bool _initialized;
    private bool _paused = false;

    public void Init()
    {
        _targetPosition = new Vector3(-30, transform.position.y, 0);
        _initialized = true;
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
        if (!(transform.position.x <= LevelManager.Instance.xDestroyThreshold)) return;
        LevelManager.Instance.RemoveCollectible(gameObject);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddToScore(score);
            LevelManager.Instance.collectibleHit?.Invoke();
            LevelManager.Instance.RemoveCollectible(gameObject);
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Unpause()
    {
        _paused = false;
    }
}
