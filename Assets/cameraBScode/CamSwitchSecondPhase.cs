using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchSecondPhase : MonoBehaviour {


	public static bool isTouchingCanSecChange;
	public int myCurrentCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		
	}

	
			public void OnTriggerEnter(Collider collider)
	{


		if (collider.tag == "Player")
		{
			isTouchingCanSecChange=true;
		
			


		}
		
		
		}
				public void OnTriggerExit(Collider collider)
	{


		if (collider.tag == "Player")
		{
			isTouchingCanSecChange=false;
		
			//myCurrentCam+=1;

			//else 
			//if(myCurrentCam==1){myCurrentCam-=1;}
			


		}
		
		
		}
}
