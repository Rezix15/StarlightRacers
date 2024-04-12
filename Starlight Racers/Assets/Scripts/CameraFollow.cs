using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineFreeLook FreeLook;
    public enum PlayerTags
    {
        PlayerRacer,
        Player
    }

    [SerializeField] private Transform targetObj;


    public PlayerTags followTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        FreeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        targetObj = GameObject.FindWithTag(followTarget.ToString()).transform;
        FreeLook.Follow = targetObj;
        FreeLook.LookAt = targetObj;
    }
}
