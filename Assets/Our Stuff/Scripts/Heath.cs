using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Heath : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    [HideInInspector] public PhotonView PV;
    [HideInInspector] public PhotonView Attacker;
    [HideInInspector] public Player AttackingPlayer;
    PlayerManager PM;
    [HideInInspector] public bool AlreadyDead;

    float TakeDamageCooldown;
    float StoredDamage;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        Hp = MaxHp;
        PM = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 && !AlreadyDead)
        {
            AlreadyDead = true;
            if (PV.IsMine)
            {
                Death();
            } 
        }

        if (TakeDamageCooldown > 0)
        {
            TakeDamageCooldown -= Time.deltaTime;
        }
        else
        {
            if (StoredDamage > 0 && Attacker!=null)
            {
                PV.RPC("TakeDamage", RpcTarget.All, StoredDamage, Attacker.ViewID);
                StoredDamage = 0;
            }
        }
    }
    [PunRPC]
    void TakeDamage(float Damage, int DamagerId)
    {
        Attacker = PhotonNetwork.GetPhotonView(DamagerId);
        AttackingPlayer = Attacker.Owner;
        Debug.Log(DamagerId);
      //  if (!PhotonNetwork.GetPhotonView(DamagerId).IsMine && Attacker != null)
            
        Hp -= Damage;
        if (Hp <= 0 && !AlreadyDead)
        {
            AlreadyDead = true;
            if (PV.IsMine)
            {
                Death();
            }
        }
    }

    public void TakingDamage(float Damage, int id)
    {
        Attacker = PhotonNetwork.GetPhotonView(id);
        AttackingPlayer = Attacker.Owner;
       // Hp -= Damage;
        if (TakeDamageCooldown > 0)
        {
            StoredDamage += Damage;
        }
        else
        {
            StoredDamage = Damage;
            TakeDamageCooldown = 0.5f;
        }
    }

    void Death()
    {
        //if (Attacker != null)
        //{
        //    Attacker.GetComponent<PlayerManager>().AddKill();
        //}
        if (AttackingPlayer != null)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Kills", (int)AttackingPlayer.CustomProperties["Kills"] + 1);
            AttackingPlayer.SetCustomProperties(hash);
        }

        GameManager.Instance.SpawnPlayer();
        PhotonNetwork.Destroy(gameObject);
    }
}
