using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
public class PersistantObjectspawner : MonoBehaviour
{
    [SerializeField] GameObject persistantGameObjectPrefab;

    static bool hasSpawned = false;
    
    private void Awake() 
    {
        if (hasSpawned) return;
        SpawnPersistentObject();
         hasSpawned = hasSpawned= true;
        
    }
    public void SpawnPersistentObject () 
    {
        GameObject persistentObject = Instantiate(persistantGameObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}


}

