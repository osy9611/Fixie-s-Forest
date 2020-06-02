using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public Player player;
    Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos.x = Mathf.Lerp(transform.position.x, player.transform.position.x, 2 * Time.smoothDeltaTime);
        pos.y = 0;
        transform.position = pos;

    }
}
