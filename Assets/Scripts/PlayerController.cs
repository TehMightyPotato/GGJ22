using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _ownRigidBody;
    [SerializeField] private GameInput _gameInput;
    private WaitForFixedUpdate _waitForFixedUpdate;
    
    [Header("Settings")]
    [SerializeField] private Vector3 _jumpVector;
    [SerializeField] private float _jumpMagnitude;
    [SerializeField] private ForceMode _jumpForceMode;
    [SerializeField] private float _maxForceMagnitude;
    [SerializeField] private Vector3 _upperPosition;
    [SerializeField] private Vector3 _lowerPosition;

    [Header("Runtime")]
    [SerializeField] private Side _currentSide;

    [Header("Events")] 
    public UnityEvent onJumpStarted;
    public UnityEvent onJumpEnded;

    private void Awake()
    {
        _waitForFixedUpdate = new WaitForFixedUpdate();
        _currentSide = Side.Up;
    }

    private void OnEnable()
    {
        _gameInput = new GameInput();
        _gameInput.Gameplay.Jump.performed += (InputAction.CallbackContext ctx) => { StartCoroutine(Jump(ctx));};
        _gameInput.Gameplay.Switch.performed += ChangeSides;
        _gameInput.Enable();
    }

    private IEnumerator Jump(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed) yield break;
        yield return _waitForFixedUpdate;
        var vector = _jumpVector * _jumpMagnitude;
        if (_currentSide == Side.Down)
        {
            vector *= -1;
        }
        _ownRigidBody.AddForce(vector, _jumpForceMode);
    }

    private void ChangeSides(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        switch (_currentSide)
        {
            case Side.Up:
                Physics.gravity = new Vector3(0,9.81f);
                _currentSide = Side.Down;
                break;
            case Side.Down:
                Physics.gravity = new Vector3(0,-9.81f);
                _currentSide = Side.Up;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _ownRigidBody.position = -_ownRigidBody.position;
        _ownRigidBody.velocity = -_ownRigidBody.velocity;
    }

}


public enum Side
{
    Up,
    Down
} 