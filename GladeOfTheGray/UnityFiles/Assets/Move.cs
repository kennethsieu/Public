using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Card
{
    #region Private Variables
    private List<GameObject> valid;

    public void Start()
    {
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        valid = new List<GameObject>();
        valid.Add(GameObject.FindWithTag("Hero1"));
        valid.Add(GameObject.FindWithTag("Hero2"));
        valid.Add(GameObject.FindWithTag("Hero3"));
        valid.Remove(battle.currentChar());

    }
    #endregion

    override
    public bool validUsage(Collision2D collision)
    {
        GameObject position = collision.gameObject;

        foreach (GameObject item in valid)
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
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        HeroDisplay display1 = position.GetComponent<HeroDisplay>();
        HeroDisplay display2 = battle.currentChar().GetComponent<HeroDisplay>();
        Hero hero1 = display1.getHero();
        Hero hero2 = display2.getHero();
        battle.PosSwap(hero1, hero2);
        display1.updateChar(hero2);
        display2.updateChar(hero1);
        battle.MinusAction();

    }
}
