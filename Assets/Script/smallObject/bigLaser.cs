using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bigLaser : MonoBehaviour
{
    float animationTime, desTime;
    public AudioClip s;
    AudioSource speak;
    PhotonView pv;

    void Start()
    {
        animationTime = 0;
        desTime = 0;
        speak = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();

        pv.RPC("playSound",RpcTarget.AllBuffered);
    }

    void Update()
    {
        desTime += Time.deltaTime;
        animationTime += Time.deltaTime;
        if(animationTime >= 0.05f)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
            animationTime = 0;
        }
        if (desTime >= 0.5f)
            PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    void playSound()
    {
        speak.clip = s;
        speak.Play();
    }
}
