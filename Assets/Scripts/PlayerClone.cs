using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    void Update()
    {
        var ownTransform = transform;
        ownTransform.position = -_player.transform.position;
        ownTransform.eulerAngles = -_player.transform.rotation.eulerAngles;
    }
}
