using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SaveAbaroaCinematic : MonoBehaviour {
  [Header("Configuration")]
  public float waitCarajo = 4;
  public float waitVortex = 5;
  public float vortexUnsummonTime = 1;
  public bool fired = false;

  [Header("Initialization")]
  public Attackable attackable;
  public AudioSource description;
  public FactionAI ai;
  public GameObject turns;
  public AudioClip vortexSummon;
  public VortexInCommand vortex;

  void OnEnable () {
    attackable.onDead += TriggerCinematic;
  }

  public void TriggerCinematic () { StartCoroutine(_TriggerCinematic()); }
  IEnumerator _TriggerCinematic () {
    if (fired) yield break;
    fired = true;
    description.Play();
    ai.enabled = false;
    turns.SetActive(false);
    Time.timeScale = 0.1f;

    yield return new WaitForSecondsRealtime(waitCarajo);
    Time.timeScale = 1;

    yield return new WaitForSecondsRealtime(6);
    Time.timeScale = 0.1f;
    description.PlayOneShot(vortexSummon);
    vortex.SummonVortex(vortexUnsummonTime);
    yield return new WaitForSecondsRealtime(1);
    Time.timeScale = 1;
    yield return new WaitForSecondsRealtime(waitVortex);
    SceneManager.LoadScene("vortice temporal abaroa");
  }
}
