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
  public AudioSource speaker;
  public AudioClip sfx;
  public PlayingUnit unit;
  public int remainingActions;
  public bool HasRemainingActions { get => remainingActions > 0 && CanAttackAdjascent(); }
  public LerpedRotation forward;

  void OnEnable () {
    unit.onTurnBegin += HandleTurnBegin;
    unit.onTurnEnd += HandleTurnEnd;
    HandleControlChange();
    unit.Faction.onControlChange += HandleControlChange;
  }

  void OnDisable () {
    unit.onTurnBegin -= HandleTurnBegin;
    unit.onTurnEnd -= HandleTurnEnd;
    unit.onSelected -= OnSelected;
    unit.Faction.onControlChange -= HandleControlChange;
  }

  public void HandleControlChange () {
    unit.onSelected -= OnSelected;
    if (unit.Faction.controlledByPlayer) {
      unit.onSelected += OnSelected;
    }
  }

  public void AttackIt (Unit other) {
    if (!CanAttack(other)) return;
    speaker.PlayOneShot(sfx);
    forward.targetForward = other.transform.position - transform.position;
    remainingActions = Mathf.Max(remainingActions-1, 0);
    unit.animator.SetTrigger("attack");
    other.GetComponent<Attackable>().GetAttacked(this);
    unit.ConsumeAction();
  }

  public bool CanAttackAdjascent () {
    if (!unit.standing) return false;
    return GetAttackables().Count > 0;
  }

  public bool CanAttack (Unit other) {
    Attackable attackable = other.GetComponent<Attackable>();
    if (!attackable || other.faction.id == unit.faction.id  || remainingActions <= 0) return false;

    float distance = Vector3.Distance(transform.position, other.transform.position);
    return (range + 0.5f) * unit.Grid.TileSize >= distance;
  }

  public List<Attackable> GetAttackables () {
    if (!unit.standing) return new List<Attackable>();
    List<Tile> adjascent = unit.standing.Adjascent;
    List<Attackable> attackables = new List<Attackable>();

    foreach (Tile tile in adjascent) {
      if (tile && tile.occupier && CanAttack(tile.occupier)) {
        attackables.Add(tile.occupier.GetComponent<Attackable>());
      }
    }

    return attackables;
  }

  public void DisplayAttack () {
    List<Attackable> attackables = GetAttackables();

    foreach (Attackable attackable in attackables) {
      attackable.unit.standing.Shine(ActionType.Attack);
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
