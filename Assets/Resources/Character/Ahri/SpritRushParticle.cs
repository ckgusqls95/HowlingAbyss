using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using Photon.Pun;
public class SpritRushParticle : MonoBehaviour
{
    const float flyspeed = 5.0f;
    float duration = 0.3f;
    PlayerController player;
    [SerializeField]
    private GameObject essensefire;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    private void Start()
    {
        player = this.transform.parent.GetComponent<PlayerController>();        
    }
    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0.0f)
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position,5.0f,Vector3.up,0.0f);

            foreach (RaycastHit hit in hits)
            {
                if (gameObject == hit.transform.gameObject) continue;
                if (hit.transform.CompareTag("particle")) continue;
                if (hit.transform.CompareTag(this.transform.parent.CompareTag("Red") ? "Blue" : "Red"))
                {
                    if (hit.transform.TryGetComponent<Units>(out Units script))
                    {
                        if(script.UnitTag != UnitsTag.Nexus &&
                            script.UnitTag != UnitsTag.Turret)
                        {
                            GameObject obj = PhotonNetwork.Instantiate("Character/Ahri/R/firesessencebolts", transform.position,Quaternion.identity);
                            obj.GetComponent<FireEsseneBolt>().Init(transform.parent.gameObject, hit.transform.gameObject);
                            break;
                        }
                    }
                }
            }

            Object.Destroy(this.gameObject);
        }
        Vector3 targetposition = player.targetpos;
        targetposition.y = transform.parent.position.y;
        
        transform.parent.position = Vector3.MoveTowards(transform.position,targetposition, flyspeed * Time.deltaTime);
    }
}
