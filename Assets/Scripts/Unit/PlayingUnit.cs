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
  public int RemainingActions {
    get => motion.remainingActions + attack.remainingActions;
  }
  public TurnManager turn;

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

  public void Deselect () {
    onSelected?.Invoke(false);
  }

  public void Select () {
    onSelected?.Invoke(true);
  }

  public void ConsumeAction () {
    animator.SetBool("has action", RemainingActions > 0);
    turn.ConsumeAction();
  }

  public void HandleBeginOfTurn (TurnManager turn) {
    if (turn.faction != this.faction.id) return;
    onTurnBegin?.Invoke();

    this.turn = turn;
    turn.remainingActions += TotalActions;
    animator.SetBool("has action", RemainingActions > 0);
  }

  public void HandleEndOfTurn (TurnManager turn) {
    if (turn.faction != this.faction.id) return;
    onTurnEnd?.Invoke();
    turn.remainingActions = 0;
    animator.SetBool("has action", RemainingActions > 0);
  }
}
