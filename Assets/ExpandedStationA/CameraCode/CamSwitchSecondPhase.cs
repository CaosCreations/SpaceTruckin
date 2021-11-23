using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitchSecondPhase : MonoBehaviour {

	[SerializeField] private CamAnimHandle camAnimHandle;

	public void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			camAnimHandle.SwitchCamera(CameraSwitches.Default);
		}
	}
}
