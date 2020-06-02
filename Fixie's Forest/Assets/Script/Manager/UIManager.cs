using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject player;
    public SpriteRenderer PlayerRender;
    public Player PlayerStatus;
    Rigidbody2D PlayerPhysics;
    public float JumpPower;
    public List<Image> ItemImage;

    //조합관련UI
    bool MixButtonWait; //버튼 활성화
    public float MixWaitTime;  //버튼이 눌러져있는 대기 시간
    public float MixWaitTimer;  //몇 초동안 눌러져 있을 것인가
    public GameObject MixWindow; //조합 창 오브젝트
    public ItemMix itemMix;     //조합체크를 위한 함수
    public Image MixComplete;   //조합이 완료된 이미지를 가지고있음
    public List<Image> CanMix;  //조합이 가능한 아이템들
    public List<Image> ClearItem; // 클리어 조건들이 있는 아이템들
    public List<Sprite> OnItem;
    public List<Sprite> OffItem;
    public float MixButtonCount;                  // 버튼 중복방지 함수
    
    //게임 클리어 및 실패 여부 관련 UI
    public GameObject GameClear;
    public GameObject GameOver;

    //인게임 UI
    public GameObject InGameMenu;
    public GameManager Manager;

    //아이템 사용가능한지 여부
    public bool SkillOn;

    public AudioManager theAudio;
    // Use this for initialization
    void Awake () {
        PlayerStatus = player.GetComponent<Player>();
        PlayerPhysics = player.GetComponent<Rigidbody2D>();
        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void SetItem()
    {
        for (int i = 0; i < PlayerStatus.ItemSprite.Count; i++)
        {
            if (ItemImage[i].sprite == null)
            {
                ItemImage[i].sprite = PlayerStatus.ItemSprite[i];
                ItemImage[i].color = new Color(1, 1, 1, 1);
            }
        }
    
        for (int i = 0; i < ItemImage.Count; i++)
        {
            if (ItemImage[i].sprite == null)
            {
                ItemImage[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetBlockItem()
    {
        if (PlayerStatus.ani.speed != 0)
        {
            for (int i = 0; i < PlayerStatus.ItemSprite.Count; i++)
            {
                if (PlayerStatus.ItemSprite[i].name == "BlockItem")
                {
                    ItemImage[PlayerStatus.MaxItemCount - 1].sprite = PlayerStatus.ItemSprite[i];
                    ItemImage[PlayerStatus.MaxItemCount - 1].color = new Color(1, 1, 1, 1);
                    PlayerStatus.ItemSprite.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void LoseItem()
    {
        theAudio.Play("Hurt");
        if (PlayerStatus.ani.speed != 0)
        {
            for (int i = 0; i < PlayerStatus.ItemSprite.Count; i++)
            {
                if (ItemImage[i].sprite.name != "BlockItem")
                {
                    ItemImage[i].sprite = null;
                    ItemImage[i].color = new Color(1, 1, 1, 0);
                    //PlayerStatus.ItemSprite.RemoveAt(0);
                }
            }

            for (int i = 0; i < CanMix.Count; i++)
            {
                if (i == 0)
                {
                    CanMix[i].sprite = null;
                    CanMix[i].color = new Color(1, 1, 1, 0);
                }
                else
                {
                    CanMix[i].sprite = OffItem[i - 1];
                }
               
            }
            itemMix.ItemImage1.Clear();
            itemMix.ItemImage2.Clear();
            itemMix.ItemImage3.Clear();
            itemMix.ItemImage4.Clear();
        }
    }

    public void MixStatsOn()
    {
        if (PlayerStatus.ani.speed != 0)
        {
            MixButtonWait = true;
        }
    }

    public void MixStatsOff()
    {
        if (PlayerStatus.ani.speed != 0)
        {
            if (MixWindow.activeSelf == false)
            {
                MixWindow.SetActive(true);
                //itemMix.CheckItems();
            }
           
        }
    }

    public void SortItems()     //아이템을 정렬하기 위한 함수
    {
        for (int i = 0; i < ItemImage.Count - 1; i++)
        {
            if (ItemImage[i].sprite == null && ItemImage[i + 1].sprite.name != "BlockItem")
            {
                ItemImage[i].sprite = ItemImage[i + 1].sprite;
                ItemImage[i + 1].color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void ClearCheck()
    {
        if (CanMix[0].sprite != null)
        {
            if (itemMix.ClearItemCount <= ClearItem.Count)
            {
                ClearItem[itemMix.ClearItemCount].sprite = CanMix[0].sprite;
                ClearItem[itemMix.ClearItemCount].color = itemMix.On;
                itemMix.ClearItemCount++;
                itemMix.DeleteItmes(CanMix[0].sprite.name);
            }
        }
    }

    public void SpeedUpCheck()
    {
        if (CanMix[1].sprite == OnItem[0])
        {
            theAudio.OnePlay("SkillButton");
            MixComplete.sprite = itemMix.Data[0].MixItemImage;
            MixComplete.color = itemMix.On;
            CanMix[1].sprite = OffItem[0];
            itemMix.DeleteItmes(MixComplete.sprite.name);
        }
    }

    public void GhostCheck()
    {
        if (CanMix[2].sprite == OnItem[1])
        {
            theAudio.OnePlay("SkillButton");
            MixComplete.sprite = itemMix.Data[1].MixItemImage;
            MixComplete.color = itemMix.On;
            CanMix[2].sprite = OffItem[1];
            itemMix.DeleteItmes(MixComplete.sprite.name);
        }
    }

    public void SuperPowerCheck()
    {
        if (CanMix[3].sprite == OnItem[2])
        {
            theAudio.OnePlay("SkillButton");
            MixComplete.sprite = itemMix.Data[2].MixItemImage;
            MixComplete.color = itemMix.On;
            CanMix[3].sprite = OffItem[2];
            itemMix.DeleteItmes(MixComplete.sprite.name);
        }
    }

    public void UseItem()
    {
        if ( SkillOn==false&& PlayerStatus.ani.speed != 0&&!PlayerStatus.ani.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            theAudio.Play("Skill");     
            if (MixComplete.sprite != null)
            {
                if (MixComplete.sprite.name == "Ghost")
                {
                    SkillOn = true;
                    StartCoroutine(Invincibility());
                }
                if (MixComplete.sprite.name == "SuperPower")
                {
                    PlayerStatus.PS = PlayerState.Skill;
                    SkillOn = true;
                    PlayerStatus.PowerOn = true;
                }
                if (MixComplete.sprite.name == "SpeedUp")
                {
                    PlayerStatus.PS = PlayerState.Skill;
                    SkillOn = true;
                    StartCoroutine(SpeedUp());
                }
                MixComplete.sprite = null;
                MixComplete.color = new Color(1, 1, 1, 0);
            }
        }
    }
    
    IEnumerator Invincibility()
    {
        PlayerStatus.GostMode = true;
        PlayerStatus.Gost();
        PlayerStatus.GostEndPointCheck.SetActive(true);
        theAudio.Play("Gost");
        yield return new WaitForSeconds(PlayerStatus.InvincibillityTime+0.3f);
        PlayerStatus.GostMode = false;
        PlayerStatus.Gost();
        SkillOn = false;
        PlayerStatus.PS = PlayerState.Run;
        PlayerStatus.GostEndPointCheck.SetActive(false);
    }

    IEnumerator SpeedUp()
    {
        PlayerStatus.SaveSpeed = PlayerStatus.Speed;
        PlayerStatus.Speed = 0;
        PlayerStatus.ani.SetBool("Run", false);
        PlayerStatus.ani.SetBool("Speed_Up", true);
        yield return new WaitForSeconds(0.35f);
        PlayerStatus.Speed = PlayerStatus.MaxSpeed;
        theAudio.OnePlay("SpeedUp");
        yield return new WaitForSeconds(PlayerStatus.SpeedUpTime);
        PlayerStatus.ani.SetBool("Speed_Up", false);
        PlayerStatus.ani.SetBool("Run", true);        
        PlayerStatus.PS = PlayerState.Run;
        if (PlayerStatus.Speed!=0)
        {
            PlayerStatus.Speed = PlayerStatus.SaveSpeed;
            PlayerStatus.PS = PlayerState.Run;
        }
        yield return new WaitForSeconds(0.5f);
        SkillOn = false;
        if(PlayerStatus.ani.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            PlayerStatus.ani.Play("Run");
        }
    
    }
    
}
