using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StartButton : MonoBehaviour {
  [Header("Configuration")]
  public string sceneToLoad = "start";
  public string sceneToUnload = "main menu";

  [Header("Initialization")]
  public Animator title;
  public Animator menu;
  public VisualEffect vortex;
  public VisualEffect ajayus;
  public GameObject mainCameras;
  public GameObject unloadCameras;
  public GameObject ocluder;

  void Reset () {
    sceneToUnload = gameObject.scene.name;
  }

  void OnMouseUpAsButton () { StartCoroutine(_Start()); }
  public IEnumerator _Start () {
    if (title) title.SetTrigger("out");
    if (menu) menu.SetTrigger("out");
    yield return new WaitForSeconds(3);
    SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    mainCameras.SetActive(false);
    unloadCameras.SetActive(true);
    Destroy(ocluder);
    vortex.Stop();
    yield return new WaitForSeconds(1);
    ajayus.Stop();
    yield return new WaitForSeconds(3);
    SceneManager.UnloadSceneAsync(sceneToUnload);
  }
}
