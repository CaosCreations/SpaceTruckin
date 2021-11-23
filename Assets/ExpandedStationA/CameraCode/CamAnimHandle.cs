using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimHandle : MonoBehaviour 
{
	Animator anim;

	public static bool top_camHandler;

	public static bool angle_camHandler;

	public static bool new_camHandler;

	private Dictionary<CameraSwitches, string> cameraSwitchDictionary = new Dictionary<CameraSwitches, string>()
	{
		{CameraSwitches.Default ,AnimationConstants.DefaultSwitchTriggerParameter},
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
		anim = GetComponent<Animator>();
		SwitchCamera(CameraSwitches.Default);
	}

	public void SwitchCamera(CameraSwitches cameraSwitch)
	{
		anim.SetTrigger(cameraSwitchDictionary[cameraSwitch]);
	}
}

public enum CameraSwitches
{
	Default,
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
