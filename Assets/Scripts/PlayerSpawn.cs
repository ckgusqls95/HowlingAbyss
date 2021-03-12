using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject Leesin;
    [SerializeField]
    private GameObject Ahri;

    // Start is called before the first frame update
    void Start()
    {
        //if(GameManager.Instance.player.ChampionName == "Ahri")
        //{
        //    Instantiate(Ahri);
        //}
        //else if (GameManager.Instance.player.ChampionName == "LeeSin")
        //{
        //    Instantiate(Leesin);
        //}

        if(PhotonNetwork.IsMasterClient)
        {
            this.transform.position = new Vector3(114, 0, 113);
            Instantiate(Ahri, this.transform.position, this.transform.rotation,this.transform);
        }
        else 
        {
            this.transform.position = new Vector3(12, 0, 14);
            Instantiate(Leesin, this.transform.position, this.transform.rotation,this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
