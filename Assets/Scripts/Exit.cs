using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Exit : MonoBehaviour {
  [Header("Configuration")]
  public bool exitsApp = false;

  public void ExitIt () {
    if (exitsApp) Application.Quit();
    else SceneManager.LoadScene("main menu");
  }
}
