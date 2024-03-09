using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject normalObj;
    public GameObject angryObj;

    public GameObject[] ghostCannons;

    private bool isSafe;
    private bool isAngry;

    private bool isAttackStateOn;

    public GameObject bossPortalObj;
    // Start is called before the first frame update
    void Start()
    {
        normalObj.SetActive(true);
        StartCoroutine(ToggleStates());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCannon();
    }

    //Transition between the states (Happy and Angry)
    IEnumerator ToggleStates()
    {
        while (true)
        {
            isSafe = true;
            isAngry = false;
            yield return new WaitForSeconds(10f);
            ToggleState(0);
            yield return new WaitForSeconds(10f);
            ToggleState(1);
        }

        //yield return new WaitForSeconds(0.1f);
    }
    
    //Attack State to spawn a random cannon through a portal to attack the player
    private void SpawnCannon()
    {
        if (isAngry && !isAttackStateOn)
        {
            var randPortalState = Random.Range(0, 6);

            if (randPortalState % 5 == 0 || randPortalState % 5 == 1 || randPortalState % 5 == 2)
            {
                var position = transform.position;
                var spawnState = Random.Range(0, 2);
                var spawnPos = new Vector3(0,0,0);
                switch (spawnState)
                {
                    case 0:
                    {
                        spawnPos = new Vector3(position.x + 150, position.y, position.z);
                        break;
                    }

                    case 1:
                    {
                        spawnPos = new Vector3(position.x - 150, position.y, position.z);
                        break;
                    }
                }

                Instantiate(bossPortalObj, spawnPos, Quaternion.Euler(0,90,0));
            }
            else if(randPortalState % 5 == 5)
            {
                var position = transform.position;
                var spawnPos1 = new Vector3(position.x + 150, position.y, position.z);
                var spawnPos2 = new Vector3(position.x - 150, position.y, position.z);
                
                Instantiate(bossPortalObj, spawnPos1, Quaternion.Euler(0,90,0));
                Instantiate(bossPortalObj, spawnPos2, Quaternion.Euler(0,90,0));
            }

            isAttackStateOn = true;
        }
    }


    private void ToggleState(int state)
    {
        switch (state)
        {
            case 0:
            {
                normalObj.SetActive(false);
                angryObj.SetActive(true);
                isAngry = true;
                isSafe = false;
                break;
            }

            case 1:
            {
                normalObj.SetActive(true);
                angryObj.SetActive(false);
                isAttackStateOn = false;
                break;
            }
        }
    }
    
}
