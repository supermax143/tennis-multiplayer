using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{

    public event Action<Player> OnBallCollision;
    
    private Player _player;

    public Player Player => _player;


    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ball"))
        {
            return;
        }
        OnBallCollision?.Invoke(Player);
    }

}
