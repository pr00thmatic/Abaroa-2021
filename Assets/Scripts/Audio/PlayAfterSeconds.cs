using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAfterSeconds : MonoBehaviour {
  [Header("Configuration")]
  public float wait;

  [Header("Initialization")]
  public AudioSource speaker;

  void Reset () {
    speaker = GetComponent<AudioSource>();
  }

  IEnumerator Start () {
    yield return new WaitForSeconds(wait);
    speaker.Play();
  }
}
