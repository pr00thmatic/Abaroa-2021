using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Overlays : MonoBehaviour {
  [Header("Information")]
  public bool isMouseOver = false;

  [Header("Initialization")]
  public Attackable attackable;
  public GameObject overlays;
  public TextMeshPro hp;
  public PlayingUnit unit;

  void OnEnable () {
    attackable.onHPChange += HandleHPChange;
    HandleHPChange();
  }

  void OnDisable () {
    attackable.onHPChange -= HandleHPChange;
  }

  void OnMouseOver () {
    isMouseOver = true;
  }

  void OnMouseExit () {
    isMouseOver = false;
  }

  void Update () {
    overlays.SetActive(isMouseOver || (unit.standing && unit.standing.isMouseOver));
  }

  public void HandleHPChange () {
    hp.text = "HP: " + attackable.currentHP + "/" + attackable.maxHP;
  }
}
