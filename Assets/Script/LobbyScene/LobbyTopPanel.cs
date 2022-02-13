using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyTopPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";
    
    public Text ConnectionStatusText;
    
    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }
}
