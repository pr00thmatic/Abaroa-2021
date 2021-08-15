using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinibusVoladorTrayectoria : MonoBehaviour {
  [Header("Configuration")]
  public float speed = 10;

  [Header("Information")]
  public MinibusLanes lanes;

  void Awake () {
    lanes = GetComponentInParent<MinibusLanes>();
  }

  void Update () {
    transform.Translate(0,0, speed * Time.deltaTime);
    if (transform.localPosition.z < lanes.end.localPosition.z) {
      transform.position = lanes.start.position;
    }
  }
}
