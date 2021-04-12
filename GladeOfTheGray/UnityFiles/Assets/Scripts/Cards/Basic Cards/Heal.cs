using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heal : Card
{
    #region Private Variables
    private List<GameObject> valid;

    public void Start()
    {
        valid = new List<GameObject>();
        valid.Add(GameObject.FindWithTag("Hero1"));
        valid.Add(GameObject.FindWithTag("Hero2"));
        valid.Add(GameObject.FindWithTag("Hero3"));

    }
    #endregion

    override
    public bool validUsage(Collision2D collision)
    {
        GameObject position = collision.gameObject;

        foreach(GameObject item in valid)
        {
            if (item.Equals(position))
            {
                return true;
            }
        }
        return false;
    }

    override
    public bool validTarget(GameObject collision)
    {

        foreach (GameObject item in valid)
        {
            if (item.Equals(collision))
            {
                return true;
            }
        }
        return false;
    }

    override
    public void cardAction(GameObject position)
    {
        HeroDisplay display = position.GetComponent<HeroDisplay>();
        Hero hero = display.getHero();
        
       
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        GameObject Display = GameObject.FindWithTag("ActionCam");

        Display.GetComponent<DisplayScene>().BattleDisplaySelf(hero, new Vector3(600, 50, 0), "Heal: " + hero.increaseHealth(.25f));
        battle.ViewBattle(0);

        battle.MinusAction();
    }
}
