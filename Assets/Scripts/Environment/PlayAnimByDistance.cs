using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimByDistance : MonoBehaviour
{
    public float range;
    public float respawDist;
    private Transform player;
    public int randomAnimState;

    public Animator myAnimator;
    private bool animationStarted = false;

    public bool zDistance;
    public float ooverallDist;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        randomAnimState = Random.Range(0, 5);
        //setting up RandomIddle
        if (randomAnimState == 0) { myAnimator.Play("Base Layer.birbIdle000", 0, 0); }
        if (randomAnimState == 1) { myAnimator.Play("Base Layer.birbIdle001", 0, 0); }
        if (randomAnimState == 2) { myAnimator.Play("Base Layer.birbIdle002", 0, 0); }
        if (randomAnimState == 3) { myAnimator.Play("Base Layer.birbIdle003", 0, 0); }
        if (randomAnimState == 4) { myAnimator.Play("Base Layer.birbIdle004", 0, 0); }
        if (randomAnimState == 5) { myAnimator.Play("Base Layer.birbIdle005", 0, 0); }






    }

    void Update()
    {

        //set overall dist
        ooverallDist = Vector3.Distance(player.transform.position, this.transform.position);


        //idk if vector distance is a good
        // I heard from a guy...that is not good for performance.

        if (player.transform.position.z < this.transform.position.z + 2 && player.transform.position.z > this.transform.position.z - 2)
        {
            // myAnimator.Play("Base Layer.birb001flysoutht",0,0);
            // animationStarted=true;   
            zDistance = true;

        }
        else { zDistance = false; }


        if (Vector3.Distance(player.position, transform.position) < range && animationStarted == false && zDistance == true)
        {



            if (player.transform.position.x > this.transform.position.x + 2 && animationStarted == false)
            {
                myAnimator.Play("Base Layer.birb001flyLeft", 0, 0);
                animationStarted = true;
                randomAnimState = -10;

            }

            if (player.transform.position.x < this.transform.position.x + 2 && animationStarted == false)
            {
                myAnimator.Play("Base Layer.birb001flyRight", 0, 0);
                animationStarted = true;
                randomAnimState = -10;

            }

        }


        //for Z?

        if (Vector3.Distance(player.position, transform.position) < range && animationStarted == false && zDistance == false)
        {



            if (player.transform.position.z > this.transform.position.z + 1 && animationStarted == false)
            {
                myAnimator.Play("Base Layer.birb001flysoutht", 0, 0);
                animationStarted = true;
                randomAnimState = -10;

            }

            if (player.transform.position.z < this.transform.position.z + 1 && animationStarted == false)
            {
                myAnimator.Play("Base Layer.birb001flynortht", 0, 0);
                animationStarted = true;
                randomAnimState = -10;

            }

        }


        if (ooverallDist > respawDist && animationStarted ==true) 
        {
            // Debug.Log("BIIRRDDD"); 
            myAnimator.Play("Base Layer.landFromNorth", 0, 0);
            animationStarted = false;
          



        }

        if (transform.localPosition==Vector3.zero && randomAnimState==-10 && animationStarted == false)
        {
            randomAnimState = Random.Range(0, 5);
             Debug.Log("BIIRRDDD");
            //setting up RandomIddle
            if (randomAnimState == 0) { myAnimator.Play("Base Layer.birbIdle000", 0, 0); }
            if (randomAnimState == 1) { myAnimator.Play("Base Layer.birbIdle001", 0, 0); }
            if (randomAnimState == 2) { myAnimator.Play("Base Layer.birbIdle002", 0, 0); }
            if (randomAnimState == 3) { myAnimator.Play("Base Layer.birbIdle003", 0, 0); }
            if (randomAnimState == 4) { myAnimator.Play("Base Layer.birbIdle004", 0, 0); }
            if (randomAnimState == 5) { myAnimator.Play("Base Layer.birbIdle005", 0, 0); }


        }










    }
}
