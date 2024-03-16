using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Controller;
namespace RPG.Cinematic
{
public class CinematicControlRemover : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GetComponent<PlayableDirector>().played += DisableControl;
        GetComponent<PlayableDirector>().stopped += EnabledControl;

    }

    private void EnabledControl (PlayableDirector pd) {
        player.GetComponent<PlayerController>().enabled = true;
        print("Enabled Control");
        
    }

    private void DisableControl (PlayableDirector pd) 
    {
        player.GetComponent<PlayerController>().enabled = false;
        print("Disabled Control");
        
    }
}
}


