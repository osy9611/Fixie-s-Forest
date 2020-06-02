using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Collider2D col;
    Player player;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.GostMode==true )
            {
                col.isTrigger = true;
            }
            if(player.rigidbody.simulated==false)
            {
                col.isTrigger = true;
            }
        }
    }
}
