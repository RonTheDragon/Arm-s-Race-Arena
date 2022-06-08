using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Heath : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    [HideInInspector] public PhotonView PV;
    PlayerManager PM;
    [HideInInspector] public bool AlreadyDead;

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
        if (Hp <= 0)
        {
            if (PV.IsMine && !AlreadyDead)
            {
                Death();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
        [PunRPC]
        void TakeDamage(float Damage)
        {
            Hp -= Damage;
        }

        void Death()
        {
            AlreadyDead = true;
            GameManager.Instance.SpawnPlayer(PM.Kills);
            PhotonNetwork.Destroy(gameObject);
        }
}
