using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Unit : MonoBehaviour {
  public static event System.Action<Unit> onClicked;

  [Header("Information")]
  public Tile standing;

  [Header("Initialization")]
  public Motion motion;
  public Faction faction;

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
    motion.DisplayMotion(standing, false);
  }

  public void Select () {
    motion.DisplayMotion(standing, true);
  }
}
