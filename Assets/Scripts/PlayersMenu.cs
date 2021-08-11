using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersMenu : MonoBehaviour {
  [Header("Information")]
  public int players;

  [Header("Initialization")]
  public Animator animator;

  void Awake () {
    if (PlayerPrefs.HasKey("player")) {
      players = PlayerPrefs.GetInt("players");
    }
    animator.SetInteger("players", players);
  }

  void OnMouseUpAsButton () {
    players = players % 2 + 1;
    animator.SetInteger("players", players);
    PlayerPrefs.SetInt("players", players);
  }
}
