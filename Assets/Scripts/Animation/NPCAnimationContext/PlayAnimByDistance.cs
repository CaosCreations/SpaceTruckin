using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimByDistance : MonoBehaviour
{
    public float range;
    private Transform player;
    public Animator myAnimator;
    private bool animationStarted=false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= range && animationStarted==false)
        {
           // if (!GetComponent<Animation>().isPlaying) GetComponent<Animation>().Play();
           myAnimator.Play("Base Layer.birb001flyLeft",0,0);
           animationStarted=true;

           
        }
       // else
       // {
            //if (GetComponent<Animation>().isPlaying) GetComponent<Animation>().Stop();
       // }
    }
}
