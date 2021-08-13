using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WhoseTurn : MonoBehaviour {
  [Header("Initialization")]
  public Image flag;
  public TextMeshProUGUI label;
  public Animator animator;

  void OnEnable () {
    TurnManager.onTurnBegin += HandleTurn;
  }

  void OnDisable () {
    TurnManager.onTurnBegin -= HandleTurn;
  }

  public void HandleTurn (TurnManager whose) { StopAllCoroutines(); StartCoroutine(_HandleTurn(whose)); }
  IEnumerator _HandleTurn (TurnManager whose) {
    animator.SetTrigger("swap");
    yield return new WaitForSeconds(3/8f);
    label.text = "Es el turno de " + whose.myFaction.factionName;
    flag.sprite = whose.myFaction.flag;
  }
}
