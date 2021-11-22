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


	// Use this for initialization
	private void Start () 
	{
		cTrans = GetComponent<Transform>();
		anim = GetComponent<Animator>();
		anim.SetBool("TopCam",true);
	}

	// Update is called once per frame
	private void Update () 
	{
		anim.SetInteger("MYCURRENTCAM",camadress);

		if (CamSwitchCol.isTouchingCanChange == true)
		{
			anim.SetBool("AnglePov", true);
			anim.SetBool("TopCam", false);
			camadress = CamSwitchCol.myCurrentCam;
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
