using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flippable : MonoBehaviour
{
    #region Editor Variable
    [SerializeField]
    private GameObject back;
    #endregion

    #region Variables
    private bool flipped = false;

    private int timer = 0;
    #endregion

    #region Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Flip());
        }
    }
    #endregion

    #region Flip Method
    IEnumerator Flip()
    {
        for (int i = 0; i < 180; i += 1)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(new Vector3(0, 1, 0));
            timer += 1;

            if (timer == 90 || timer == -90)
            {
                if (flipped)
                {
                    back.SetActive(false);
                    flipped = false;
                }
                else
                {
                    back.SetActive(true);
                    flipped = true;
                }
            }
        }
        timer = 0;
    }
    #endregion
}
