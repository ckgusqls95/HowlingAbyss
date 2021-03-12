using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
public class range_fireball : MonoBehaviour
{
    ParticleSystem fireball;
    
    
    [SerializeField]
    private GameObject Target;
    private Units TargetUnit;
    private Units Parent;
    public float speed;
    private void Awake()
    {
        fireball = GetComponent<ParticleSystem>();
        speed = 10.0f;
    }
    void Start()
    {
        fireball.Play();
    }

    void Update()
    {
        if (fireball.isPlaying && Target)
        {
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Target.transform.position, step);
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[fireball.main.maxParticles];
            int numParticlesAlive = fireball.GetParticles(particles);
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].position =
                    Vector3.MoveTowards(particles[i].position, Target.transform.position, step);
            }

            fireball.SetParticles(particles, numParticlesAlive);
        }

        if (Target == null || TargetUnit.isDeath)
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
        fireball.Play();
    }

    public void ParticleStop()
    {
        if(fireball.isPlaying)
        {
            fireball.Stop();

            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[fireball.main.maxParticles];
            int numParticlesAlive = fireball.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.remainingLifetime = 0.0f;
                particles[i] = p;
            }

            fireball.SetParticles(particles, numParticlesAlive);
        }
        Object.Destroy(this.gameObject);
    }


    public void init(GameObject parent,GameObject target)
    {
        Parent = parent.GetComponent<Units>();
        Target = target;
        TargetUnit = Target.GetComponent<Units>();
    }
}
