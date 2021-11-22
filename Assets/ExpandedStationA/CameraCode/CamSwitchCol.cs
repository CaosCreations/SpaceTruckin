using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchCol : MonoBehaviour {

	public static bool isTouchingCanChange;
	[SerializeField] private CameraSwitches camSwitch;

	[SerializeField] private CamAnimHandle camAnimHandle;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			isTouchingCanChange=true;

			camAnimHandle.SwitchCamera(camSwitch);
		}
	}

		
	public void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Player")
		{
			isTouchingCanChange=false;
		}
	}
}
