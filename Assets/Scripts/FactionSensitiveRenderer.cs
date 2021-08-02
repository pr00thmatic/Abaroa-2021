using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactionSensitiveRenderer : MonoBehaviour {
  [Header("Initialization")]
  public new Renderer renderer;
  public int materialIndex;

  void Reset () {
    GetComponent<Renderer>();
  }
}
