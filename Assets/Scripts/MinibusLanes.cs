using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinibusLanes : MonoBehaviour {
  [Header("Initialization")]
  public Transform start;
  public Transform end;

  void Reset () {
    start = transform.Find("start");
    end = transform.Find("end");
  }
}
