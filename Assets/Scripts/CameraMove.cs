using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private float dist = 7.0f;

    private float height = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - (1 * Vector3.forward * dist) + (Vector3.up * height);
        transform.LookAt(target);
    }
}
