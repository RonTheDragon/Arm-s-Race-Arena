using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ParticleDamage : MonoBehaviour
{
    [HideInInspector] public float Damage;
    [HideInInspector] public PhotonView ShooterPV;
    private void OnParticleCollision(GameObject other)
    {
        if (ShooterPV.IsMine) 
        {
                Heath PV = other.GetComponent<Heath>();
                if (PV != null)
                {
                    PV.TakingDamage(Damage,ShooterPV.ViewID);
                }
            
        }
    }
}
