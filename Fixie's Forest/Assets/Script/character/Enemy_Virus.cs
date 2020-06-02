using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Virus : MonoBehaviour
{
    public EnemyState ES;
    Animator ani;
    Rigidbody2D rigidbody;
    UIManager UI;
    int SoundCount;

    public void AnimationUpdate()
    {
        if (ES == EnemyState.Die)
        {
            ani.Play("Enemy_Virus_Die");
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            if (SoundCount == 0)
            {
                UI.theAudio.OnePlay("VirusMonsterDie");
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
