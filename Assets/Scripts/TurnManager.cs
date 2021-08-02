using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class TurnManager : MonoBehaviour {
  public static event System.Action<TurnManager> onTurnBegin;
  public static event System.Action<TurnManager> onTurnEnd;

  [Header("Configuration")]
  public int faction = 0;

  [Header("Information")]
  public PlayingUnit selectedUnit;
  public int remainingActions = 0;
  public Motion blockedByMotion = null;

  void OnEnable () {
    PlayingUnit.onClicked += HandleUnitClick;
    Tile.onAnySelected += HandleTileClick;
    BeginTurn(); // TODO: remove?
  }

  void OnDisable () {
    PlayingUnit.onClicked -= HandleUnitClick;
    Tile.onAnySelected -= HandleTileClick;
  }

  public void BeginTurn () {
    remainingActions = 0;
    onTurnBegin?.Invoke(this);
  }

  public void DeselectSelected () {
    if (selectedUnit) selectedUnit.Deselect();
    selectedUnit = null;
  }

  public void HandleUnitClick (PlayingUnit clicked) {
    if (blockedByMotion) return;
    if (clicked.faction.id != faction && selectedUnit && selectedUnit.attack.actions > 0) {
      selectedUnit.attack.AttackIt(clicked);
      return;
    }
    if (clicked.RemainingActions <= 0) return;

    if (selectedUnit) {
      PlayingUnit selected = selectedUnit;
      DeselectSelected();
      if (selected == clicked) return;
    }

    selectedUnit = clicked;
    selectedUnit.Select();
    selectedUnit.motion.onStartMoving += HandleMotion;
  }

  public void HandleMotion (Motion motion) {
    motion.onStartMoving -= HandleMotion;
    blockedByMotion = motion;
    motion.onStopMoving += HandleStop;
  }

  public void HandleStop (Motion motion) {
    motion.onStopMoving -= HandleStop;
    if (blockedByMotion == motion) blockedByMotion = null;
  }

  public void HandleTileClick (Tile tile) {
    if (tile.occupier) HandleUnitClick(tile.occupier as PlayingUnit);
  }

  public void ConsumeAction () {
    remainingActions--;
    DeselectSelected();
    if (remainingActions <= 0) {
      remainingActions = 0;
      onTurnEnd?.Invoke(this);
      GetComponentInParent<StateMachine>().SetNextState();
    }
  }
}
