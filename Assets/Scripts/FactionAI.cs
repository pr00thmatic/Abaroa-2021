using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactionAI : MonoBehaviour {
  [Header("Information")]
  public TurnManager myTurn;

  void OnEnable () {
    TurnManager.onTurnBegin += HandleTurnBegin;
  }

  IEnumerator _TakeAction () {
    bool keepGoing = false;

    int guard = 10000;
    do {
      keepGoing = false;
      foreach (Transform child in transform) {
        UnitAI unit = child.GetComponent<UnitAI>();
        yield return StartCoroutine(unit._TakeAction());
        if (!keepGoing && unit.actionWasTaken) keepGoing = true;
      }
    } while (keepGoing && guard-- > 0);

    if (myTurn.gameObject.activeSelf) {
      myTurn.GetComponentInParent<StateMachine>().SetNextState();
    }
  }

  public void HandleTurnBegin (TurnManager turn) {
    if (Utils.TwoPlayersPlaying) return;
    if (turn.faction == GetComponent<Faction>().id) {
      myTurn = turn;
      StartCoroutine(_TakeAction());
    }
  }
}
