using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y+2,0);
    }
}
