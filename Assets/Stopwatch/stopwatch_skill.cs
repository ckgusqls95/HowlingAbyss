using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopwatch_skill : MonoBehaviour
{

    private float MaintainTime;
    private float ElaspedTime;
    private Material Original_mtrl;

    public Material Gold_mtrl;
    public GameObject particles;
    private GameObject InstanceParticle;

    private void Awake()
    {
        MaintainTime = 2.5f;   
    }

    void Update()
    {
        if(Try())
        {
            Play();
        }
    }

    private void FixedUpdate()
    {
        if(ElaspedTime > 0.0f)
        {
            ElaspedTime -= Time.deltaTime;
            if(ElaspedTime <= 0.001f)
            {
                ElaspedTime = 0.0f;
                GetComponent<Renderer>().material = Original_mtrl;
                InstanceParticle.GetComponent<effect_stop>().StopParticle();
                
                if(InstanceParticle)
                {
                    InstanceParticle = null;
                }
            }
        }
    }

    bool Try()
    {
        if(Input.GetKey(KeyCode.Alpha1) && ElaspedTime <= 0.0f)
        {
            return true;
        }

        return false;
    }

    void Play()
    {
        InstanceParticle = Instantiate(particles,Vector3.zero,Quaternion.identity);
        InstanceParticle.transform.parent = this.transform;

        InstanceParticle.transform.position = this.transform.position;
        InstanceParticle.transform.localRotation = Quaternion.identity;
        ElaspedTime = MaintainTime;



        // renderer
        Renderer renderer = GetComponent<Renderer>();
        Original_mtrl = renderer.material;
        renderer.material = Gold_mtrl;
    }

    void Stop()
    {

    }
}
