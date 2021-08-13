using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DisplayOverlayedInfo : MonoBehaviour {
  [Header("Initialization")]
  public TextMeshProUGUI hpValue;
  public Image hpBar;
  public TextMeshProUGUI atk;
  public Text unitName;
  public TextMeshProUGUI mov;
  public TextMeshProUGUI availableAtk;
  public TextMeshProUGUI availableMov;
  public Image portrait;
  public Image cantAttack;
  public GameObject root;

  [Header("Information")]
  public PlayingUnit displaying;

  void OnEnable () {
    Overlays.onOverlay += Display;
    Overlays.onExit += Hide;
  }

  void OnDisable () {
    Overlays.onOverlay -= Display;
    Overlays.onExit -= Hide;
  }

  public void Display (PlayingUnit unit) {
    if (!unit) return;
    displaying = unit;
    root.SetActive(true);
    unitName.text = unit.name;
    hpValue.text = unit.attackable.currentHP + "/" + unit.attackable.maxHP;
    hpBar.fillAmount = unit.attackable.NormalizedHP;
    atk.text = unit.attack.power + "";
    mov.text = unit.motion.distance + "";
    availableAtk.text = unit.attack.actions + "";
    availableMov.text = unit.motion.actions + "";
    cantAttack.gameObject.SetActive(unit.attack.CanAttackAdjascent());
    portrait.sprite = unit.portrait;
  }

  public void Hide (PlayingUnit unit) {
    if (unit == displaying) {
      root.SetActive(false);
    }
  }
}
