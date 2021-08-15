using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartOnTimeout : MonoBehaviour {
  [Header("Configuration")]
  public float waitToStart = 6;

  [Header("Initialization")]
  public StartButton start;

  IEnumerator Start () {
    yield return new WaitForSeconds(waitToStart);
    start.StartCoroutine(start._Start());
  }
}
