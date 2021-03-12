using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFoxFire : MonoBehaviour
{
    const float duration = 3.0f;
    float leftTime = 0.0f;

    Vector3 rotation = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //rotation.y += speed * Time.deltaTime;
        //this.transform.rotation = Quaternion.Euler(rotation);

        if (transform.childCount == 0)
        {
            Object.Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        leftTime += Time.deltaTime;
        if (leftTime > duration)
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void init(GameObject Parent)
    {
        this.transform.tag = Parent.transform.tag;
    }
}
