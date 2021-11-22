using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimHandle : MonoBehaviour {

	Transform cTrans;

	Animator anim;

	public static bool top_camHandler;

	public static bool angle_camHandler;

	public static bool new_camHandler;

	public int camadress;

	private Dictionary<CameraSwitches, string> cameraSwitchDictionary = new Dictionary<CameraSwitches, string>()
	{
		{CameraSwitches.WorkShop,AnimationConstants.WorkShopSwitchTriggerParameter},
		{CameraSwitches.Dinner,AnimationConstants.DinnerSwitchTriggerParameter},
		{CameraSwitches.Bar,AnimationConstants.BarSwitchTriggerParameter},
		{CameraSwitches.Yoga,AnimationConstants.YogaSwitchTriggerParameter},
		{CameraSwitches.HangarEdge,AnimationConstants.HangarEdgeSwitchTriggerParameter},
		{CameraSwitches.OfficeLounge,AnimationConstants.OfficeLoungetSwitchTriggerParameter},
		{CameraSwitches.Hangar,AnimationConstants.HangarSwitchTriggerParameter},
		{CameraSwitches.WashingStablishment,AnimationConstants.WashingStablishmentSwitchTriggerParameter},
		{CameraSwitches.AbandonedTank,AnimationConstants.AbandonedTankSwitchTriggerParameter},
		{CameraSwitches.SpacePort,AnimationConstants.SpacePortSwitchTriggerParameter},
		{CameraSwitches.RefugeCampI,AnimationConstants.RefugeCampISwitchTriggerParameter},
		{CameraSwitches.AbbandonedStorageRoom,AnimationConstants.AbbandonedStorageRoomSwitchTriggerParameter},
		{CameraSwitches.StationMainOfiice,AnimationConstants.StationMainOfiiceSwitchTriggerParameter},
		{CameraSwitches.AbbandonedTank,AnimationConstants.AbbandonedTankTriggerParameter},
		{CameraSwitches.NorthEastCorridor,AnimationConstants.NorthEastCorridorTriggerParameter},
		{CameraSwitches.PPHQ,AnimationConstants.PPHQTriggerParameter},
		{CameraSwitches.RefugeUnd01,AnimationConstants.RefugeUnd01SwitchTriggerParameter},
		{CameraSwitches.RefugeUnd02,AnimationConstants.RefugeUnd02SwitchTriggerParameter},
		{CameraSwitches.RefugeUnd03,AnimationConstants.RefugeUnd03SwitchTriggerParameter}
	};

	private void Start () 
	{
		cTrans = GetComponent<Transform>();
		anim = GetComponent<Animator>();
		anim.SetBool("TopCam",true);
	}

	private void switchCameraFirstPhase()
	{
		camadress = CamSwitchCol.myCurrentCam;
	}


	// Update is called once per frame
	private void Update () 
	{
		anim.SetInteger("MYCURRENTCAM",camadress);

		if (CamSwitchCol.isTouchingCanChange == true)
		{
			anim.SetBool("AnglePov", true);
			anim.SetBool("TopCam", false);
			
		}

		// anim.SetBool("AngleCam",true);

		if(CamSwitchSecondPhase.isTouchingCanSecChange==true)
		{
			anim.SetBool("TopCam",true);
			anim.SetBool("AnglePov",false);
			camadress=0;
		}

		// anim.SetBool("AngleCam",true);
	}
}

public enum CameraSwitches
{
	WorkShop,
	Dinner,
	Bar,
	Yoga,
	HangarEdge,
	OfficeLounge,
	Hangar,
	WashingStablishment,
	AbbandonedTank,
	SpacePort,
	RefugeCampI,
	AbbandonedStorageRoom,
	StationMainOfiice,
	AbandonedTank,
	NorthEastCorridor,
	PPHQ,
	RefugeUnd01,
	RefugeUnd02,
	RefugeUnd03
}
