using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimByDistance : MonoBehaviour
{
    public float range;
    private Transform player;
   // public float playerZ;
   // public float playerX;
   public float stDistance;

    public Animator myAnimator;
    private bool animationStarted=false;

    public bool zDistance;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        stDistance=  Vector3.Distance(player.position, transform.position);

        //idk if vector distance is a good
        // I heard from a guy...that is not good for performance.

           if(player.transform.position.z<this.transform.position.z+2 && player.transform.position.z>this.transform.position.z-2)
       {
           // myAnimator.Play("Base Layer.birb001flysoutht",0,0);
          // animationStarted=true;   
          zDistance=true;

       }else{ zDistance=false;}


        if (Vector3.Distance(player.position, transform.position) < range && animationStarted==false && zDistance==true) 
        {
         
      
     
       if(player.transform.position.x>this.transform.position.x+2 && animationStarted==false)
       {
         myAnimator.Play("Base Layer.birb001flyLeft",0,0);
           animationStarted=true;   

       }

           if(player.transform.position.x<this.transform.position.x+2 && animationStarted==false)
       {
         myAnimator.Play("Base Layer.birb001flyRight",0,0);
           animationStarted=true;   

       }

        }


//for Z?

   if (Vector3.Distance(player.position, transform.position) < range && animationStarted==false && zDistance==false) 
        {
         
      
     
       if(player.transform.position.z>this.transform.position.z+1 && animationStarted==false)
       {
         myAnimator.Play("Base Layer.birb001flysoutht",0,0);
           animationStarted=true;   

       }

           if(player.transform.position.z<this.transform.position.z+1 && animationStarted==false)
       {
         myAnimator.Play("Base Layer.birb001flynortht",0,0);
           animationStarted=true;   

       }

        }
       



    }
}
