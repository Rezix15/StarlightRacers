using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject teleporterEffect;

    private bool teleportActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        teleportActive = true;
        StartCoroutine(SpawnEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEffect()
    {
        while (teleportActive)
        {
            var position = transform.position;
            var spawnPos = new Vector3(position.x, position.y + 1, position.z);
            
            List<GameObject> teleporterEffects = new List<GameObject>();

            for (int i = 0; i < 5; i++)
            {
                var rotation = Quaternion.Euler(90, 0, 0);
                var effect = Instantiate(teleporterEffect, spawnPos, rotation);
                teleporterEffects.Add(effect);
                yield return new WaitForSeconds(0.2f);
            }

            foreach (var effect in teleporterEffects)
            {
                Destroy(effect);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("StarLightRacers_BetaTest");
        }
    }
}
