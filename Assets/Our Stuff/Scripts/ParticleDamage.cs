using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ParticleDamage : MonoBehaviour
{
    [HideInInspector] public float Damage;
    private void OnParticleCollision(GameObject other)
    {
        PhotonView PV = other.GetComponent<PhotonView>();
        if (PV!=null && PV.IsMine)
        {
                PV.RPC("TakeDamage", RpcTarget.All, Damage);
        }
    }
}
