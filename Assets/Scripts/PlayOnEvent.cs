using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayOnEvent : MonoBehaviour {
  [Header("Configuration")]
  public AudioClip clip;

  [Header("Initialization")]
  public AudioSource speaker;

  void Reset () {
    speaker = GetComponent<AudioSource>();
  }

  public void PlayIt () {
    speaker.PlayOneShot(clip);
  }
}
