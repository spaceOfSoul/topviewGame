using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bullet_1 : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject Efect;
    Rigidbody2D B_rb;
    PhotonView pv;
    Vector2 networkPosition;
    float networkRotation;
    public string _playerId = "";
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        B_rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "laser")
            return;
        PhotonNetwork.Instantiate("efect_2", transform.position, Quaternion.identity);
        pv.RPC("DestroyGameobjectRPC", RpcTarget.AllBuffered);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "laser" || collision.gameObject.name == "captureFeild")
            return;
        PhotonNetwork.Instantiate("efect_2", transform.position, Quaternion.identity);
        pv.RPC("DestroyGameobjectRPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void DestroyGameobjectRPC() => Destroy(gameObject);
    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(B_rb.position);
            stream.SendNext(B_rb.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (float)stream.ReceiveNext();
        }
    }
}
