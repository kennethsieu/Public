using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    #region GameObject_variables
    public string itemText;
    #endregion

    #region Item_functions
    private void OnMouseDown() {
        Interact();

    }

    IEnumerator DestroyChest() {
      yield return new WaitForSeconds(1.1f);
      Destroy(this.gameObject);
    }

    public void Interact() {
      Debug.Log("Interacting with chest");
      StartCoroutine(DisplayText());
      StartCoroutine(DestroyChest());
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
