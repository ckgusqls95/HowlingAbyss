using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnce : MonoBehaviour
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.isStopped)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
