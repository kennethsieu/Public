using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCHandler : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject options;

    private ESCHandler instance = null;
    #endregion

    #region Start/Update Method
    private void Start()
    {
        /*if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }*/
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        pressedESC();
    }
    #endregion

    #region Button Press
    private void pressedESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.SetActive(!options.activeSelf);
        }
    }
    #endregion
}
