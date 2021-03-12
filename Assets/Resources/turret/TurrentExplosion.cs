using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentExplosion : MonoBehaviour
{
    ParticleSystem[] ParticleSystems;

    private void Awake()
    {
        ParticleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
      if(!ParticleSystems[0].IsAlive())
        {
            Object.Destroy(gameObject);
        }
    }

    

}
