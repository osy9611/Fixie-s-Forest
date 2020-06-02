using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    D_Jump,
    Hurt,
    Attack,
    Skill,
    Die
}

public class Player : MonoBehaviour {

    //캐릭터 관련 
    public PlayerState PS;
    public float Speed;
    public float MaxSpeed;
    public float SuperPowerSpeed;
    public float SaveSpeed;
    public float LawSpeed;
    public float JumpPower;
    public Rigidbody2D rigidbody;
    public float JumpLimit;     //점프 제한횟수
    bool JumpStop;              //점프를 못하게 하는 역할
    public bool GetItem;
    public float JumpCount;     //점프 횟수
    public Animator ani;        //캐릭터 애니메이터
    Animation animation;        //캐릭터 애니메이션 
    public SpriteRenderer render;      //캐리터 스프라이트 랜더
    Vector2 OriginScale = new Vector2(1, 1);

    //아이템 쿨타임 관련
    public float InvincibillityTime;    //무적시간
    public float SpeedUpTime;           //스피드업 시간

    //캐릭터가 계단에 있는지 바닥에 있는지 확인하는 함수
    public bool StairOn;
    //UI관련
    public UIManager UI;

    //게임 매니져
    public GameManager manager;

    //아이템
    public List<Sprite> ItemSprite;
    public int ItemCount;
    public int MaxItemCount;
    public bool PowerOn;    //오브젝트 파괴 아이템 사용
    public bool PowerDelay; //잠깐 무적을 채크하기 위하여 사용
    public bool Infinity;       //무적체크
    public GameObject GostEndPointCheck;//영체화 상태일때 클리어 여부 체크하기 위한 함수
    public CameraMove MoveCamera;

    public GameObject tutorial;     //튜토리얼 관련함수
    public float PowerRange;        //슈퍼파워 공격거리
    public bool GostMode;           //영체화 체크하는 함수

    //포물선 공식관련 함수들
    float tx, ty, tz;
    float tx1, ty1, tz1;
    float v;
    public float g = 9.8f;
    float elapsed_time;
    public float max_height;
    float t;
    private Vector2 start_pos, end_pos, pos;
    float dat;
    Vector2 tpos;
    bool Stop;

    public EndPoint endpoint;
   
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        animation = GetComponent<Animation>();
        render = GetComponent<SpriteRenderer>();
        endpoint = FindObjectOfType<EndPoint>();
        if(endpoint.data!=null)
        {
            MaxItemCount = endpoint.data.MaxItemCount;
        }
        
    }

    public void PlayerMove()
    {
        if (PS != PlayerState.Die && PS != PlayerState.Attack)
        {
            transform.Translate(Speed * Time.smoothDeltaTime, 0, 0);
        }
        else if (PS == PlayerState.Die)
        {
            ani.SetBool("Die", true);
        }

        if(ani.GetCurrentAnimatorStateInfo(0).IsName("Speed_Up") && UI.SkillOn==false)
        {
            ani.Play("Run");
        }
    }

    public void JumpButton()
    {
        if (PS != PlayerState.Die && PS!=PlayerState.Skill)
        {
            if (PS == PlayerState.Jump)
            {
                D_Jump();
            }
            if (PS==PlayerState.Run)
            {
                Jump();
            }
        }
    }

    void Jump()
    {        
        PS = PlayerState.Jump;
        ani.SetTrigger("Jump");
        ani.SetBool("Run", false);       
        UI.theAudio.Play("Jump");
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(Vector3.up * JumpPower);
    }

    void D_Jump()
    {
        PS = PlayerState.D_Jump;
        ani.SetTrigger("D_Jump");
        UI.theAudio.Play("Jump");
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(Vector3.up * JumpPower);
    }
    
    public void GetItemsOn()
    {
        GetItem = true;
    }

    public void GetItemsOff()
    {
        GetItem = false;
    }

    public void DropItem()
    {
        UI.theAudio.Play("Throw");
        if (UI.ItemImage[0].sprite != null)
        {
            for (int i = 0; i < MaxItemCount-1; i++)
            {
                UI.ItemImage[i].sprite = UI.ItemImage[i + 1].sprite;
                UI.ItemImage[i + 1].sprite = null;
            }

            ItemSprite.RemoveAt(0);
            ItemCount--;
        }

        for(int i=0;i<UI.ItemImage.Count;i++)
        {
            if(UI.ItemImage[i].sprite==null)
            {
                UI.ItemImage[i].color = new Color(1, 1, 1, 0);
            }
        }


        for (int i = 0; i < UI.CanMix.Count; i++)
        {
            if (i == 0)
            {
                UI.CanMix[i].sprite = null;
                UI.CanMix[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                UI.CanMix[i].sprite = UI.OffItem[i - 1];
            }

        }
        UI.itemMix.ItemReset();
        UI.itemMix.CheckItems();
    }

    void SuperPower()
    {
        ani.SetTrigger("SuperPower_Wait");
        UI.theAudio.OnePlay("SuperPower");
        Vector2 Pos = new Vector2(transform.position.x,transform.position.y+1);
        RaycastHit2D hit = Physics2D.Raycast(Pos, Vector2.right, PowerRange);
        if (hit.collider!=null && PS!=PlayerState.Jump && PS!=PlayerState.D_Jump)
        {            
            if (hit.collider.gameObject.tag.Equals("VirusMonster"))
            {
                SaveSpeed = Speed;
                PS = PlayerState.Attack;
                StartCoroutine(KillVirus(hit.collider.gameObject.GetComponent<VirusEnemy>()));
                ani.SetTrigger("SuperPower_On");
            }
            if (hit.collider.gameObject.tag.Equals("Obstacle"))
            {
                SaveSpeed = Speed;
                //PS = PlayerState.Attack;
                StartCoroutine(BreakObj(hit.collider.gameObject));
                ani.SetTrigger("SuperPower_On");
                rigidbody.AddForce(Vector3.up * 800);
            }
        }
        PowerOn = false;
        PowerDelay = true;
        StartCoroutine(ItemDelay());
    }
    
    IEnumerator ItemDelay()
    {
        yield return new WaitForSeconds(1.0f);
        PowerDelay = false;
        UI.SkillOn = false;
    }

    IEnumerator BreakObj(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("SuperPower_On"))
        {
            yield return new WaitForSeconds(0.5f);
            UI.theAudio.OnePlay("ThornBreak");
            MoveCamera.CameraShake();
            obj.SetActive(false);
            MoveCamera.CameraShake();
            PS = PlayerState.Run;
            Speed = SaveSpeed+1;

           yield return new WaitForSeconds(0.3f);
            Speed = SaveSpeed;
        }
    }
    
    IEnumerator KillVirus(VirusEnemy enemy)
    {
        start_pos = transform.position;
        end_pos = new Vector2(enemy.transform.position.x - 4, enemy.transform.position.y);
        
        var dh = enemy.transform.position.y - transform.position.y;
        var mh = (enemy.transform.position.y + max_height) - transform.position.y;
        if(mh<0)
        {
            Mathf.Abs(mh);
        }
        ty = Mathf.Sqrt(2 * g * mh);

        float a = 9;
        float b = -2 * ty;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        tx = -(start_pos.x - end_pos.x) / dat;

        elapsed_time = 0;

        while(true)
        {
            this.elapsed_time += Time.smoothDeltaTime*2.5f;

            tx1 = start_pos.x + this.tx * elapsed_time;
            ty1 = start_pos.y + (this.ty * elapsed_time) - (0.5f * g * elapsed_time * elapsed_time);
            tpos = new Vector2(tx1, ty1);
            
            transform.position = tpos;

            if(Stop==true)
            {
                if(this.elapsed_time >=this.dat)
                {
                    transform.position = new Vector2(transform.position.x, enemy.transform.position.y);
                    enemy.ES = EnemyState.Die;
                    MoveCamera.CameraShake();
                    PS = PlayerState.Run;
                    Speed = SaveSpeed;
                    break;
                }
            }
            yield return null;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            if (PS != PlayerState.Die && PS != PlayerState.Skill && PS != PlayerState.Hurt && PS!=PlayerState.Attack)
            {
                ani.Play("Run");
                JumpCount = 0;
                StairOn = false;
                Stop = true;
                PS = PlayerState.Run;
            }
        }
        if (collision.gameObject.tag == "Stairs")
        {
            if (PS != PlayerState.Die && PS != PlayerState.Skill && PS != PlayerState.Hurt && PS != PlayerState.Attack)
            {
                ani.Play("Run");
                JumpCount = 0;
                StairOn = false;
                Stop = true;
                PS = PlayerState.Run;
            }
        }

        if(collision.gameObject.tag=="SkyStep")
        {
            PS = PlayerState.Die;
            UI.GameOver.SetActive(true);
        }

        if (collision.gameObject.tag == "SkyJump")
        {
            PS = PlayerState.Die;
            UI.GameOver.SetActive(true);
        }

    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle"&&rigidbody.simulated==false)
        {
            collision.collider.isTrigger = true;
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "VirusMonster")
        {
            if (PS!=PlayerState.Attack&&collision.gameObject.GetComponent<VirusEnemy>().ES != EnemyState.Die && Speed!=MaxSpeed && GostMode==false)
            {
                if (PowerDelay == false && PowerOn==false)
                {
                    PS = PlayerState.Hurt;
                    ani.Play("On_Damage");
                    GostFadeOn();
                    UI.LoseItem();
                    ItemSprite.Clear();
                    ItemCount = 0;
                    collision.gameObject.GetComponent<VirusEnemy>().ES = EnemyState.Die;
                    StartCoroutine(Back());
                    UI.itemMix.ItemReset();
                    UI.itemMix.CheckItems();
                    if (ani.GetCurrentAnimatorStateInfo(0).IsName("Speed_Up"))
                    {
                        Speed = SaveSpeed;
                    }
                    UI.SkillOn = true;
                    StartCoroutine(ItemDelay());
                }
            }
        }
        if (collision.gameObject.tag == "Item")
        {
            if (ItemCount != MaxItemCount && !ani.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                ItemSprite.Add(collision.gameObject.GetComponent<SpriteRenderer>().sprite);
                ItemCount++;
                UI.SetItem();
                UI.theAudio.Play("Item");
                collision.gameObject.SetActive(false);
                UI.itemMix.ItemReset();
                UI.itemMix.CheckItems();
            }
        }
        if(collision.gameObject.tag == "Stone")
        {
            if (ItemCount != MaxItemCount && !ani.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                ItemSprite.Add(collision.gameObject.GetComponent<SpriteRenderer>().sprite);
                ItemCount++;
                UI.SetItem();
                UI.theAudio.Play("Stone");
                collision.gameObject.SetActive(false);
                UI.itemMix.ItemReset();
                UI.itemMix.CheckItems();
            }
        }
        if (collision.gameObject.tag == "BlockItem")
        {
            if(ItemCount != MaxItemCount)
            {
                ItemSprite.Add(collision.gameObject.GetComponent<SpriteRenderer>().sprite);
                UI.SetBlockItem();
                MaxItemCount--;
                UI.theAudio.Play("Stone");
                collision.gameObject.SetActive(false);                
            }
        }
        if(collision.gameObject.tag=="Step")
        {
            if(ani.GetCurrentAnimatorStateInfo(0).IsName("Speed_Up"))
            {
                ani.SetBool("Run", false);
                collision.isTrigger = false;
            }
        }

        if(collision.gameObject.tag=="Water")
        {
            PS = PlayerState.Die;
            UI.GameOver.SetActive(true);
        }

        if (collision.gameObject.tag == "Stairs")
        {
            Collider2D col = collision.gameObject.GetComponent<Collider2D>();
            if (col.isTrigger == true)
            {
                col.isTrigger = false;
            }
        }
    }

    IEnumerator Back()
    {
        SaveSpeed = Speed;

        while(Speed>0)
        {
            Speed -= LawSpeed;
            if(Speed<0)
            {
                Speed = 0;
                PS = PlayerState.Run;
            }
            yield return null;
        }
        rigidbody.simulated = false;
        yield return new WaitForSeconds(0.5f);
        rigidbody.simulated = true;
        Speed = SaveSpeed;
    }
    
    public void Gost()
    {
        if (GostMode == true)
        {
            StartCoroutine(GostOn());
        }
        else if (GostMode == false)
        {
            StartCoroutine(GostOff());
        }
    }

    public void FadeSet()
    {
        StartCoroutine(DieFade());

    }

    IEnumerator DieFade()
    {
        Color color = render.color;
        while (render.color.a > 0.0f)
        {
            color.a -= Time.fixedDeltaTime * 1.0f;
            render.color = color;

            if (color.a <= 0)
            {
                color.a = 0;
                UI.GameOver.SetActive(true);
            }
            yield return null;
        }
        render.color = color;
    }

    IEnumerator GostOn()
    {
        Color color = render.color;
        while(render.color.a>0.5f)
        {
            color.a -= Time.fixedDeltaTime * 1.0f;
            render.color = color;

            if(color.a<=0.5f)
            {
                color.a = 0.5f;
            }
            yield return null;
        }
        render.color = color;
    }

    IEnumerator GostOff()
    {
        Color color = render.color;
        while (render.color.a < 1f)
        {
            color.a += Time.fixedDeltaTime / 1.0f;
            render.color = color;

            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
        render.color = color;
    }

    public void GostFadeOn()
    {
        StartCoroutine(GostFade());
    }

    IEnumerator GostFade()
    {
        int CountTime = 0;
        while (CountTime < 10)
        {
            if(CountTime %2==0)
            {
                render.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                render.color = new Color(1, 1, 1, 1);
            }

            yield return new WaitForSeconds(0.2f);
            CountTime++;
        }

        render.color = new Color(1, 1, 1, 1);

        yield return null;
    }

	void FixedUpdate () {

        PlayerMove();
        if (PowerOn == true)
        {
            SuperPower();
        }
        if(PS==PlayerState.Die)
        {
            FadeSet();
        }
    }
}
