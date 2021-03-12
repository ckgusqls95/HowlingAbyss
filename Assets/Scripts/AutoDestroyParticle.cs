using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    ParticleSystem[] psArray;

    private void Awake()
    {
        psArray = GetComponentsInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        foreach (ParticleSystem item in psArray)
        {
            if(!item.IsAlive())
            {
                Object.Destroy(this.gameObject);
            }
        }
    }
}
