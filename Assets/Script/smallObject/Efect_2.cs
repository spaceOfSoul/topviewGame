using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Efect_2 : MonoBehaviour
{
    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("DEst",RpcTarget.AllBuffered);
    }
    [PunRPC]
    void DEst() => Destroy(gameObject,0.21f);  
}