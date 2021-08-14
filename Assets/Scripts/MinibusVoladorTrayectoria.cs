using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinibusVoladorTrayectoria : MonoBehaviour {
  [Header("Configuration")]
  public float speed = 10;

  void Update () {
    transform.Translate(0,0, speed * Time.deltaTime);
  }
}
