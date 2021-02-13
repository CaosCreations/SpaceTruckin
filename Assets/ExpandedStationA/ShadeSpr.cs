using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeSpr : MonoBehaviour {

	 public Renderer renderer;
	 public bool shadowOnly;

	// Use this for initialization
	void Start () {

		if (renderer == null) Debug.Log("Renderer is empty"); 
			GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On; 
			GetComponent<Renderer>().receiveShadows = true;
			//TESTE
			if(shadowOnly==true)
			{GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;}

			


			
		
	}
	
	// Update is called once per frame
	

	
}
