using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_stop : MonoBehaviour
{
    ParticleSystem[] Systems;
    // Start is called before the first frame update
    void Start()
    {
        int childCount = this.transform.childCount;
        Systems = new ParticleSystem[childCount];
        int cnt = 0;
        for (int i = 0; i < childCount; i++)
        {
            ParticleSystem ps = this.transform.GetChild(i).GetComponent<ParticleSystem>(); 
            if(ps)
            {
                Systems[cnt++] = ps;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StopParticle()
    {
        for (int i = 0; i < Systems.Length; i++)
        {
            Systems[i].Stop();
        }

        Object.Destroy(this.gameObject);
    }

}



