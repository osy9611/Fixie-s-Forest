using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float ScrollSpeed = 0.5f;
    float Target;

	// Update is called once per frame
	void Update () {
        Target += Time.deltaTime * ScrollSpeed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Target, 0);
	}
}
