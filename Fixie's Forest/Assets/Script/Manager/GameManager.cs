using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    public UIManager UI;
    AudioManager theAudio;
    public bool Clear;

    public List<Sprite> Item;   //클리어 아이템을 넣는곳
        
    public GameObject TitleButton;
    public GameObject StartButton;
    public GameObject ExitButton;

    public StoreData store;

    public void MainTitle()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("MainTitle");
    }

    public void Title()
    {
        TitleButton.SetActive(false);
        StartButton.SetActive(true);
        ExitButton.SetActive(true);
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SelectStage");
    }

    public void Stage1()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("Stage1");
    }

    public void Stage2()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("Stage2");
    }

    public void Stage3()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("Stage3");
    }

    public void tutorial()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("Tutorial");
    }

    public void GameExit()
    {
        UI.theAudio.OnePlay("Button");
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        if (store !=null)
        {
            if(store.TutorialDone ==true)
            {
                SceneManager.LoadScene("SelectStage");
                theAudio.OnePlay("Button");
            }
            else
            {
                SceneManager.LoadScene("StartCutScene(Tuto)");
                theAudio.OnePlay("Button");
            }
        }
        else
        {
            SceneManager.LoadScene("SelectStage");
            theAudio.OnePlay("Button");
        }
    }

    public void Replay()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GamePause()
    {
        UI.InGameMenu.SetActive(true);
        theAudio.Play("UI");
        Time.timeScale = 0;
    }

    public void Close()
    {
        UI.InGameMenu.SetActive(false);
        theAudio.OnePlay("Button");
        Time.timeScale = 1;
    }

    public void CutScene()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("SelectCutScene");
    }

    public void StartCutScene()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("StartCutScene");
    }

    public void EndCutScene()
    {
        Time.timeScale = 1;
        theAudio.OnePlay("Button");
        SceneManager.LoadScene("EndCutScene");
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        theAudio = FindObjectOfType<AudioManager>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

   

}
