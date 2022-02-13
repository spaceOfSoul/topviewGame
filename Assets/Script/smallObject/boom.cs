using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class boom : MonoBehaviour
{
    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("DEstB", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void DEstB() => Destroy(gameObject, 0.21f);
}
