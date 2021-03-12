using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritRushParticle : MonoBehaviour
{
    const float flyspeed = 5.0f;
    float duration = 0.5f;
    PlayerController player;

    // Start is called before the first frame update
    private void Awake()
    {
        player = this.transform.parent.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0.0f)
        {
            Object.Destroy(this.gameObject);
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position,player.targetpos, flyspeed * Time.deltaTime);
    }
}
