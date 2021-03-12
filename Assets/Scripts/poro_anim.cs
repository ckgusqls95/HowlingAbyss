using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poro_anim : MonoBehaviour
{
    private Animator animator;
    float exitTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > exitTime)
        {
            int rand = Random.Range(0,3);
            animator.SetInteger("num", rand);
        }
    }
}
