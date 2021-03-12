using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class LobbySceneUI : MonoBehaviourPun
{
    TMP_Text createGameText;
    TMP_Text joinGameText;
    TMP_Text battleRecordText;

    private Button createGameButton;
    private Button joinGameButton;
    private Button battleRecordButton;

    private GameObject lastCanvas;

    [SerializeField]
    private GameObject roomButton;
    public GameObject RoomButton { get { return roomButton; } set { roomButton = value; } }
    [SerializeField]
    private GameObject userNameTextPrefab;
    public GameObject UserNameTextPrefab { get { return userNameTextPrefab; } set { userNameTextPrefab = value; } }

    private GameObject roomListPath;

    private Dictionary<RoomInfo, GameObject> RoomList = new Dictionary<RoomInfo, GameObject>();
    private List<GameObject> playerList = new List<GameObject>();

    public PhotonView PV;

    public List<GameObject> PlayerList { get { return playerList; } set { playerList = value; } }
    private void Awake()
    {
        UI.Instance.CreateRoomButton = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Create").Find("Button").Find("CreateRoom").GetComponent<Button>();
        UI.Instance.CreateRoomButton.onClick.AddListener(delegate { NetworkManager.Instance.CreateRoom(); });
        roomListPath = this.transform.Find("UserCustumGame Join").Find("RoomList").gameObject;
        //createGameText = FindObjectOfType<Canvas>().transform.Find("Common UI").Find("Text").Find("UserCustumGameCreate Text").GetComponent<TMP_Text>();
        //joinGameText = FindObjectOfType<Canvas>().transform.Find("Common UI").Find("Text").Find("UserCustumGameJoin Text").GetComponent<TMP_Text>();
        //battleRecordText = FindObjectOfType<Canvas>().transform.Find("Common UI").Find("Text").Find("Battle Records Text").GetComponent<TMP_Text>();

        lastCanvas = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Create").gameObject;
        NetworkManager.Instance.lobbyUI = this;
        NetworkManager.Instance.playerListContent = this.transform.Find("UserCustumGame Lobby").Find("PlayerList");
        NetworkManager.Instance.startGameButton = this.transform.Find("UserCustumGame Lobby").Find("Start Game").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UserCustumGameCreate()
    {
        lastCanvas.SetActive(false);
        lastCanvas = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Create").gameObject;
        lastCanvas.SetActive(true);
    }

    public void UserCustumGameLobby()
    {
        lastCanvas.SetActive(false);
        lastCanvas = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Lobby").gameObject;
        lastCanvas.SetActive(true);
    }

    public void UserCustumGameJoin()
    {
        lastCanvas.SetActive(false);
        lastCanvas = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Join").gameObject;
        lastCanvas.SetActive(true);
    }

    public void CreateRoomButton(RoomInfo roomInfo)
    {
        Vector3 buttonVec = new Vector3(this.transform.position.x - 470, this.transform.position.y + RoomList.Count * 50, 0);
        GameObject obj = Instantiate(roomButton, buttonVec, this.transform.rotation, roomListPath.transform);
        Button button = obj.GetComponent<Button>();
        Text textTemp = button.GetComponentInChildren<Text>();
        RoomList.Add(roomInfo, obj);
        textTemp.text = roomInfo.Name;
        button.onClick.AddListener(delegate { NetworkManager.Instance.ClickRoom(textTemp.text); });
    }

    public void DeleteRoomButton(RoomInfo roomInfo)
    {
        RoomList.Remove(roomInfo);
    }

    public void ExitPickRoom()
    {
        transform.Find("UserCustumGame Pick").gameObject.SetActive(false);
        transform.Find("Common UI").gameObject.SetActive(true);
        transform.Find("UserCustumGame Create").gameObject.SetActive(true);
    }

    public void InputRoomName(TMP_InputField inputRoomName)
    {
        UI.Instance.RoomName = inputRoomName.text;
    }

    public void InputRoomPassword(TMP_InputField inputRoomPassword)
    {
        UI.Instance.RoomPassword = inputRoomPassword.text;
    }

    public void EnteredNewPlayer(string newPlayerName)
    {
        Vector3 ghestPosition = userNameTextPrefab.transform.position + new Vector3(-150, 0, 0);
        //playerList[1] = PhotonNetwork.Instantiate(userNameTextPrefab, ghestPosition, userNameTextPrefab.transform.rotation);
    }

    public void LeftNewPlayer()
    {
        playerList.Remove(playerList[1]);
    }

    public void OnStartPickButton()
    {
        photonView.RPC("UserCustumGamePick", RpcTarget.All);
    }

    [PunRPC]
    public void UserCustumGamePick()
    {
        lastCanvas.SetActive(false);
        lastCanvas = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Pick").gameObject;
        transform.Find("Common UI").gameObject.SetActive(false);
        lastCanvas.SetActive(false);
        transform.Find("UserCustumGame Pick").gameObject.SetActive(true);
    }
}