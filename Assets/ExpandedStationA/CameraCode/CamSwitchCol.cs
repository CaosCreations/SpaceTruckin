using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchCol : MonoBehaviour 
{
	[SerializeField] private CameraSwitches camSwitch;

	[SerializeField] private CamAnimHandle camAnimHandle;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			camAnimHandle.SwitchCamera(camSwitch);
		}
	}
}
