using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;

    private float speed;

    private bool isMoving;

    private int isMovingHash = Animator.StringToHash("isMoving");
    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    //Checks if the object is moving and update the animation accordingly
    void Update()
    {
        anim.SetBool(isMovingHash, transform.hasChanged);
    }
}
