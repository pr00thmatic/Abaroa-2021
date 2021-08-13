using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatrixGraph;

public class UnitAI : MonoBehaviour {
  [Header("Configuration")]
  public float attackPowerMultiplier = 2;
  public float hpMultiplier = 1;

  [Header("Information")]
  public bool actionWasTaken = false;

  [Header("Initialization")]
  public PlayingUnit unit;

  public IEnumerator _TakeAction () {
    List<Attackable> possibleTargets = unit.attack.GetAttackables();
    Attackable target = null;
    for (int i=0; i<possibleTargets.Count; i++) {
      if (!target) { target = possibleTargets[i]; continue; }
      if (AttackUtility(possibleTargets[i]) > AttackUtility(target)) {
        target = possibleTargets[i];
      }
    }

    if (target) {
      unit.attack.AttackIt(target.unit);
      actionWasTaken = true;
      yield break;
    }

    if (unit.motion.HasRemainingActions) {
      Transform factions = unit.Faction.transform.parent;
      Faction otherFaction = factions.GetChild((unit.Faction.transform.GetSiblingIndex() + 1) % 2).GetComponent<Faction>();
      PlayingUnit motionTarget = null;
      foreach (Transform child in otherFaction.transform) {
        PlayingUnit possibleTarget = child.GetComponent<PlayingUnit>();
        if (!possibleTarget) continue;
        if (!motionTarget) { motionTarget = possibleTarget; continue; }
        if (MotionUtility(possibleTarget) > MotionUtility(motionTarget)) {
          motionTarget = possibleTarget;
        }
      }

      if (!motionTarget) {
        actionWasTaken = false;
        yield break;
      }

      List<Tile> path = unit.motion.GetPathTo(motionTarget.standing);
      if (path == null) {
        unit.motion.DisplayMotion(unit.standing, true);
        unit.motion.DisplayMotion(unit.standing, false);
        if (unit.motion.shinning.Count == 0) {
          actionWasTaken = false;
          yield break;
        }
        actionWasTaken = true;
        yield return StartCoroutine(unit.motion._Move(unit.motion.shinning[Random.Range(0, unit.motion.shinning.Count)]));
        yield break;
      } else {
        yield return StartCoroutine(unit.motion._ExecutePath(path, 1, unit.motion.distance));
        actionWasTaken = true;
        yield break;
      }
    }

    actionWasTaken = false;
  }

  public float AttackUtility (Attackable toAttack) {
    return -(toAttack.currentHP - unit.attack.power) * hpMultiplier +
      toAttack.unit.attack.power * attackPowerMultiplier;
  }

  public float MotionUtility (PlayingUnit target) {
    return - Mathf.Ceil(Vector3.Distance(transform.position, target.transform.position) -
                       unit.motion.distance * GraphGrid.Instance.TileSize)
      - target.attackable.currentHP
      + target.attack.power;
  }
}
