using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    enum RoomMode { BLIND_PICK, RANDOM_PICK }

    private static NetworkManager instance = null;

    private List<RoomInfo> myList = new List<RoomInfo>();
    public List<RoomInfo> MyList { get { return myList; } }

    public Transform playerListContent;
    public LobbySceneUI lobbyUI;
    public GameObject startGameButton;
    public PhotonView pV;
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnCreatedRoom() => Debug.Log("방 생성 완료");

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedRoom()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(lobbyUI.UserNameTextPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = UI.Instance.UserID;
        GameManager.Instance.LoadLobbyScene();
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(UI.Instance.RoomPassword) && !string.IsNullOrEmpty(UI.Instance.RoomName))
        {
            PhotonNetwork.CreateRoom(UI.Instance.RoomName + "_" + UI.Instance.RoomPassword, new RoomOptions { MaxPlayers = 2, IsVisible = true });
            lobbyUI.UserCustumGameLobby();
            Debug.Log("CreateSecretRoom");
        }
        else if (!string.IsNullOrEmpty(UI.Instance.RoomName))
        {
            PhotonNetwork.CreateRoom(UI.Instance.RoomName, new RoomOptions { MaxPlayers = 2 });
            lobbyUI.UserCustumGameLobby();
            Debug.Log("CreateRoom");
        }
        else
        {
            Debug.Log("CreateRoom Failed");
        }
    }

    // 대기방 보여주기
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList) // 폐쇄방이 아니라면
            {
                if (!myList.Contains(roomList[i]))
                {
                    myList.Add(roomList[i]); //  현재 가지고 있는 룸 리스트에 없으면 추가
                    lobbyUI.CreateRoomButton(roomList[i]);
                }
                else
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];      // 있으면 갱신
                }
            }
            else if (myList.IndexOf(roomList[i]) != -1)
            {
                myList.RemoveAt(myList.IndexOf(roomList[i]));  // 폐쇄방이 돼버림
                lobbyUI.DeleteRoomButton(roomList[i]);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Instantiate(lobbyUI.UserNameTextPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }

    public void JoinRoom()
    {
        if (UI.Instance.RoomPassword != null)
        {
            PhotonNetwork.JoinRoom(UI.Instance.RoomName + "_" + UI.Instance.RoomPassword);
        }
        else
        {
            PhotonNetwork.JoinRoom(UI.Instance.RoomName);
        }
    }

    public void ClickRoom(string roomNameText)
    {
        lobbyUI.UserCustumGameLobby();
        PhotonNetwork.JoinRoom(roomNameText);
    }

    public void StartGame()
    {

        PhotonNetwork.LoadLevel("Howling Abyss");
    }
    
}
