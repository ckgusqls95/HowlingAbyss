using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Nexus : MonoBehaviourPun, IPunObservable
{
    private PhotonView pv;
    #region minion

    [SerializeField]
    GameObject melee;

    [SerializeField]
    GameObject range;

    [SerializeField]
    GameObject Siege;

    #endregion

    private const float StartTime = 15.0f;  // 
    private const float CoolTime = 30.0f;   // 30 seconds;
    private const float SumonDelayTime = 0.5f;

    private float ElapsedTime; 
    private float FixedCoolTime;
    private static int wave;

    Vector3 spawnerpos;
    const string spawnername = "SumonPosition";

    [SerializeField]
    GameObject Icon;

    private void Awake()
    {
        GameObject.FindWithTag("MiniMap").GetComponent<MiniMapSystem>().Attach(this.transform.gameObject,Icon);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (string.Compare(child.name,spawnername) == 0)
            {
                spawnerpos = child.transform.position;
                break;
            }
        }

        ElapsedTime = 0.0f;
        FixedCoolTime = StartTime;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(pv.IsMine)
        {
            ElapsedTime += Time.deltaTime;
        }

        if (ElapsedTime % FixedCoolTime <= Time.deltaTime && ElapsedTime >= StartTime)
        {
            StartCoroutine(SumonMinion());
        }
    }
    // TODO: 미니언 생성 버그
    [PunRPC]
    IEnumerator SumonMinion()
    {
        const int eachWave = 2;

        int[] IndexMinion = {0,0,0,int.MaxValue,1,1,1};

        if (wave % eachWave  == 0 && wave != 0)
        {
            IndexMinion[3] = 2;
        }

        foreach (int index in IndexMinion)
        {
            GameObject minion;
            switch (index)
            {
                case 0:
                    if(this.transform.name == "Nexus_red")
                    {
                        minion = PhotonNetwork.Instantiate("ha_melee 1/Red_Melee", spawnerpos, Quaternion.identity);
                    }
                    else
                    {
                        minion = PhotonNetwork.Instantiate("ha_melee 1/Blue_Melee", spawnerpos, Quaternion.identity);
                    }
                    break;
                case 1:
                    if (this.transform.name == "Nexus_red")
                    {
                        minion = PhotonNetwork.Instantiate("ha_range/Red_Range", spawnerpos, Quaternion.identity);
                    }
                    else
                    {
                        minion = PhotonNetwork.Instantiate("ha_range/Blue_Range", spawnerpos, Quaternion.identity);
                    }
                    break;
                case 2:
                    if (this.transform.name == "Nexus_red")
                    {
                        minion = PhotonNetwork.Instantiate("chaosminionsiege/chaosSiege", spawnerpos, Quaternion.identity);
                    }
                    else
                    {
                        minion = PhotonNetwork.Instantiate("orderminion/OrderSiege", spawnerpos, Quaternion.identity);
                    }
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(SumonDelayTime);
        }

        if (wave == 0)
        {
            GameObject.Find("Announcer").GetComponent<Announcer>().Request(Announcer.RequestMenu.MinionSummon);
            ElapsedTime = CoolTime;
        }

        wave++;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(FixedCoolTime);
        }
    }
}
