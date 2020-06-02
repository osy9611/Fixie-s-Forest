using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public VideoPlayer video;
    public string name;
    public GameObject SkipUI;
    public GameObject SkipImage;
    bool UICheck;
    private void Start()
    {
        video = GetComponent<VideoPlayer>();
    }

    public void SkipUIOn()
    {
        SkipUI.SetActive(true);
        SkipImage.SetActive(true);
        video.Pause();
        UICheck = true;
    }

    public void YesSkip()
    {
        SceneManager.LoadScene(name);
    }

    public void NoSkip()
    {
        SkipUI.SetActive(false);
        SkipImage.SetActive(false);
        video.Play();
        UICheck = true;
    }
    
    private void Update()
    {
        if (video.isPaused && UICheck==false)
        {
            SceneManager.LoadScene(name);
        }
    }
}
