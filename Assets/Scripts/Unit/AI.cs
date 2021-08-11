using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
  [Header("Configuration")]
  public float attackPowerMultiplier = 2;
  public float hpMultiplier = 1;

  [Header("Initialization")]
  public PlayingUnit unit;

  void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) {
      TakeAction();
    }
  }

  public void TakeAction () {
    List<Attackable> possibleTargets = unit.attack.GetAttackables();
    Attackable target = null;
    for (int i=0; i<possibleTargets.Count; i++) {
      if (!target) { target = possibleTargets[i]; continue; }
      if (AttackUtility(possibleTargets[i]) > AttackUtility(target)) {
        target = possibleTargets[i];
      }
    }

    if (target) unit.attack.AttackIt(target.unit);
  }

  public float AttackUtility (Attackable toAttack) {
    return -(toAttack.currentHP - unit.attack.power) * hpMultiplier +
      toAttack.unit.attack.power * attackPowerMultiplier;
  }
}
