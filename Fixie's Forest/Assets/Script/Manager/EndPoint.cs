using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndPoint : MonoBehaviour {

    public string ClearItem;
    public UIManager UI;
    public GameManager Manager;
    public Player player;
    public StoreData data;
    Sprite sprite;

 
    public void CheckClear()
    {
        if (UI.ClearItem[0].sprite != null)
        {
            for (int i = 0; i < UI.ClearItem.Count; i++)
            {
                if (UI.ClearItem[i].sprite.name == ClearItem)
                {
                    Manager.Clear = true;
                    sprite = UI.ClearItem[i].sprite;
                    break;
                }
                else
                {
                    Manager.Clear = false;
                }
            }
        }

        if(Manager.Clear==true)
        {
            UI.GameClear.SetActive(true);
            UI.theAudio.OnePlay("Clear");
            data.ItemSprite.Add(sprite);
            if(SceneManager.GetActiveScene().name != "Tutorial")
            {
                if(SceneManager.GetActiveScene().name == "Stage1" && data.Stage1==false)
                {
                    data.Stage1 = true;
                    data.MaxItemCount++;
                }
                else if(SceneManager.GetActiveScene().name == "Stage2" && data.Stage2==false)
                {
                    data.Stage2 = true;
                    data.MaxItemCount +=2;
                }
                else if(SceneManager.GetActiveScene().name == "Stage3" && data.Stage3==false)
                {
                    data.Stage3 = true;
                }
            }
        }
        else
        {
            UI.GameOver.SetActive(true);
            UI.theAudio.Play("Fail");
        }
     
    }

    IEnumerator Fade()
    {
        player.ani.speed = 0;
        player.Speed = 0;
        player.rigidbody.simulated = false;
        Color color = player.render.color;
        while(player.render.color.a>0.0f)
        {
            color.a -= Time.fixedDeltaTime * 1.0f;
            player.render.color = color;
            if(color.a<=0)
            {
                color.a = 0;
                CheckClear();
                player.Speed = 0;
            }
            yield return null;
        }
        player.render.color = color;
    }

    public void Next()
    {
        if(SceneManager.GetActiveScene().name=="Stage1" && Manager.Clear==true)
        {
            SceneManager.LoadScene("Stage1Clear");
        }
        else if (SceneManager.GetActiveScene().name == "Stage2" && Manager.Clear == true)
        {
            SceneManager.LoadScene("Stage2Clear");
        }
        else if (SceneManager.GetActiveScene().name == "Stage3" && Manager.Clear == true)
        {
            SceneManager.LoadScene("EndingCutScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fade());
        }
    }

    private void Start()
    {
        if (GameObject.Find("Data") != null)
        {
            data = GameObject.Find("Data").GetComponent<StoreData>();
        }
        //data.MaxItemCount = player.MaxItemCount;
    }
}
