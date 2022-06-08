using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Heath : MonoBehaviour
{
    [SerializeField] float MaxHp;
    float Hp;
    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    [PunRPC]
    void TakeDamage(float Damage)
    {
        Hp -= Damage;
        if (Hp <= 0 && PV.IsMine)
        {
            GameManager.Instance.SpawnPlayer(GetComponent<PlayerManager>().Color);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
