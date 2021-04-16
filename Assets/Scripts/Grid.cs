using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
  [Header("Initialization")]
  public Transform tiles;

  public Tile GetTile (int r, int c) {
    if (r < 0 || r >= tiles.childCount || c < 0 || c >= tiles.GetChild(r).childCount) return null;
    return tiles.GetChild(r).GetChild(c).GetComponent<Tile>();
  }
}
