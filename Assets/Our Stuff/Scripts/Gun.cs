using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviour
{
    [SerializeField] float Damage;
    [SerializeField] float Cooldown = 0.1f;
    float _cooldown;
    ParticleSystem particle;
    ParticleDamage particleDamage;


    private void Awake()
    {
       particle = transform.GetChild(0).GetComponent<ParticleSystem>();
       particleDamage = transform.GetChild(0).GetComponent<ParticleDamage>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown > 0) { _cooldown -= Time.deltaTime; }
    }

    public virtual void Shoot(Vector3 AimAt)
    {
        if (_cooldown <= 0)
        {
            if (AimAt == Vector3.zero)
            {
                particle.transform.localRotation = Quaternion.identity;
            }
            else
            {
                particle.transform.LookAt(AimAt);
            }
            particleDamage.Damage = Damage;
            _cooldown = Cooldown;
            particle.Emit(1);
        }
    }
}
