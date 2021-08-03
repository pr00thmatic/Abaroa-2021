using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MatrixGraph {
public class Tile : MonoBehaviour {
  public static event System.Action<Tile> onAnySelected;
  public event System.Action<Tile> onSelected;
  [Header("Information")]
  public bool isMouseOver;
  public Unit occupier;
  public bool IsOccupied { get => occupier; }
  public Vector2 RowColumn { get => new Vector2(Row, Column); }
  public int Row { get => transform.parent.GetSiblingIndex(); }
  public int Column { get => transform.GetSiblingIndex(); }
  List<Tile> _adjascent = new List<Tile>();
  public List<Tile> Adjascent {
    get {
      if (_adjascent.Count != 0) return _adjascent;
      GraphGrid grid = GetComponentInParent<GraphGrid>();
      foreach (Vector2 direction in Utils.orthogonalDirections) {
        _adjascent.Add(grid.GetTile(Row + (int) direction.x, Column + (int) direction.y));
      }
      return _adjascent;
    }
  }

  [Header("Initialization")]
  public GameObject selected;
  public GameObject attack;

  void OnMouseUpAsButton () {
    onSelected?.Invoke(this);
    onAnySelected?.Invoke(this);
  }

  void OnMouseOver () {
    isMouseOver = true;
  }

  void OnMouseExit () {
    isMouseOver = false;
  }

  void OnTriggerStay (Collider c) {
    Unit possibleOccupier = c.GetComponent<Unit>();
    if (possibleOccupier) {
      occupier = possibleOccupier;
    }
  }

  void OnTriggerExit (Collider c) {
    Unit possibleOccupier = c.GetComponent<Unit>();
    if (possibleOccupier == occupier) {
      occupier = null;
    }
  }

  public void StopShinning () {
    selected.SetActive(false);
    attack.SetActive(false);
  }
  public void Shine (ActionType type) {
    selected.SetActive(!attack.gameObject.activeSelf && type == ActionType.Motion);
    attack.SetActive(type == ActionType.Attack);
  }
}
}
