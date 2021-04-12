using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyHealth : MonoBehaviour
{
    private Vector3 maxScale;
    private Vector3 localScale;
    private float healthFraction;

    void Start() {
        maxScale = transform.localScale;
    }

    void Update() {
        int i;
        DungeonManager dungeonManager = FindObjectOfType<DungeonManager>();
        Hero[] dungeonHeroes = dungeonManager.dungeonHeroes;
        List<int> currentHealth = dungeonManager.currentHealth;
        if (this.tag == "Hero1") {
            i = 0;
        } else if (this.tag == "Hero2") {
            i = 1;
        } else if (this.tag == "Hero3") {
            i = 2;
        } else {
            return;
        }

        if (dungeonHeroes[i] == null) {
            return;
        }
        healthFraction = (float) currentHealth[i] / (float) dungeonHeroes[i].getMaxHealth();
        localScale = maxScale;
        localScale.x = healthFraction * maxScale.x;
        transform.localScale = localScale;
    }
}
