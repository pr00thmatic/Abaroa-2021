using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attackable : MonoBehaviour {
  public event System.Action onDead;
  public event System.Action onHPChange;

  [Header("Configuration")]
  public int maxHP;
  public bool diesWhenDead = false;

  [Header("Information")]
  public int currentHP;

  [Header("Initialization")]
  public PlayingUnit unit;

  public void GetAttacked (Attack attacker) { StartCoroutine(_GetAttacked(attacker)); }
  IEnumerator _GetAttacked (Attack attacker) {
    yield return new WaitForSeconds(0.2f);
    currentHP -= attacker.power;
    onHPChange?.Invoke();
    unit.animator.SetTrigger("take damage");

    if (currentHP <= 0) {
      currentHP = 0;
      onDead?.Invoke();
      unit.animator.SetBool("is dead", true);
      yield return new WaitForSeconds(2);
      if (diesWhenDead) {
        Destroy(gameObject);
      }
    }
  }
}
