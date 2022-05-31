using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0,5,0), Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
