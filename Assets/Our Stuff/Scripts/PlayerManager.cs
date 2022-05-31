using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    PhotonView view;
    bool firsttime = true;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(firsttime)
        {
            view = GetComponent<PhotonView>();
            firsttime = false;
            if (!view.IsMine)
            {
                GetComponent<FirstPersonMovement>().enabled = false;
                GetComponent<Jump>().enabled = false;
                GetComponent<Crouch>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

    }
}
