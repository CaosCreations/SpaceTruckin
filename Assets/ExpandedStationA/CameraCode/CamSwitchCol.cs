using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchCol : MonoBehaviour {

	public static bool isTouchingCanChange;
	public static int myCurrentCam;
	[SerializeField] private AnimationConstants.CameraSwitches camName;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			isTouchingCanChange=true;

			//else 
			//if(myCurrentCam==0){myCurrentCam+=1;}
			//	else if(myCurrentCam==1){myCurrentCam-=1;}

			/*
			if(camName=="WorkShopCam")
			{
				myCurrentCam=1;
			}

			if(camName=="DinerCam")
			{
				myCurrentCam=2;
			}

			if(camName=="BarCam")
			{
				myCurrentCam=3;
			}

			if(camName=="YogaCam")
			{
				myCurrentCam=4;
			}

			if(camName=="HangarEdge")
			{
				myCurrentCam=5;
			}

			if(camName=="officeLounge")
			{
				myCurrentCam=6;
			}
		
			if(camName=="Hangarcam")
			{
				myCurrentCam=7;
			}

			if(camName=="WashingStablishment")
			{
				myCurrentCam=8;
			}

			if(camName=="abbandonedTank")
			{
				myCurrentCam=9;
			}

			if(camName=="SpacePort")
			{
				myCurrentCam=10;
			}

			if(camName=="RefugeCampI")
			{
				myCurrentCam=11;
			}

			if(camName=="abbandonedStorageRoom")
			{
				myCurrentCam=12;
			}

			if(camName=="StationMainOfiice")
			{
				myCurrentCam=13;
			}

			if(camName=="AbandonedTank")
			{
				myCurrentCam=14;
			}

			if(camName=="northEastCorridor")
			{
				myCurrentCam=15;
			}

			if(camName=="PPHQ")
			{
				myCurrentCam=16;
			}

			if(camName=="RefugeUnd01")
			{
				myCurrentCam=20;
			}

			if(camName=="RefugeUnd02")
			{
				myCurrentCam=21;
			}

			if(camName=="RefugeUnd03")
			{
				myCurrentCam=22;
			}

			*/
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
