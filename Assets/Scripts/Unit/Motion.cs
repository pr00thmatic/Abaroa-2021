using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Motion : MonoBehaviour {
  public event System.Action<Motion> onStartMoving;
  public event System.Action<Motion> onStopMoving;

  [Header("Configuration")]
  public int distance;
  public float speed = 8;
  public int actions = 1;
  public float animationSpeed = 1;

  [Header("Information")]
  public int remainingActions = 0;
  public bool showingMotion;
  public List<Tile> shinning;
  public bool HasRemainingActions { get => remainingActions > 0; }

  [Header("Initialization")]
  public Dictionary<Tile, float> distances; // TODO: record this at the beginning of the turn
  public Transform rootMotion;
  public PlayingUnit unit;
  public LerpedRotation forward;

  void OnEnable () {
    unit.onTurnBegin += HandleTurnBegin;
    unit.onTurnEnd += HandleTurnEnd;
    if (unit.Faction.controlledByPlayer) {
      unit.onSelected += OnSelected;
    }
  }

  void OnDisable () {
    unit.onTurnBegin -= HandleTurnBegin;
    unit.onTurnEnd -= HandleTurnEnd;
    unit.onSelected -= OnSelected;
  }

  public void DisplayMotion (Tile t, bool value) {
    if (!value) { // turn'em off
      showingMotion = false;
      foreach (Tile tile in shinning) {
        tile.StopShinning();
        tile.onSelected -= HandleSelection;
      }
    } else { // turn'em on
      showingMotion = true;
      shinning.Clear();
      Dijkstra();
      foreach (Tile tile in shinning) {
        tile.Shine(ActionType.Motion);
        tile.onSelected += HandleSelection;
      }
    }
  }

  public void HandleSelection (Tile t) {
    Move(t);
  }

  public void Move (Tile t) { StopAllCoroutines(); StartCoroutine(_Move(t)); }
  IEnumerator _Move (Tile t) {
    onStartMoving?.Invoke(this);
    remainingActions = Mathf.Max(remainingActions-1, 0);
    unit.turn.DeselectSelected();
    Tile standing = unit.standing;
    Tile origin = t;
    List<Tile> path = new List<Tile>();

    do { // was this the clever dijkstra? o_O
      path.Add(origin);
      Tile destination = null;
      foreach (Tile tile in origin.Adjascent) {
        if (path.Contains(tile)) { continue; }
        if (!tile || !distances.ContainsKey(tile)) { continue; }
        if (!destination) {
          destination = tile;
          continue;
        }
        if (distances[tile] < distances[destination]) {
          destination = tile;
        }
      }
      origin = destination;
      yield return null;
    } while (origin != standing);


    for (int i=path.Count-1; i>= 0; i--) {
      Tile destination = path[i];

      while (Vector3.Distance(rootMotion.transform.position, destination.transform.position) > 0.05f) {
        unit.animator.SetFloat("speed", animationSpeed);
        forward.targetForward = Utils.SetY(destination.transform.position - rootMotion.transform.position, 0);
        rootMotion.transform.position = Vector3.MoveTowards(rootMotion.transform.position, destination.transform.position,
                                                            speed * Time.deltaTime);
        yield return null;
      }
    }
    forward.ForceComplete();
    unit.animator.SetFloat("speed", 0);
    onStopMoving?.Invoke(this);
    unit.ConsumeAction();
  }

  public void Dijkstra () {
    shinning = new List<Tile>();
    List<Tile> unexplored = new List<Tile>();
    distances = new Dictionary<Tile, float>();
    Tile origin = unit.standing;

    unexplored.Add(origin);
    distances[origin] = 0;

    while (unexplored.Count > 0) {
      Tile min = unexplored[0];
      for (int i=0; i<unexplored.Count; i++) {
        if (KnownDistanceTo(unexplored[i]) < KnownDistanceTo(min)) {
          min = unexplored[i];
        }
      }

      unexplored.Remove(min);
      if (!shinning.Contains(min)) {
        shinning.Add(min);
      }
      foreach (Tile tile in min.Adjascent) {
        if (!tile) continue;
        if (tile.IsOccupied && tile.occupier != unit) continue;
        if (distances[min] + 1 > distance) continue;
        if (!unexplored.Contains(tile) && !shinning.Contains(tile)) unexplored.Add(tile);
        if (distances[min] + 1 < KnownDistanceTo(tile)) {
          distances[tile] = distances[min] + 1;
        }
      }
    }

    shinning.Remove(unit.standing);
  }

  public float KnownDistanceTo (Tile tile) {
    return distances.ContainsKey(tile)? distances[tile]: Mathf.Infinity;
  }

  public void OnSelected (bool value) {
    if (!value) {
      DisplayMotion(unit.standing, false);
    } else if (HasRemainingActions) {
      DisplayMotion(unit.standing, value);
    }
  }

  public void HandleTurnBegin () {
    remainingActions = actions;
  }

  public void HandleTurnEnd () {
    remainingActions = 0;
  }
}
