using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
  // [Header("Information")]
  public int Row { get => transform.parent.GetSiblingIndex(); }
  public int Column { get => transform.GetSiblingIndex(); }

  [Header("Initialization")]
  public GameObject selected;

  public void Shine (bool value) {
    selected.SetActive(value);
  }
}
