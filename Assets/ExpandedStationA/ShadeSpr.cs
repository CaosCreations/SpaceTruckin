using UnityEngine;

public class ShadeSpr : MonoBehaviour 
{
	public Renderer _renderer;
	public bool shadowOnly;

	void Start () 
	{
		if (_renderer == null) 
		{
			Debug.Log("Renderer is empty");
		} 

		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On; 
		GetComponent<Renderer>().receiveShadows = true;
		
		//TESTE
		if (shadowOnly == true)
        {
			GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
	}
}
