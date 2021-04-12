using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealShrine : MonoBehaviour
{
    #region GameObject_variables
    public string itemText = "Your party was restored to full healths.";
    private List<int> partyHealth;
    private Hero[] dungeonHeroes;
    private DungeonManager dungeonManager;
    #endregion

    #region Item_functions
    private void OnMouseDown() {
        dungeonManager = FindObjectOfType<DungeonManager>();
        partyHealth = dungeonManager.currentHealth;
        dungeonHeroes = dungeonManager.dungeonHeroes;
        Interact();
    }

    IEnumerator DestroyShrine() {
      yield return new WaitForSeconds(1.1f);
      Destroy(this.gameObject);
    }

    public void Interact() {
      Debug.Log("Interacting with shrine");
      StartCoroutine(DisplayText());
    //   StartCoroutine(DestroyShrine());
        for (int i = 0; i < dungeonHeroes.Length; i++) {
          if (dungeonHeroes[i] != null) {
            partyHealth[i] = dungeonHeroes[i].getMaxHealth();
          }
        }
      dungeonManager.currentHealth = partyHealth;
      return;
    }

    IEnumerator DisplayText() {
      GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().text = itemText;
      yield return new WaitForSeconds(1f);
      Debug.Log("no text");
      GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().text = "";
      yield return null;
    }

    #endregion
}
