using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchSecondPhase : MonoBehaviour {

	public static bool isTouchingCanSecChange;

	[SerializeField] private CamAnimHandle camAnimHandle;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			camAnimHandle.SwitchCamera(CameraSwitches.Default);
			isTouchingCanSecChange =true;
		}
	}

	public void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Player")
		{
			isTouchingCanSecChange = false;
		}
	}
}
