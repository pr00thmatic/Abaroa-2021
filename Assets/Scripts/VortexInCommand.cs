using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;

public class VortexInCommand : MonoBehaviour {
  [Header("Configuration")]
  public GameObject target;

  [Header("Initialization")]
  public VisualEffect vortex;
  public VisualEffect ajayus;
  public Animator animator;
  public GameObject root;

  // void Update () {
  //   if (Input.GetKeyDown(KeyCode.Space)) {
  //     SummonVortex();
  //   }
  // }

  public void SummonVortex (float exitTime = 3) { StartCoroutine(_SummonVortex(exitTime)); }
  IEnumerator _SummonVortex (float exitTime) {
    vortex.Play();
    ajayus.Play();
    root.SetActive(true);
    animator.SetTrigger("start");
    yield return new WaitForSecondsRealtime(exitTime);

    if (target) target.SetActive(false);
    vortex.Stop();
    ajayus.Stop();
    yield return new WaitForSecondsRealtime(1);
    animator.SetTrigger("stop");
  }
}
