using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Attack : MonoBehaviour {
  [Header("Configuration")]
  public int range = 1;
  public int actions = 1;

  [Header("Initialization")]
  public PlayingUnit unit;
  public int remainingActions = 0;

  void OnEnable () {
    unit.onTurnBegin += HandleTurnBegin;
    unit.onTurnEnd += HandleTurnEnd;
    unit.onSelected += OnSelected;
  }

  void OnDisable () {
    unit.onTurnBegin -= HandleTurnBegin;
    unit.onTurnEnd -= HandleTurnEnd;
    unit.onSelected -= OnSelected;
  }

  public void AttackIt (Unit other) {
    if (!CanAttack(other)) return;
    remainingActions = Mathf.Max(remainingActions-1, 0);
    unit.ConsumeAction();
    print("ATTACK! " + Time.time);
  }

  public bool CanAttack (Unit other) {
    if (!(other as PlayingUnit) || other.faction.id == unit.faction.id) return false;

    float distance = Vector3.Distance(transform.position, other.transform.position);
    return (range + 0.5f) * unit.Grid.TileSize >= distance;
  }

  public void DisplayAttack () {
    List<Tile> adjascent = unit.standing.Adjascent;

    foreach (Tile tile in adjascent) {
      if (tile && tile.occupier && CanAttack(tile.occupier)) {
        tile.Shine(ActionType.Attack);
      }
    }
  }

  public void HideAttack () {
    List<Tile> adjascent = unit.standing.Adjascent;

    foreach (Tile tile in adjascent) {
      tile.StopShinning();
    }
  }

  public void HandleTurnBegin () {
    remainingActions = actions;
  }

  public void HandleTurnEnd () {
    remainingActions = 0;
  }

  public void OnSelected (bool value) {
    if (value) DisplayAttack();
    else HideAttack();
  }
}
