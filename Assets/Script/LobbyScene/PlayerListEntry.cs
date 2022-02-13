
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class PlayerListEntry : MonoBehaviour
{
        public Text PlayerNameText;

        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage;
        private int ownerId;
        private bool isPlayerReady;

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }
    
        public void Start()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                PlayerReadyButton.gameObject.SetActive(false);
            }
            else
            {
                Hashtable initialProps = new Hashtable() {{ BlackJack.PLAYER_READY, isPlayerReady}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                PlayerReadyButton.onClick.AddListener(() =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() {{ BlackJack.PLAYER_READY, isPlayerReady}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
                });
            }
        }
    private void Update()
    {
        OnPlayerNumberingChanged();
        if (PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.none)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            PlayerReadyButton.gameObject.SetActive(true);
        }
    }
    public void OnDisable()
    {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

        #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    private void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == ownerId)
            {
                if (p.GetTeam() == PunTeams.Team.red)
                    PlayerColorImage.color = Color.red;
                else if (p.GetTeam() == PunTeams.Team.blue)
                    PlayerColorImage.color = Color.blue;
                else
                    PlayerColorImage.color = Color.white;
            }
        }
    }

        public void SetPlayerReady(bool playerReady)
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
            PlayerReadyImage.enabled = playerReady;
        }
}
