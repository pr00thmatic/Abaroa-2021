using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class TurnManager : MonoBehaviour {
  [Header("Information")]
  public int playerTurn = 0;
  public Unit selectedUnit;

  void OnEnable () {
    Unit.onClicked += HandleUnitClick;
    Tile.onAnySelected += HandleTileClick;
  }

  void OnDisable () {
    Unit.onClicked -= HandleUnitClick;
    Tile.onAnySelected -= HandleTileClick;
  }

  public void DeselectSelected () {
    if (selectedUnit) selectedUnit.Deselect();
    selectedUnit = null;
  }

  public void HandleUnitClick (Unit clicked) {
    if (clicked.faction.id != playerTurn) return;

    if (selectedUnit) {
      Unit selected = selectedUnit;
      DeselectSelected();
      if (selected == clicked) return;
    }

    selectedUnit = clicked;
    selectedUnit.Select();
    selectedUnit.motion.onStartMoving += HandleMotion;
  }

  public void HandleMotion (Motion motion) {
    motion.onStartMoving -= HandleMotion;
    DeselectSelected();
  }

  public void HandleTileClick (Tile tile) {
    if (tile.occupier) HandleUnitClick(tile.occupier);
  }
}
