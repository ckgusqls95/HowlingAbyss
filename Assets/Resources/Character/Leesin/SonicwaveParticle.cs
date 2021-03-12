using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicwaveParticle : MonoBehaviour
{
    #region member
    ParticleSystem[] psArray;
    private Vector3 dir;
    private float speed = 5.0f;
    private SonicWave parent;
    private Vector3 startpos;
    private Collider col;
    #endregion

    private void Awake()
    {
        psArray = GetComponentsInChildren<ParticleSystem>();
        col = GetComponent<Collider>();
        foreach(ParticleSystem ps in psArray)
        {
            ps.Play();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(startpos,this.transform.position) >= 100.0f)
        {

        }
        else
        {
            col.transform.position += dir * Time.deltaTime * speed;            
        }

    }

    public void init(GameObject parentObject)
    {
        startpos = this.transform.position;
        dir = parentObject.transform.forward.normalized;
        parent = parentObject.GetComponent<SonicWave>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(parent)
        {
            if(!parent.transform.CompareTag(other.transform.tag))
            {
                parent.HitObject = other.transform.gameObject;
                Object.Destroy(this.gameObject);
            }

        }
    }
}
