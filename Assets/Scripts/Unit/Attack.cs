using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Attack : MonoBehaviour {
  [Header("Configuration")]
  public int range = 1;
  public int actions = 1;
  public int power = 1;

  [Header("Initialization")]
  public PlayingUnit unit;
  public int remainingActions;
  public bool HasRemainingActions { get => remainingActions > 0 && CanAttackAdjascent(); }

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
    unit.animator.SetTrigger("attack");
    other.GetComponent<Attackable>().GetAttacked(this);
    unit.ConsumeAction();
  }

  public bool CanAttackAdjascent () {
    if (!unit.standing) return false;

    List<Tile> adjascent = unit.standing.Adjascent;

    foreach (Tile tile in adjascent) {
      if (!tile) continue;
      if (tile.occupier && CanAttack(tile.occupier)) return true;
    }

    return false;
  }

  public bool CanAttack (Unit other) {
    Attackable attackable = other.GetComponent<Attackable>();
    if (!attackable || other.faction.id == unit.faction.id  || remainingActions <= 0) return false;

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
      if (tile) tile.StopShinning();
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
