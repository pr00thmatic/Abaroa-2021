using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffsetForSibling : MonoBehaviour {
  [Header("Configuration")]
  public float mod = 3;

  [Header("Initialization")]
  public Animator animator;

  void Start () {
    animator.SetFloat("offset", (transform.GetSiblingIndex() % mod) / (mod));
  }
}
