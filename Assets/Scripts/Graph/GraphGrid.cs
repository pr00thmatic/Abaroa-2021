using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MatrixGraph {
public class GraphGrid : MonoBehaviour {
  // [Header("Information")]
  public int Rows { get => tiles.childCount; }
  public int Cols { get => tiles.GetChild(0).childCount; }
  public float TileSize { get => Vector3.Distance(GetTile(0,0).transform.position, GetTile(0,1).transform.position); }

  [Header("Initialization")]
  public Transform tiles;

  public Tile GetTile (Vector2 rowColumn) { return GetTile((int) rowColumn.x, (int) rowColumn.y); }
  public Tile GetTile (int r, int c) {
    if (r < 0 || r >= tiles.childCount || c < 0 || c >= tiles.GetChild(r).childCount) return null;
    return tiles.GetChild(r).GetChild(c).GetComponent<Tile>();
  }

  public Dictionary<Tile, float> Dijkstra (Vector2 origin, List<Tile> range) {
    if (range == null || range.Count == 0) {
      range = new List<Tile>(GetComponentsInChildren<Tile>());
    }
    if (!range.Contains(GetTile(origin))) range.Add(GetTile(origin));

    List<Tile> sptSet = new List<Tile>();
    List<Tile> unexplored = new List<Tile>();
    Dictionary<Tile, float> distances = new Dictionary<Tile, float>();

    for (int i=0; i<range.Count; i++) {
      if (range[i].Row == origin.x && range[i].Column == origin.y) {
        distances[range[i]] = 0;
      } else {
        distances[range[i]] = Mathf.Infinity;
      }
      unexplored.Add(range[i]);
    }

    int guard = 100;
    while (unexplored.Count > 0 && guard-- > 0) {
      Tile min = unexplored[0];
      for (int i=0; i<unexplored.Count; i++) {
        if (distances[unexplored[i]] < distances[min]) {
          min = unexplored[i];
        }
      }

      unexplored.Remove(min);
      sptSet.Add(min);
      foreach (Tile tile in min.Adjascent) {
        if (!tile || !distances.ContainsKey(tile)) continue;
        if (distances[min] + 1 < distances[tile]) {
          distances[tile] = distances[min] + 1;
        }
      }
    }

    return distances;
  }
}
}
