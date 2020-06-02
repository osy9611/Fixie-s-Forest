using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tree
{
    public Vector2 TreePos;
    public Vector2 TreePos2;
    public Vector2 TreePos3;
}
public class StoreData : MonoBehaviour {

    public int MaxItemCount;
    public List<Sprite> ItemSprite;
    public bool TutorialDone;

    //클리어가 됬는지 확인하는 함수
    public bool Stage1;
    public bool Stage2;
    public bool Stage3;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
