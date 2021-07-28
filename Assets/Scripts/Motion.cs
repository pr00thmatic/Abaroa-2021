using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Motion : MonoBehaviour {
  public event System.Action<Motion> onStartMoving;

  [Header("Configuration")]
  public int distance;
  public float speed = 8;

  [Header("Information")]
  public bool showingMotion;
  public List<Tile> shinning;

  [Header("Initialization")]
  public GraphGrid grid;
  public Dictionary<Tile, float> distances; // TODO: record this at the beginning of the turn
  public Transform rootMotion;
  public Unit unit;

  void Reset () {
    grid = GameObject.FindObjectOfType<GraphGrid>();
  }

  public void DisplayMotion (Tile t, bool value) {
    if (!value) { // turn'em off
      showingMotion = false;
      foreach (Tile tile in shinning) {
        tile.IsShinning = false;
        tile.onSelected -= HandleSelection;
      }
    } else { // turn'em on
      showingMotion = true;
      shinning.Clear();
      Dijkstra();
      foreach (Tile tile in shinning) {
        tile.IsShinning = true;
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
    Tile standing = unit.standing;
    Tile origin = t;
    List<Tile> path = new List<Tile>();

    do {
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
        rootMotion.transform.position = Vector3.MoveTowards(rootMotion.transform.position, destination.transform.position,
                                                            speed * Time.deltaTime);
        yield return null;
      }
    }
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
}
