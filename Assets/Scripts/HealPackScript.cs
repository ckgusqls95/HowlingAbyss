using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackScript : MonoBehaviour
{
    ParticleSystem ps;
    public GameObject healOnce;
    float CoolTime;
    float ElapsedTime;
    BoxCollider Boxcollider;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Boxcollider = GetComponent<BoxCollider>();
        CoolTime = 5.0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (ElapsedTime >= 0.0f)
        {
            ElapsedTime -= Time.deltaTime;
            if(ElapsedTime < 0.0f)
            {
                ElapsedTime = 0.0f;
                Boxcollider.enabled = true;
                ps.Play();
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player") && ps.isPlaying)
        {
            Boxcollider.enabled = false;
            ps.Stop();
            GameObject healObj =  Instantiate(healOnce,Vector3.zero,Quaternion.identity,other.transform);
            ElapsedTime = CoolTime;
            healObj.transform.localPosition = Vector3.zero;
            healObj.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            // 힐팩 이펙트 
            //

        }
    }

}
