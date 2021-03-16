using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempestParticle : MonoBehaviour
{
    #region member
    ParticleSystem[] psArray;
    private SonicWave parent;
    float elapsedTime;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        psArray = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f)
        {
            Object.Destroy(this.gameObject);
        }
    }
    public void init(GameObject parentObject)
    {
        parent = parentObject.GetComponent<SonicWave>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!parent.transform.CompareTag(other.transform.tag))
        {
            Debug.Log("Tempest Hit Enemy");
        }
    }
}