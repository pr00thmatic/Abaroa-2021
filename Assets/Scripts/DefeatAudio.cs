using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DefeatAudio : MonoBehaviour {
  [Header("Configuration")]
  public float summonWait;
  public float summonAfterWait = 5;
  public float audioLength;
  public string toLoad = "vortice temporal Bolivia defeat";

  [Header("Initialization")]
  public VortexInCommand vortex;

  void Reset () {
    audioLength = GetComponent<AudioSource>().clip.length + 0.5f;
  }

  void Start () {
    StartCoroutine(_Restart());
  }
  IEnumerator _Restart () {
    yield return new WaitForSeconds(summonWait);
    vortex.SummonVortex();
    yield return new WaitForSeconds(Mathf.Max(summonAfterWait, audioLength - summonWait));
    SceneManager.LoadScene(toLoad);
  }

  // void Start () { StartCoroutine(_VortexSummon()); StartCoroutine(_NextScene()); }
  // IEnumerator _VortexSummon () {
  //   yield return new WaitForSeconds(summonWait);
  //   vortex.SummonVortex();
  // }
  // IEnumerator _NextScene () {
  //   yield return new WaitForSeconds(nextSceneWait);
  //   SceneManager.LoadScene(toLoad);
  // }
}
