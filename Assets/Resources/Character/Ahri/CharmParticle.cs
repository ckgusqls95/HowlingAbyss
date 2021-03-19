using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class CharmParticle : MonoBehaviour
{
    private ParticleSystem ps;
    const float speed = 5.0f;
    float duration;
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

        Charm charm = parent.GetComponent<Charm>();
        
        switch(charm.CurrentLevel)
        {
            case 0:
                duration = 1.4f;
                break;
            case 1:
                duration = 1.55f;
                break;
            case 2:
                duration = 1.7f;
                break;
            case 3:
                duration = 1.85f;
                break;
            case 4:
                duration = 2.0f;
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        if (other.CompareTag(parent.transform.tag == "Red" ? "Blue" : "Red"))
        {
            Vector3 pos = new Vector3(0, 0, 0);
            GameObject Effecter = Instantiate(Effect,pos, Quaternion.identity, other.transform);
            SoundManager.instance.PlaySE("SFX_charmHit", Effecter);
            Effecter.GetComponent<CharmEffect>().initialize(parent);
            Charm charm = parent.GetComponent<Charm>();
            Units unit = parent.GetComponent<Units>();

            float Damage = unit.Attack(AttackType.AP_SKILL, charm.skillFactor, charm.LevelperValues[charm.CurrentLevel].addDamage);
            float Suffer = 0.0f;
            if (other.TryGetComponent<Units>(out var script))
            {
                Suffer = script.hit(AttackType.AP_SKILL, Damage,parent.GetComponent<Ahri>(),unit.UnitStatus.magicResist);
            }
            Debug.Log("Damage = " + Damage + " / " + "Suffer = " + Suffer);
        }
        Object.Destroy(this.gameObject);            
    }
}
