using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Faction : MonoBehaviour {
  [Header("Configuration")]
  public int id;

  [Header("Information")]
  public int aknowledgedChildren = 0;

  [Header("Initialization")]
  public Material factionColor;

  void Update () {
    if (aknowledgedChildren != transform.childCount) {
      UpdateColors();
      aknowledgedChildren = transform.childCount;
    }
  }

  public void UpdateColors () {
    FactionSensitiveRenderer[] rs = GetComponentsInChildren<FactionSensitiveRenderer>();
    foreach (FactionSensitiveRenderer r in rs) {
      Material[] materials = r.renderer.sharedMaterials;
      materials[r.materialIndex] = factionColor;
      r.renderer.sharedMaterials = materials;
    }
  }
}
