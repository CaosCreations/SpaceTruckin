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


		


		if(camName=="MainHall")
		{
			myCurrentCam=0;
		}

		
		if(camName=="WorkShopCam")
		{
			myCurrentCam=1;
		}

		

			if(camName=="BarCam")
		{
			myCurrentCam=3;
		}


		
			if(camName=="DineCam")
		{
			myCurrentCam=4;
		}




			if(camName=="HangarEdge")
		{
			myCurrentCam=5;
		}

			if(camName=="HangarCam")
		{
			myCurrentCam=6;
		}




		
		if(camName=="LoungeUndOffice")
		{
			myCurrentCam=10;
		}

		
		if(camName=="CamToOuterCircle")
		{
			//20to30 for habitat ring
			myCurrentCam=20;
		}





		
		if(camName=="RefugeeUnder02")
		{
			//30to40 for refuggecamp
			myCurrentCam=30;
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
