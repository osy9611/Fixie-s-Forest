﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;     //사운드 이름
    public AudioClip Clip;  //사운드 파일
    private AudioSource Source; //사운드 플레이어

    public float Volum;
    public bool loop;

    public void Setsource(AudioSource _source)
    {
        Source = _source;
        Source.clip = Clip;
        Source.loop = loop;
    }

    public void SetVolume()
    {
        Source.volume = Volum;
    }

    public void Play()
    {
        Source.Play();
    }

    public void OnePlay()
    {
        if(!Source.isPlaying)
        {
            Source.Play();
        }
    }

    public void Stop()
    {
        Source.Stop();
    }

    public void SetLoop()
    {
        Source.loop = true;
    }

    public void SetLoopCancel()
    {
        Source.loop = false;
    }
}
public class AudioManager : MonoBehaviour {

    static public AudioManager instance;
    [SerializeField]
    public Sound[] sounds;


	// Use this for initialization
	void Start () {
		for(int i=0;i<sounds.Length;i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 :" + i + "=" + sounds[i].Name);
            sounds[i].Setsource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);

            sounds[i].SetVolume();
        }
	}

    public void Play(string _name)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if (_name == sounds[i].Name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

  
    public void OnePlay(string _name)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if(_name == sounds[i].Name)
            {
                sounds[i].OnePlay();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if(_name==sounds[i].Name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if(_name==sounds[i].Name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if(_name == sounds[i].Name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolume(string _name,float _Volume)
    {
        for(int i=0;i<sounds.Length;i++)
        {
            if(_name == sounds[i].Name)
            {
                sounds[i].Volum = _Volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
