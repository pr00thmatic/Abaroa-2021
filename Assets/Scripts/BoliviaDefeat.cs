using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoliviaDefeat : MonoBehaviour {
  [Header("Initialization")]
  public Transform sounds;

  IEnumerator Start () {
    yield return new WaitForSeconds(1);
    sounds.GetChild(Random.Range(0, sounds.childCount)).gameObject.SetActive(true);
  }
}
