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
                PhotonView PV = other.GetComponent<PhotonView>();
                if (PV != null)
                {
                    PV.RPC("TakeDamage", RpcTarget.All, Damage,ShooterPV.ViewID);
                }
            
        }
    }
}
