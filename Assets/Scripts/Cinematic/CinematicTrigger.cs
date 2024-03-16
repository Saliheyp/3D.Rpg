using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
public class CinematicTrigger : MonoBehaviour
{
    bool alreadyTriggered = false;
    void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered == false && other.gameObject.tag == "Player")
        {
            alreadyTriggered = true;
            GetComponent<PlayableDirector>().Play();
            GetComponent<BoxCollider>().enabled = false;
        }

    }
}
}
