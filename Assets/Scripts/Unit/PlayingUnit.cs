using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class PlayingUnit : Unit {
  public static event System.Action<PlayingUnit> onClicked;
  public event System.Action<bool> onSelected;
  public event System.Action onTurnBegin;
  public event System.Action onTurnEnd;

  // [Header("Configuration")]
  public int TotalActions { get => motion.actions + attack.actions; }

  [Header("Information")]
  public Tile standing;
  public TurnManager turn;
  public bool hasRemainingActions;
  public bool HasRemainingActions {
    get => motion.HasRemainingActions || attack.HasRemainingActions;
  }

  [Header("Initialization")]
  public Attack attack;
  public Motion motion;
  public Animator animator;

  void OnEnable () {
    TurnManager.onTurnBegin += HandleBeginOfTurn;
    TurnManager.onTurnEnd += HandleEndOfTurn;
  }

  void OnDisable () {
    TurnManager.onTurnBegin -= HandleBeginOfTurn;
    TurnManager.onTurnEnd -= HandleEndOfTurn;
  }

  void OnTriggerStay (Collider c) {
    Tile found = c.GetComponentInParent<Tile>();
    if (found) {
      standing = found;
    }
  }

  void OnMouseDown () {
    onClicked?.Invoke(this);
  }

  void Update () {
    hasRemainingActions = HasRemainingActions;
  }

  public void Deselect () {
    onSelected?.Invoke(false);
  }

  public void Select () {
    onSelected?.Invoke(true);
  }

  public void ConsumeAction () {
    turn.DeselectSelected();
    turn.CheckForEndTurn();
    UpdateHasActionAnimation();
  }

  public void HandleBeginOfTurn (TurnManager turn) {
    if (turn.faction != this.faction.id) return;
    turn.unitsInTurn.Add(this);
    onTurnBegin?.Invoke();

    this.turn = turn;
    UpdateHasActionAnimation();
  }

  public void HandleEndOfTurn (TurnManager turn) {
    if (turn.faction != this.faction.id) return;
    onTurnEnd?.Invoke();
    UpdateHasActionAnimation();
  }

  public void UpdateHasActionAnimation () {
    animator.SetBool("has action", HasRemainingActions);
  }
}
