using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LerpedRotation : MonoBehaviour {
  [Header("Configuration")]
  public Vector3 targetForward;
  public float angularSpeed = 360;

  // [Header("Information")]
  public Vector3 CurrentForward { get => root.forward; set => root.forward = value; }

  [Header("Initialization")]
  public Transform root;

  void Reset () {
    targetForward = CurrentForward;
  }

  void OnEnable () {
    targetForward = CurrentForward;
  }

  void Update () {
    // if (targetForward == CurrentForward) return;
    // Vector3 patchedTarget =
    //   Mathf.Abs(CurrentForward.x) == Mathf.Abs(targetForward.x) &&
    //   Mathf.Abs(CurrentForward.z) == Mathf.Abs(targetForward.z)? targetForward + new Vector3(0.1f, 0, 0.1f): targetForward;
    CurrentForward =
      Utils.SetY(Vector3.RotateTowards(CurrentForward, targetForward, angularSpeed * Time.deltaTime * Mathf.Deg2Rad, 1), 0);
  }

  public void ForceComplete () {
    CurrentForward = targetForward;
  }
}
