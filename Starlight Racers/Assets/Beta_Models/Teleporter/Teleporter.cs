using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject teleporterEffect;
    public GameObject teleporterEffect2;

    private bool teleportActive;

    private string teleporterDialogue;

    private bool inTeleport;
    // Start is called before the first frame update
    void Start()
    {
        teleporterDialogue = "Are you sure that you want to begin to the race?";
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
            var effect = new GameObject();
            
            List<GameObject> teleporterEffects = new List<GameObject>();

            for (int i = 0; i < 5; i++)
            {
                var rotation = Quaternion.Euler(90, 0, 0);

                if (MenuManager.RaceCount > 3)
                {
                    effect = Instantiate(teleporterEffect2, spawnPos, rotation);
                }
                else
                {
                    effect = Instantiate(teleporterEffect, spawnPos, rotation);
                }
                
                teleporterEffects.Add(effect);
                yield return new WaitForSeconds(0.2f);
            }

            foreach (var effects in teleporterEffects)
            {
                Destroy(effects);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            inTeleport = true;

            if (inTeleport)
            {
                if (MenuManager.RaceCount < 3)
                {
                    DialogueManager.inDialogue = true;
                    DialogueManager.id = 0;
                    DialogueManager.currentDialogue = new Dialogue("Teleporter", teleporterDialogue, Dialogue.DialogueType.Question);
                }
                else if(MenuManager.RaceCount == 3)
                {
                    teleporterDialogue = "Are you ready to begin the final race?";
                    DialogueManager.inDialogue = true;
                    DialogueManager.id = 0;
                    DialogueManager.currentDialogue = new Dialogue("Teleporter", teleporterDialogue, Dialogue.DialogueType.Question);
                }
                else
                {
                    teleporterDialogue = "Are you ready to defeat the boss?";
                    DialogueManager.inDialogue = true;
                    DialogueManager.id = 1;
                    DialogueManager.currentDialogue = new Dialogue("Teleporter", teleporterDialogue, Dialogue.DialogueType.Question);
                    BossCanvas.GameOver = false;
                }
                
            }
            
        }
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        inTeleport = false;
    }
}
