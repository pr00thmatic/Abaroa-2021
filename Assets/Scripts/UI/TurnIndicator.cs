using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnIndicator : MonoBehaviour {
  [Header("Initialization")]
  public StateMachine turnsManager;
  public Animator animator;

  void OnEnable () {
    turnsManager.onStateChange += HandleTurnChange;
    UpdateAnimator(turnsManager.GetCurrentState());
  }

  void OnDisable () {
    turnsManager.onStateChange -= HandleTurnChange;
  }

  public void UpdateAnimator (State state) {
    animator.SetBool("is my turn", state.GetComponent<TurnManager>().faction == 0);
  }

  public void HandleTurnChange (State state) {
    UpdateAnimator(turnsManager.GetCurrentState());
  }

  public void Pass () {
    turnsManager.SetNextState();
  }
}
