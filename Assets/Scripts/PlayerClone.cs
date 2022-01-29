using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    void Update()
    {
        transform.position = -_player.transform.position;
    }
}
