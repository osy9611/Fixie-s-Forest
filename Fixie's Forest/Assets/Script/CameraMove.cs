
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    GameObject Player;
    Player PlayerStatus;
    Vector3 Pos;
    Vector3 Pos2;
    Vector3 Pos3;
    public float Speed;
    public float NormalDistance;
    public float SpeedDistance;

    public float Amount;
    public float Duration;

    float Distance;

    Vector3 OriginPos;  //카메라 쉐이크를 하기위한 함수들

    IEnumerator CameraZoomOut()
    {
        while(Distance < SpeedDistance)
        {
            Distance += 0.1f;
            yield return null;
        }
    }

    IEnumerator CameraZoomIn()
    {
        while (Distance > NormalDistance)
        {
            Distance -= 0.1f;
            yield return null;
        }
    }

    public void CameraShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(0.1f);
        float timer = 0;
        while(timer<=Duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * Amount + Pos3;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Pos3;
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<Player>();
        Distance = NormalDistance;
        OriginPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (PlayerStatus.Speed == PlayerStatus.MaxSpeed)
        {
            StartCoroutine(CameraZoomOut());
        }
        else
        {
            StartCoroutine(CameraZoomIn());
        }
       
        Pos = new Vector3(Player.transform.position.x + 4.5f, 0, Player.transform.position.z - Distance);
        Pos2 = new Vector3(this.gameObject.transform.position.x, 5, this.gameObject.transform.position.z);
        Pos3 =  new Vector3(Player.transform.position.x + 4.5f, 3, Player.transform.position.z - Distance);
        gameObject.transform.position = Vector3.Lerp(Pos2, Pos,Speed*Time.deltaTime);
	}
}
