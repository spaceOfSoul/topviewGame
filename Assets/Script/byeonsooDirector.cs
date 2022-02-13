using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class byeonsooDirector : MonoBehaviour,IPunObservable
{
    public static byte character;
    public static bool team;
    public static byte rTeam;
    public static byte bTeam;
    PhotonView pv;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        team = false;
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        character = 1;
        pv.RPC("setTeamNum",RpcTarget.AllBuffered);
    }
    void Update()
    {
        
    }
    public void soldier_1() => character = 1;
    public void soldier_2() => character = 2;
    public void soldier_3() => character = 3;
    public void redTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeam() != PunTeams.Team.red)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
            pv.RPC("increaseR",RpcTarget.AllBuffered);
            if (bTeam > 0)
                pv.RPC("decreaseB", RpcTarget.AllBuffered);
        }
    }
    public void blueTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeam() != PunTeams.Team.blue)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.blue);
            pv.RPC("increaseB", RpcTarget.AllBuffered);
            if (rTeam > 0)
                pv.RPC("decreaseR", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rTeam);
            stream.SendNext(bTeam);
        }
        else
        {
            rTeam = (byte)stream.ReceiveNext();
            bTeam = (byte)stream.ReceiveNext();
        }
    }
    [PunRPC]
    void setTeamNum()
    {
        rTeam = 0;
        bTeam = 0;
    }
    [PunRPC]
    void increaseR() => ++rTeam;
    [PunRPC]
    void increaseB() => ++bTeam;
    [PunRPC]
    void decreaseR() => --rTeam;
    [PunRPC]
    void decreaseB() => --bTeam;
}
