using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string spawnPointName = PlayerPrefs.GetString("spawnPoint", "");
        if (spawnPointName != "")
        {
            GameObject spawnPoint = GameObject.Find(spawnPointName);
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (spawnPoint != null && player != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
