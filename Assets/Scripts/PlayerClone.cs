using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private VisualEffect _cloneTrail;
    [SerializeField] private Transform _visualTransform;

    void Update()
    {
        _cloneTrail.SetVector3("TrailSpawnPosition", transform.position);
        transform.position = -_player.transform.position;
        _visualTransform.eulerAngles = _playerVisual.rotation.eulerAngles - new Vector3(0,0,180);
    }
}
