using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Jump,
    Die
}

public class Enemy : MonoBehaviour {

    public EnemyState ES;
    Animator ani;
    public float Speed;
    float SaveSpeed;
    public float MinSpeed;
    public float MaxSpeed;
    public float JumpPower;
    public float MaxJumpPower;
    public float SavePower;
    public float JumpTime;
    public GameObject Player;
    Player player;
    public float PlayerRange;
    public UIManager UI;            //아이템을 강제로 버리려고 만듬
    bool Change;
    public bool JumpOn;             //점프를 위한 불값
    public bool AttackOn;
    Rigidbody2D rigidbody;
    float Rand;
    Collider2D col;

    Vector2 PlayerPos;
    Vector2 MovePos;

    //씬을 체크하는 함수(2스테이지 이동을 위하여 만듬)
    public bool Stage2;
    public bool SkyJump;
    //사운드 관련 함수
    float RandomSoundTime;
    public float SoundTimeMax;
    public float SoundTimeMin;
    string[] SoundName = { "Monster01", "Monster02", "Monster03"};

    private void Start()
    {
        ani = GetComponent<Animator>();
        player = Player.GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        SavePower = JumpPower;
        col = GetComponent<Collider2D>();
        UI = FindObjectOfType<UIManager>();
        if(SceneManager.GetActiveScene().name == "Stage2")
        {
            Stage2 = true;
        }

    }

    public void AnimationUpdate()
    {
        if (ES == EnemyState.Die)
        {
            ani.SetBool("Die", true);
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    IEnumerator EnemyDie()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    IEnumerator MoveEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        DistanceCheck();
    }

    void DistanceCheck()
    {
        Vector2 ray = new Vector2(transform.position.x + 2.0f, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.down, 0.8f);
        Debug.DrawRay(ray, Vector3.down, new Color(1, 0, 0), 0.8f);
        float Distance = Vector2.Distance(this.transform.position, player.transform.position);      //플레이어와 적의 거리

        Vector2 Pos = new Vector2(transform.position.x, 0);
       
        if (Distance > PlayerRange)
        {
            PlayerPos = new Vector2(player.transform.position.x, this.transform.position.y);
            if (hit.collider != null && player.Speed !=0)
            {
                if (hit.collider.gameObject.tag.Equals("Floor"))
                {
                    ani.Play("NormalEnemy_Idle");
                    SkyJump = false;
                    Jump();
                }
                if (hit.collider.gameObject.tag.Equals("Stairs"))
                {
                    ani.Play("NormalEnemy_Idle");
                    Jump();
                }
             
            }
            if (SkyJump == true)
            {
                transform.position = Vector2.Lerp(this.transform.position, player.transform.position, Speed / 2 * Time.smoothDeltaTime);
            }
            else
            {
                transform.position = Vector2.Lerp(this.transform.position, PlayerPos, Speed / 2 * Time.smoothDeltaTime);
            }       
        }
        else if (Distance <= PlayerRange)
        {
            if (AttackOn == false)
            {
                AttackOn = true;
                StartCoroutine(Attack());
            }
        }
    }

    void Jump()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("NormalEnemy_Idle"))
        {
            ani.speed = 1.0f;
            ani.SetTrigger("Jump");
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(Vector2.up * JumpPower);
            Speed = Random.Range(MinSpeed, MaxSpeed);
            StartCoroutine(Penetrate());
        }
    }

    void MaxJump()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("NormalEnemy_Idle"))
        {
            ani.speed = 0.5f;
            ani.SetTrigger("Jump");
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(Vector2.up * MaxJumpPower);
            Speed = Random.Range(MinSpeed, MaxSpeed);
            StartCoroutine(Penetrate());
        }
    }

    IEnumerator Penetrate()
    {
        col.isTrigger = true;
        yield return new WaitForSeconds(0.5f);

        if(player.Speed!=0)
        {
        col.isTrigger = false;
            RandomSoundTime = Random.Range(SoundTimeMax, SoundTimeMin);
            yield return new WaitForSeconds(RandomSoundTime);
            int Rand = Random.Range(0, 2);
            UI.theAudio.OnePlay(SoundName[Rand]);
        }
    }

    IEnumerator Attack()
    {
        SaveSpeed = Speed;
        if (AttackOn == true && player.PowerDelay == false && player.GostMode==false)
        {
            if (UI.ItemImage[0].sprite != null)
            {
                Speed = 0;
                UI.LoseItem();
                UI.itemMix.ItemReset();
                UI.itemMix.CheckItems();
                player.ItemSprite.Clear();
                player.ItemCount = 0;
                player.SaveSpeed = player.Speed;
                player.Speed = 0;    
                player.ani.SetTrigger("Damage");
                player.GostMode = true;
                UI.SkillOn = true;
                UI.theAudio.OnePlay("Monster04");
                player.GostFadeOn();
            }
            else if (UI.ItemImage[0].sprite == null && player.PS != PlayerState.Die)
            {
                player.PS = PlayerState.Die;
            }
        }

        yield return new WaitForSeconds(1.0f);
        player.GostMode = false;
        UI.SkillOn = false;
        player.Speed = player.SaveSpeed;
        yield return new WaitForSeconds(1.5f);
        AttackOn = false;
        Speed = SaveSpeed;
    }

    IEnumerator SpeedChange()
    {
        Change = true;
        yield return new WaitForSeconds(0.5f);
        Change = false;
        Speed = Random.Range(MinSpeed, MaxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Jump();
        }
        if(collision.gameObject.tag=="SkyStep")
        {
            Jump();
        }
        if (collision.gameObject.tag == "SkyJump")
        {
            SkyJump = true;
            MaxJump();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Jump();
        }
        if (collision.gameObject.tag == "SkyStep")
        {
            Jump();
        }
    }
    
 
    private void FixedUpdate()
    {
        if (player.Speed != 0)
        {
            DistanceCheck();
        }
        if (player.Speed == 0)
        {
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("NormalEnemy_Jump"))
            {
                ani.Play("NormalEnemy_Idle");
            }
        }

    }
}
