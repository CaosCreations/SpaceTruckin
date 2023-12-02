using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFacingAnim : MonoBehaviour
{


    private Transform player;

    public Animator myAnimator;
    private bool animationStarted = false;
    public bool zDistance;
    public float distZ;
    public float distX;
    public float overallDist;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //set overall dist
        overallDist= Vector3.Distance(player.transform.position, this.transform.position);


        //change to OntriggerStay
        if (overallDist < 1f)
        {
            if (player.transform.position.z < this.transform.position.z + distZ && player.transform.position.z > this.transform.position.z - distZ)
        {
            // myAnimator.Play("Base Layer.npcStandUp",0,0);
            // animationStarted=true;   
            zDistance = true;

        }
        else { zDistance = false; }

        //start with a generic dostance


        if (animationStarted == false && zDistance == true)
        {

            if (zDistance == true)
            {

                if (player.transform.position.x >= this.transform.position.x + distX && animationStarted == false)
                {
                    myAnimator.Play("Base Layer.npcStandRight", 0, 0);
                    animationStarted = true;
                    Debug.Log("HelloRIGHT");


                }



            }

        }
    }
    }
}
