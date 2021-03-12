using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmEffect : MonoBehaviour
{
    const float effectDuration = 1.0f;
    float elapsedTime = 0.0f;
    const float speed = 1.5f;
    GameObject Creater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > effectDuration)
        {
            Object.Destroy(this.gameObject);
        }

        this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, Creater.transform.position,
            speed * Time.deltaTime);
    }



    public void initialize(GameObject creater)
    {
        this.Creater = creater;
    }
}
