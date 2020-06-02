using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour {

    Player player;
    public Tutorial tuto;
    public GameObject Button;
    public GameObject Button_Image;
    public UIManager UI;
    public float count;
    float Speed;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player") 
        {
            count++;
            if (count == 1)
            {
                if (player.Speed != player.MaxSpeed)
                {
                    player.SaveSpeed = player.Speed;    
                }
                player.Speed = 0;
                player.ani.speed = 0;
                player.rigidbody.simulated = false;
                player.tutorial.SetActive(true);
                tuto.ShowText();
                tuto.Panel.SetActive(true);
                if (Button != null)
                {
                    Button.SetActive(true);
                    if (Button_Image != null)
                    {
                        Button_Image.SetActive(true);
                    }
                }
            }
            else if(count > 1 )
            {
                player.PS = PlayerState.Run;
                if (player.Speed != 0)
                {
                    if (Button_Image != null)
                    {
                        Button_Image.SetActive(false);
                    }
                }
            }
        }
        
    }
    
}
