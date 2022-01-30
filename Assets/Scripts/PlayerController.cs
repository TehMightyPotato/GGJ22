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
    [SerializeField] private Animator _ownAnimator;
    [SerializeField] private Animator _cloneAnimator;
    [SerializeField] private GameInput _gameInput;
    private WaitForFixedUpdate _waitForFixedUpdate;
    
    [Header("Settings")]
    [SerializeField] private Vector3 _jumpVector;
    [SerializeField] private float _jumpMagnitude;
    [SerializeField] private ForceMode _jumpForceMode;

    [Header("Runtime")]
    [SerializeField] private Side _currentSide;

    private static readonly int JumpAnimatorIndex = Animator.StringToHash("Jump");
    private static readonly int ActivateAnimatorIndex = Animator.StringToHash("Activate");
    private static readonly int DeactivateAnimatorIndex = Animator.StringToHash("Deactivate");
    private static readonly int DieAnimatorIndex = Animator.StringToHash("Die");

    private void Awake()
    {
        _waitForFixedUpdate = new WaitForFixedUpdate();
        LevelManager.Instance.hazardHit.AddListener(() =>
        {
            _gameInput.Disable();
            _ownRigidBody.isKinematic = true;
            _ownAnimator.SetTrigger(DieAnimatorIndex);
            _cloneAnimator.SetTrigger(DieAnimatorIndex);
        });
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
        _ownAnimator.SetTrigger(JumpAnimatorIndex);
        _cloneAnimator.SetTrigger(JumpAnimatorIndex);
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
                transform.eulerAngles = new Vector3(0, 0, 180);
                _currentSide = Side.Down;
                break;
            case Side.Down:
                transform.eulerAngles = new Vector3(0, 0, 0);
                Physics.gravity = new Vector3(0,-9.81f);
                _currentSide = Side.Up;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _ownAnimator.SetTrigger(ActivateAnimatorIndex);
        _cloneAnimator.SetTrigger(DeactivateAnimatorIndex);
        _ownRigidBody.position = -_ownRigidBody.position;
        _ownRigidBody.velocity = -_ownRigidBody.velocity;
    }

}

public enum Side
{
    Up,
    Down
} 