using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour {
    
    public Text text;
    public List<string> listSentence;
    private int count;      //대화 진행 상황 카운트


    // Use this for initialization
    void Start () {
        listSentence = new List<string>();	
	}

    public void ShowDialogue(Dialog dialog)
    {
        for(int i=0;i<dialog.sentences.Length;i++)
        {
            listSentence.Add(dialog.sentences[i]);
        }

        StartCoroutine(StartDialogueCorutine());
    }
	
    public void ExitDialog()
    {
        text.text = "";
        count = 0;
        listSentence.Clear();
    }
    IEnumerator StartDialogueCorutine()
    {
        for(int i=0;i<listSentence[count].Length;i++)
        {
            text.text += listSentence[count][i];    //1글자씩 출력
            yield return new WaitForSeconds(0.01f);
        }
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z))
        {
            count++;
            text.text = "";
            if (count != listSentence.Count -1)
            {
                StopAllCoroutines();

                ExitDialog();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartDialogueCorutine());
            }
        }
	}
}
