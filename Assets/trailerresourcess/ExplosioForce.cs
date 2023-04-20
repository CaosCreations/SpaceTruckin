using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosioForce : MonoBehaviour {
    public float expForce;
    public float radius;
    void Start () 
    {

        ExplosionKnockback();

    }

    // Update is called once per frame
    void Update () {
            
       
    }

    void ExplosionKnockback() {
        Collider[] colliders = Physics.OverlapSphere (transform.position, radius);

        foreach (Collider nearby in colliders) {

            Rigidbody rgb = nearby.GetComponent<Rigidbody> ();
            if (rgb != null) {
                rgb.AddExplosionForce (expForce, transform.position, radius);
            }
        }

    }

}