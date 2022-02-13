using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaserMisaile : MonoBehaviour
{
    float timeCount;
    void Start()
    {
        timeCount = 0;
    }

    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= 1f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
