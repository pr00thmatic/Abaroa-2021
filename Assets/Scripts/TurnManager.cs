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
  public Motion blockedByMotion = null;
  public List<PlayingUnit> unitsInTurn;

  [Header("Initialization")]
  public Faction myFaction;

  void OnEnable () {
    PlayingUnit.onClicked += HandleUnitClick;
    Tile.onAnySelected += HandleTileClick;
    BeginTurn(); // TODO: remove?
  }

  void OnDisable () {
    PlayingUnit.onClicked -= HandleUnitClick;
    Tile.onAnySelected -= HandleTileClick;
    EndTurn();
  }

  public void BeginTurn () {
    if (unitsInTurn == null) unitsInTurn = new List<PlayingUnit>();
    unitsInTurn.Clear();
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
    if (!clicked.HasRemainingActions) return;

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

  public void EndTurn () {
    unitsInTurn.Clear();
    DeselectSelected();
    onTurnEnd?.Invoke(this);
  }

  public bool CheckForEndTurn () {
    foreach (PlayingUnit unit in unitsInTurn) {
      if (unit.HasRemainingActions) {
        return false;
      }
    }

    EndTurn();
    GetComponentInParent<StateMachine>().SetNextState();
    return true;
  }
}
