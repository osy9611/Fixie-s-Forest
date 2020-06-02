using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwipe : MonoBehaviour
{
    float Speed;
    Vector2 StartPos;
    Vector2 EndPos;
    float SwipeLength;
    public float SwipeSpeed;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.StartPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            EndPos = Input.mousePosition;
            SwipeLength = (EndPos.x - StartPos.x);
            this.Speed = SwipeLength / SwipeSpeed;
        }

        transform.Translate(Speed, 0, 0);
        Speed *= 0.98f;
    }
}
