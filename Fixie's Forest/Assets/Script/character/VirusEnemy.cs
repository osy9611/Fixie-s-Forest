using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Virus,
    Normal
}
public class VirusEnemy : MonoBehaviour {

    public EnemyState ES;
    public EnemyType ET;
    Animator ani;
    Rigidbody2D rigidbody;
    UIManager UI;
    int SoundCount;

    public void AnimationUpdate()
    {
        if (ES == EnemyState.Die && ET == EnemyType.Virus)
        {
            ani.Play("VirusMonster_Die");
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            if (SoundCount == 0)
            {
                UI.theAudio.OnePlay("VirusMonsterDie");
                SoundCount++;
            }
        }
        else if (ES == EnemyState.Die && ET == EnemyType.Normal)
        {
            ani.SetBool("Die", true);
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            if (SoundCount == 0)
            {
                UI.theAudio.OnePlay("Monster02");
                SoundCount++;
            }
        }
    }

    IEnumerator EnemyDie()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        UI = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        AnimationUpdate();
    }
}
