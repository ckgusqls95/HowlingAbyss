using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private ChampionPrefabData ChampionPrefab;
    GameObject championObj;
    public Transform blueTeamSpawnTransform;
    public Transform redTeamSpawnTransform;
    private void Awake()
    {
        MatchChampion();
        //pV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void MatchChampion()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.ChampionName == "Ahri")
        {
            championObj = PhotonNetwork.Instantiate("Ahri Prefab", Vector3.zero, Quaternion.identity);
        }
        else if (GameManager.Instance.ChampionName == "LeeSin")
        {
            championObj = PhotonNetwork.Instantiate("Leesin Prefab", Vector3.zero, Quaternion.identity);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            championObj.transform.position = redTeamSpawnTransform.position;
            championObj.GetComponent<PlayerController>().tag = "Red";
        }
        else
        {
            championObj.transform.position = blueTeamSpawnTransform.position;
            championObj.GetComponent<PlayerController>().tag = "Blue";
        }
        
        Camera.main.GetComponent<CameraMove>().Target = championObj.transform;
        
        //else if (GameManager.Instance.ChampionName == "Lucian")
        //{
        //    PhotonNetwork.Instantiate("Lucian Prefab", Vector3.zero, Quaternion.identity);
        //}
    }

}
