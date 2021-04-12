using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarDisplay : MonoBehaviour
{
    #region Serialized variables
    [SerializeField]
    private GameObject HealthBarBD;

    [SerializeField]
    private GameObject HealthBar;

    [SerializeField]
    private Text text;
    #endregion

    [SerializeField]
    private Text buff;

    #region private variables
    private Damagable hero;
    private GameObject HBD;
    private GameObject HB;

    #endregion

    public void SetHealthBar(Damagable character)
    {
        hero = character;

    }

    public void Start()
    {
        HBD = this.gameObject.transform.GetChild(0).gameObject;
        HB = HBD.transform.GetChild(0).gameObject;
    }


    public void Update()
    {
        if (hero.isAlive())
        {
            int cur = hero.getCurHealth();
            int max = hero.getMaxHealth();
            float percent = (float)cur / max;
            float width = HBD.GetComponent<RectTransform>().rect.width * percent;
            RectTransform ht = HB.GetComponent<RectTransform>() as RectTransform;
            ht.sizeDelta = new Vector2(width, 7);

            text.text = cur.ToString() + "/" + max.ToString();

            if(hero.getModTime("dodge") != 0)
            {
                buff.text = "+" + hero.getModValue("dodge").ToString() + " Dodge (" + hero.getModTime("dodge").ToString() + " turns) + \n"; 
            } if(hero.getModTime("stun") != 0)
            {
                buff.text = "Stunned (" + hero.getModTime("stun").ToString() + " turns) + \n";
            } else
            {
                buff.text = "";
            }




        }
        else
        {
            text.text = "";
            RectTransform ht = HB.GetComponent<RectTransform>() as RectTransform;
            RectTransform hbt = HBD.GetComponent<RectTransform>() as RectTransform;
            hbt.sizeDelta = new Vector2(0, 0);
            ht.sizeDelta = new Vector2(0, 0);
        }


        
    }

}
