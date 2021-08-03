using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour {
  void Update () {
    transform.forward = transform.position - Camera.main.transform.position;
  }
}
