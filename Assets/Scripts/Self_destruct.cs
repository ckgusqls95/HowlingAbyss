using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using debug = UnityEngine.Debug;
public class Self_destruct : MonoBehaviour
{
    private Stopwatch stopwatch;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(stopwatch.ElapsedMilliseconds /1000 >= 10.0f )
        {
            debug.Log(stopwatch.ElapsedMilliseconds);
            Destroy(this.gameObject);
        }

    }
}
