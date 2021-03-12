using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmParticle : MonoBehaviour
{
    private ParticleSystem ps;
    const float speed = 5.0f;
    const float duration = 1.0f;
    private float elaspedTime = 0.0f;
    private Vector3 direction;
    private Collider col;
    private GameObject parent;

    [SerializeField]
    private GameObject Effect;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        col = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (col)
        {
            col.transform.position += direction * speed * Time.deltaTime;

        }
    }

    private void FixedUpdate()
    {
        elaspedTime += Time.deltaTime;

        if (elaspedTime > duration)
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void init(GameObject obj)
    {
        parent = obj;

        direction = obj.transform.forward;
        direction = direction.normalized;
        ps.startRotation3D = obj.transform.rotation.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(parent.transform.tag == "Red" ? "Blue" : "Red"))
        {
            Vector3 pos = new Vector3(0, 0, 0);
            GameObject Effecter = Instantiate(Effect,pos, Quaternion.identity, other.transform);
            Effecter.GetComponent<CharmEffect>().initialize(parent);
            Object.Destroy(this.gameObject);            
        }
    }
}
