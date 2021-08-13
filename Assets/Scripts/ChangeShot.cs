using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeShot : MonoBehaviour {
  [Header("Configuration")]
  public float waitingTime = 8;
  [Header("Initialization")]
  public GameObject anotherShot;

  IEnumerator Start () {
    yield return new WaitForSeconds(waitingTime);
    anotherShot.SetActive(true);
  }
}
