using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour {
  [Header("Configuration")]
  public float orientation = 1;

  void Update () {
    transform.forward = (transform.position - Camera.main.transform.position) * orientation;
  }
}
