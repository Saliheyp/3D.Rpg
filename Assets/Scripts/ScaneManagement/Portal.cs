using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A,B,C,D,E
    }
    [SerializeField] int sceneToLoad = 1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float fadeWaitTime = 1f;


    [SerializeField] DestinationIdentifier destination;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("if de");
            StartCoroutine(Transition());
        }
        {

        }
    }   

    IEnumerator Transition()
    {
        DontDestroyOnLoad(gameObject);
        
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        Portal otherPortal = GetOtherPortal();

        UpdatePlayer(otherPortal);

        yield return  new WaitForSeconds (fadeWaitTime);
        yield return fader.FadeIn(fadeInTime);

        Destroy(gameObject);
    }

    private Portal GetOtherPortal()
    {
        foreach(var portal in FindObjectsOfType<Portal>())
        {
            if (portal == this)
            {
                continue;
            }
            if (portal.destination != destination) continue;

            return portal;
        }
        return null;
        
    }
    private void UpdatePlayer (Portal portal) 
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.transform.position = portal.spawnPoint.position;
        player.transform.rotation = portal.spawnPoint.rotation;
        player.GetComponent<NavMeshAgent>().enabled = true;

    }
}
}
