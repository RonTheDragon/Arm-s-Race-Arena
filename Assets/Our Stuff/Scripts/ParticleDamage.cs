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
            Heath HP = other.GetComponent<Heath>();
            if (HP != null)
            {
                PhotonView PV = HP.PV;
                if (PV != null)
                {
                    if (HP.Hp <= Damage && !HP.AlreadyDead)
                    {
                        HP.AlreadyDead = true;
                        ShooterPV.RPC("AddKill", RpcTarget.All,1);
                    }
                    PV.RPC("TakeDamage", RpcTarget.All, Damage);
                }
            }
        }
    }
}
