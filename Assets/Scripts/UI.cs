using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    private string userID;
    private string userPassword;

    private string roomName;
    private string roomPassword;

    private static UI instance = null;

    private Button joinServerButton;
    private Button createRoomButton;
    private Button createInGame;

    public string UserID { get { return userID; } set { userID = value; } }
    public string UserPassword { get { return userPassword; } set { userPassword = value; } }
    public string RoomName { get { return roomName; } set { roomName = value; } }
    public string RoomPassword { get { return roomPassword; } set { roomPassword = value; } }

    public Button JoinServerButton { get { return joinServerButton; } set { joinServerButton = value; } }
    public Button CreateRoomButton { get { return createRoomButton; } set { createRoomButton = value; } }

    public Button CreateInGame { get { return createInGame; } set { createInGame = value; } }


    public static string nextScene;

    [SerializeField]
    Text progressText;

    private void Awake()
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

    public static UI Instance
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
        if (SceneManager.GetActiveScene().name == "LogIn")
        {
            joinServerButton = FindObjectOfType<Canvas>().transform.Find("LoginButton").GetComponent<Button>();
            joinServerButton.onClick.AddListener(delegate { LogInButtonOn(); });
            Screen.SetResolution(1280, 720, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "LogIn")
        {
            ButtonInteractable();

        }
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            createInGame = FindObjectOfType<Canvas>().transform.Find("UserCustumGame Pick").Find("Button").Find("GameStart").GetComponent<Button>();
            createInGame.onClick.AddListener(OnButtonCreateInGame);
        }
    }

    public void LogInButtonOn()
    {
        if (PlayerPrefs.GetString(userID, "Unknown") == "Unknown")
        {
            PlayerPrefs.SetString(userID, userPassword);
            NetworkManager.Instance.Connect();
        }
        else if (PlayerPrefs.GetString(userID) == userPassword)
        {
            Debug.Log("접속");
            NetworkManager.Instance.Connect();
        }
        else
        {
            // 비밀번호 틀림 팝업 호출
            Debug.Log("비밀번호 틀림");
        }
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
        Screen.SetResolution(1920, 1080, true);
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //op.allowSceneActivation = false;


        //progressText = FindObjectOfType<Canvas>().transform.Find("Host").GetComponentInChildren<Text>();
        //if (progressText)
        //{
        //    Debug.Log("ok");
        //}
        //else
        //    Debug.LogError("failed");

        //while (!op.isDone)
        //{
        //    yield return null;

        //    if (op.progress < 0.9f)
        //    {
        //        progressText.text = (op.progress * 100) + "%";
        //    }
        //    else if (op.progress == 1.0f)
        //    {
        //        op.allowSceneActivation = true;
        //        yield break;
        //    }
        //}
    }

    public void InputUserID(TMP_InputField inputUserID)
    {
        userID = inputUserID.text;
    }
    public void InputUserPassword(TMP_InputField inputUserPassword)
    {
        userPassword = inputUserPassword.text;
    }
    public void InputRoomName(TMP_InputField inputRoomName)
    {
        roomName = inputRoomName.text;
    }
    public void InputRoomPassword(TMP_InputField inputRoomPassword)
    {
        roomPassword = inputRoomPassword.text;
    }

    public void ButtonInteractable()
    {
        if (userID != null && userPassword != null)
        {
            FindObjectOfType<Canvas>().transform.Find("LoginButton").GetComponent<Button>().interactable = true;
            FindObjectOfType<Canvas>().transform.Find("LoginButton").Find("Login ON").gameObject.SetActive(true);
            FindObjectOfType<Canvas>().transform.Find("LoginButton").Find("Login OFF").gameObject.SetActive(false);
        }
        else
        {
            FindObjectOfType<Canvas>().transform.Find("LoginButton").GetComponent<Button>().interactable = false;
            FindObjectOfType<Canvas>().transform.Find("LoginButton").Find("Login ON").gameObject.SetActive(false);
            FindObjectOfType<Canvas>().transform.Find("LoginButton").Find("Login OFF").gameObject.SetActive(true);
        }
    }

    //public void SaveLoginState()
    //{
    //}
    public void OnButtonCreateInGame()
    {
        LoadScene("Howling Abyss");
    }



}
