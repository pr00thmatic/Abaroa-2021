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
    CurrentForward = Vector3.RotateTowards(CurrentForward, targetForward, angularSpeed * Time.deltaTime * Mathf.Deg2Rad, 1);
  }

  public void ForceComplete () {
    CurrentForward = targetForward;
  }
}
