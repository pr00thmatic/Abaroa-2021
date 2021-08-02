using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class Unit : MonoBehaviour {
  [Header("Initialization")]
  public GraphGrid _grid;
  public GraphGrid Grid { get { if (!_grid) _grid = GameObject.FindObjectOfType<GraphGrid>(); return _grid; } }
  public Faction faction { get => GetComponentInParent<Faction>(); }
}
