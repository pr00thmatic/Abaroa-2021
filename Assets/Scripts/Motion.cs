using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Motion : MonoBehaviour {
  [Header("Information")]
  public Tile standing;
  public Tile oldStanding;

  [Header("Configuration")]
  public int distance;
  public Grid grid;

  void OnTriggerStay (Collider c) {
    print("trigger stay");
    Tile found = c.GetComponentInParent<Tile>();
    if (!found) return;
    if (found != standing && standing) {
      oldStanding = null;
      Shine(oldStanding, false);
    }

    standing = c.GetComponent<Tile>();
    if (standing) {
      Shine(standing, true);
    }
  }

  public void Shine (Tile t, bool value) {
    for (int r=-distance; r<=distance; r++) {
      for (int c=-distance; c<=distance; c++) {
        if (Mathf.Abs(r) + Mathf.Abs(c) > distance) continue;
        grid.GetTile(standing.Row + r, standing.Column + c)?.Shine(value);
      }
    }
  }
}
