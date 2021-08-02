using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils {
  public static Vector2[] orthogonalDirections = new Vector2[] {
    Vector2.right, -Vector2.right,
    Vector2.up, -Vector2.up
    // , new Vector2(1,1).normalized, new Vector2(-1,1).normalized,
    // new Vector2(-1,-1).normalized, new Vector2(1,-1).normalized,
  };

  public static Vector3 SetY (Vector3 v, float y) {
    v.y = y;
    return v;
  }
}
