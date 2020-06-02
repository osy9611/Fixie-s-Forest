using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    //튜토리얼 설명
    public Text TutorialText;
    public GameObject button;
    [TextArea(1,2)]
    public List<string> sentence;
    public List<string> listSentence;
    public int TextCount;
    public float TextSpeed;
    public Player player;
    public bool On;             //글자가 전부 나오기 전까지는 버튼눌림을 방지하기 위함

    public GameObject Panel;    //튜토리얼 판낼

    public GameObject SkipPanel;

    public StoreData store;

    UIManager UI;

    void Start()
    {
        UI = FindObjectOfType<UIManager>();
        listSentence = new List<string>();
        On = true;
        if (GameObject.Find("Data") != null)
        {
            store = GameObject.Find("Data").GetComponent<StoreData>();
        }
    }

    public void ShowText()
    {
        listSentence.Clear();
        TutorialText.text = "";
        for (int i = 0; i < sentence.Count; i++)
        {
            listSentence.Add(sentence[i]);
        }
        StartCoroutine(StartTextCorutine());
    }
    
    public void ExitText()
    {
        TutorialText.text = "";
        TextCount = 0;
        listSentence.Clear();
    }

    IEnumerator StartTextCorutine()
    {
        On = false;
        for (int i = 0; i < listSentence[TextCount].Length; i++)
        {
            TutorialText.text += listSentence[TextCount][i];    //1글자씩 출력
            yield return new WaitForSeconds(TextSpeed);
        }

        if(TutorialText.text == listSentence[TextCount])
        {
            On = true;
        }
    }

    public void TutorialButton()
    {
        if (On == true)
        {
            TextCount++;
            TutorialText.text = "";
            if (TextCount == listSentence.Count - 1)
            {
                StopAllCoroutines();
                ExitText();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartTextCorutine());
            }

            if (TutorialText.text == "")
            {
                player.ani.speed = 1;
                player.Speed = player.SaveSpeed;
                player.rigidbody.simulated = true;
                sentence.RemoveAt(TextCount);
                Panel.SetActive(false);
                button.SetActive(false);
            }
        }
    }

    public void SkipTutorial()
    {
        Time.timeScale = 0;
        SkipPanel.SetActive(true);
        TutorialText.gameObject.SetActive(false);
        UI.theAudio.Play("UI");
    }

    public void YesSkip()
    {
        store.TutorialDone = true;
        SceneManager.LoadScene("SelectStage");
        UI.theAudio.Play("UI");
    }

    public void NoSkip()
    {
        Time.timeScale = 1;
        SkipPanel.SetActive(false);
        TutorialText.gameObject.SetActive(true);
        UI.theAudio.Play("UI");
    }

    private void FixedUpdate()
    {
        if (TextCount == listSentence.Count - 1)
        {
            StopAllCoroutines();
            ExitText();
            store.TutorialDone = true;

        }
    }

}
