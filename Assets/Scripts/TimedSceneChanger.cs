using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TimedSceneChanger : MonoBehaviour {
  [Header("Configuration")]
  public float wait = 5;
  public string sceneToLoad = "final level";

  IEnumerator Start () {
    yield return new WaitForSeconds(wait);
    SceneManager.LoadScene(sceneToLoad);
  }
}
