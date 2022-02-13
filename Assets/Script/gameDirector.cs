using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class gameDirector : MonoBehaviourPunCallbacks
{
    public static gameDirector Instance = null;
    public GameObject quitAndStart;
    public GameObject DeadMenu;
    public GameObject canvas;

    [Header("PlayerUI")]
    public GameObject deadPanal;
    public GameObject killog;
    public Text respwnTime;
    public GameObject gameover;
    [Header("GameOver")]
    public Text whosWinner;
    public Text restartTime;

    float respawntime;
    float restartTimer;
    Vector3 spawnPoint;
    bool instan;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Hashtable props = new Hashtable
            {
                {BlackJack.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        if (PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red)
            spawnPoint = new Vector3(32, Random.Range(-10, 10), 0);
        else
            spawnPoint = new Vector3(-32, Random.Range(-10,10), 0);
        if (byeonsooDirector.character == 1)
        {
            GameObject p = PhotonNetwork.Instantiate("ugle", spawnPoint, Quaternion.identity);
        }
        else if (byeonsooDirector.character == 2)
        {
            GameObject p = PhotonNetwork.Instantiate("eire", spawnPoint, Quaternion.identity);
        }
        Debug.Log(byeonsooDirector.character);
        quitAndStart.SetActive(false);
        deadPanal.SetActive(false);
        DeadMenu.SetActive(false);
        gameover.SetActive(false);
        respawntime = 0;
        instan = false;
        restartTimer = 0;
    }
    private void Update()
    {
        if (PlayerControl.alive == false)
        {
            deadPanal.SetActive(true);
            respawntime += Time.deltaTime;
            respwnTime.text = "리스폰까지 남은 시간 : " + (5-respawntime).ToString("N0");
            if (respawntime >= 5f)
            {
                if (byeonsooDirector.character == 1)
                    PhotonNetwork.Instantiate("ugle", spawnPoint, Quaternion.identity);
                else if (byeonsooDirector.character == 2)
                    PhotonNetwork.Instantiate("eire", spawnPoint, Quaternion.identity);
                deadPanal.SetActive(false);
                respawntime = 0;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                DeadMenu.SetActive(true);
        }
        else
            instan = false;
        if (PlayerControl.alive && Input.GetKeyDown(KeyCode.Escape))
        {
            quitAndStart.SetActive(true);
        }
        if (captureFeild.winner > 0)
        {
            gameover.SetActive(true);
            if (captureFeild.winner == 1)
                whosWinner.text = "RedTeam win!";
            else if (captureFeild.winner == 2)
                whosWinner.text = "BlueTeam win!";
            restartTimer += Time.deltaTime;
            restartTime.text = "경기 재시작까지 남은 시간 : " + (10-restartTimer).ToString("N0");
            if (restartTimer >= 10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void quit()
    {
        Application.Quit();
    }
    public void continu()
    {
        quitAndStart.SetActive(false);
    }
    public void backToLobby()
    {
        PhotonNetwork.LeaveRoom();
        if (!PhotonNetwork.InRoom)
            SceneManager.LoadScene("LobyScene");
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void eireC()
    {
        byeonsooDirector.character = 2;
    }
    public void ugleC()
    {
        byeonsooDirector.character = 1;
    }
    #region RPC
    [PunRPC]
    void addTagR(GameObject p) => p.tag = "rTeam";
    [PunRPC]
    void addTagB(GameObject p) => p.tag = "bTeam";
    #endregion
}
