using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class After_Effect : MonoBehaviour
{
    public Player player;
    public SpriteRenderer spriteSrc;
    public bool AfterEffectOn;
    public Color color;

    void Start()
    {
        AfterEffectOn = false;
        StartCoroutine(AfterEffect());
    }

    IEnumerator AfterEffect()
    {
        while(true)
        {
            while(AfterEffectOn)
            {

                SpriteRenderer spriteCopy = Instantiate(spriteSrc) as SpriteRenderer;
                spriteCopy.transform.position = spriteSrc.transform.position;
                spriteCopy.transform.localScale = spriteSrc.transform.parent.transform.localScale;
                spriteCopy.color = color;
                spriteCopy.sortingOrder = 1;
                
            }
        }
    }
   
}
