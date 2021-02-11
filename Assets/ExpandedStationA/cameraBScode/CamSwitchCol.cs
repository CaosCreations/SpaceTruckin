using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchCol : MonoBehaviour {

	public static bool isTouchingCanChange;
	public static int myCurrentCam;
	public string camName;
	

	// Use this for initialization
	void Start () 
	{

		
	}
	
	// Update is called once per frame
	void Update () 
	{



		
	}



			public void OnTriggerEnter(Collider collider)
	{


		if (collider.tag == "Player")
		{
			isTouchingCanChange=true;
		
			

			//else 
			//if(myCurrentCam==0){myCurrentCam+=1;}
		//	else if(myCurrentCam==1){myCurrentCam-=1;}


		
		if(camName=="WorkShopCam")
		{
			myCurrentCam=1;
		}

		

			if(camName=="BarCam")
		{
			myCurrentCam=3;
		}



			if(camName=="HangarEdge")
		{
			myCurrentCam=5;
		}

		
			


		}
		
		
		}

		
				public void OnTriggerExit(Collider collider)
	{


		if (collider.tag == "Player")
		{
			isTouchingCanChange=false;
		
		
			//myCurrentCam+=1;
			//if(myCurrentCam==0){myCurrentCam+=1;}


			//else 
			//if(myCurrentCam==1){myCurrentCam-=1;}
			


		}
		
		
		}



}
