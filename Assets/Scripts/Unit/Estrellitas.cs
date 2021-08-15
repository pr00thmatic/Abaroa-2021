using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Estrellitas : MonoBehaviour {
  [Header("Initialization")]
  public Attackable attackable;
  public GameObject estrellitas;

  void OnEnable () {
    attackable.onDead += HandleDead;
  }
  void OnDisable () {
    attackable.onDead -= HandleDead;
  }

  public void HandleDead () {
    if (attackable.diesWhenDead) estrellitas.SetActive(true);
  }
}
