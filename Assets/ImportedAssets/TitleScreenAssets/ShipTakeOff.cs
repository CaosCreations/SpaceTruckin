using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTakeOff: MonoBehaviour {
  public float Speed = 5.0f;

  public bool RotateOnX = false;
  public bool RotateOnY = false;
  public bool RotateOnZ = false;

  public bool MoveOnX = false;
  public bool MoveOnY = false;
  public bool MoveOnZ = false;

  public bool takeOffStart;
  public float movTimer;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

    Vector3 rotFactor = Vector3.one * Speed;

    if (!RotateOnX) rotFactor.x = 0;
    if (!RotateOnY) rotFactor.y = 0;
    if (!RotateOnZ) rotFactor.z = 0;

    transform.Rotate(
      rotFactor * Time.deltaTime
    );

    if (takeOffStart == true) {
      movTimer += 0.1f * Time.deltaTime;

      if (movTimer <= 0.1f) {
        this.transform.position -= new Vector3(0, -0.1f, 0);

      }

      if (movTimer >= 0.02f && movTimer <= 0.09f) {
        this.transform.position -= new Vector3(0.2f, 0, 0);
        RotateOnY = true;
        Speed = 25;

      }

      if (movTimer >= 0.38f) {

        RotateOnY = false;

      }

      if (movTimer >= 0.42f) {

        this.transform.position += new Vector3(0, 0, 0.5f);

      }

    }

  }

}