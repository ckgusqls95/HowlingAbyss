using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class Siege_CannonBall : MonoBehaviour
{
    ParticleSystem[] CannonParticle;

    [SerializeField]
    private Units Target;
    private Units Parent;
    public float speed;

    private void Awake()
    {
        speed = 10.0f;
        CannonParticle = GetComponentsInChildren<ParticleSystem>();        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if((CannonParticle[0] && Target) || !Target.isDeath)
        {
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Target.transform.position, step);
        }


        if (Target == null || Target.isDeath)
        {
            ParticleStop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.transform.gameObject == Target)
        {
            float Damage = Parent.Attack(AttackType.MEELEE);
            float Suffer;
            if (other.TryGetComponent<Units>(out var script))
            {
                Suffer = script.hit(AttackType.MEELEE, Damage, Parent.UnitStatus.armorPenetration);
            }
            ParticleStop();
        }
    }

    public void Attack()
    {
        foreach (ParticleSystem ps in CannonParticle)
        {
            ps.Play();
        }
    }

    public void init(GameObject parent, GameObject target)
    {
        Parent = parent.GetComponent<Units>();
        Target = target.GetComponent<Units>();
    }


    public void ParticleStop()
    {
        if (CannonParticle[0].isPlaying)
        {
            foreach (ParticleSystem ps in CannonParticle)
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
                int numParticlesAlive = ps.GetParticles(particles);

                for (int i = 0; i < numParticlesAlive; i++)
                {
                    ParticleSystem.Particle p = particles[i];
                    p.remainingLifetime = 0.0f;
                    particles[i] = p;
                }   
                ps.SetParticles(particles, numParticlesAlive);
                ps.Stop();
            }
        }

        Object.Destroy(this.gameObject);
    }

}
