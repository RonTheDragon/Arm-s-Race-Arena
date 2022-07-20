using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject MyCanvas;
    [SerializeField] TMP_Text MyKills;
    [SerializeField] Image MyHpBar;
    [SerializeField] GameObject TheirCanvas;
    [SerializeField] TMP_Text PlayerName;
    [SerializeField] TMP_Text ShowThemKills;
    [SerializeField] Image HpBar;
    [SerializeField] GameObject TheCamera;
    [SerializeField] Renderer mesh;
    PhotonView view;
    bool firsttime = true;
    Heath hp;
   // public int Kills;
    void Start()
    {
        TheCamera = transform.GetChild(0).gameObject;
        hp = GetComponent<Heath>();
    }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(firsttime)
        {
            
            firsttime = false;
            TheCamera.GetComponent<Zoom>().enabled = false;
            if (!view.IsMine)
            {
                MyCanvas.SetActive(false);
                TheirCanvas.SetActive(true);
                GetComponent<FirstPersonMovement>().enabled = false;
                GetComponent<Jump>().enabled = false;
                GetComponent<Crouch>().enabled = false;
                TheCamera.GetComponent<Camera>().enabled = false;
                TheCamera.GetComponent<FirstPersonLook>().enabled = false;
                PlayerName.text = view.Owner.NickName;
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                MyCanvas.SetActive(true);
                TheirCanvas.SetActive(false);
                TheCamera.GetComponent<AudioListener>().enabled = true;
                GameManager.Instance.TheCamera = TheCamera.GetComponent<Camera>();
            }
            UpdateKills();
            if (view.Owner.CustomProperties.ContainsKey("Kills"))
            {
                ShowThemKills.text = $"Kills: {(int)view.Owner.CustomProperties["Kills"]}";
                MyKills.text = $"Kills: {(int)view.Owner.CustomProperties["Kills"]}";
            }
            if (GameManager.Instance.colors.Count> view.Owner.ActorNumber)
            mesh.materials[0].color = GameManager.Instance.colors[view.Owner.ActorNumber-1].color;
        }

        if (!view.IsMine && GameManager.Instance.TheCamera != null)
        {
            TheirCanvas.transform.LookAt(GameManager.Instance.TheCamera.transform.position);
            HpBar.fillAmount = hp.Hp / hp.MaxHp;
        }
        if (view.IsMine)
        {
            MyHpBar.fillAmount = hp.Hp / hp.MaxHp;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    //public void AddKill()
    //{
    //    Hashtable hash = new Hashtable();
    //    //if (view.Owner.CustomProperties.ContainsKey("Kills"))
    //    //{
    //        hash.Add("Kills", (int)view.Owner.CustomProperties["Kills"] + 1);
    //    //}
    //    //    else
    //    //    {
    //    //        hash.Add("Kills", 0);
    //    //    }
    //    view.Owner.SetCustomProperties(hash);
    //}

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps != null && targetPlayer!=null)
        {
            if (targetPlayer == view.Owner && changedProps.ContainsKey("Kills"))
            {
                ShowThemKills.text = $"Kills: {(int)view.Owner.CustomProperties["Kills"]}";
                MyKills.text = $"Kills: {(int)view.Owner.CustomProperties["Kills"]}";
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateKills();
    }

    public void UpdateKills()
    {
        Hashtable hash = new Hashtable();
        if (view.Owner.CustomProperties.ContainsKey("Kills"))
        {
            hash.Add("Kills", (int)view.Owner.CustomProperties["Kills"]);
        }
        else
        {
            hash.Add("Kills", 0);
        }
        view.Owner.SetCustomProperties(hash);
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }
}
