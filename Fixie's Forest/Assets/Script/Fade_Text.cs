using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_Text : MonoBehaviour {

    public Text text;
    Color color;
    public float FadeInTime = 1.0f;
    public float FadeOutTime = 1.0f;
    float time = 1.0f;
    private void Start()
    {
        color = text.color;
    }
    
    IEnumerator FadeIn()
    {
        while (color.a < 1f)
        {
            color.a += Time.fixedDeltaTime / FadeInTime;
            text.color = color;
            if(color.a>=1f)
            {
                color.a = 1f;
            }

            yield return null;
        }

        text.color = color;
    }

    IEnumerator FadeOut()
    {
        while (color.a > 0f)
        {
            color.a -= Time.fixedDeltaTime * FadeOutTime;
            text.color = color;

            if(color.a<=0f)
            {
                color.a = 0f;
            }

            yield return null;
        }
        text.color = color;

        StartCoroutine(FadeIn());

    }
	// Update is called once per frame
	void FixedUpdate () {

        if(color.a==1)
        {
            StartCoroutine(FadeOut());
        }
    }
}
