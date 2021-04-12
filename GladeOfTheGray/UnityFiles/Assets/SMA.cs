using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMA : Card
{
    #region Private Variables
    private List<GameObject> valid;
    private Hero hero;
    private BattleManager battle;

    #endregion
    public void Start()
    {
        valid = new List<GameObject>();
        valid.Add(GameObject.FindWithTag("Hero1"));
        valid.Add(GameObject.FindWithTag("Hero2"));
        valid.Add(GameObject.FindWithTag("Hero3"));
        battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();

    }
    public override bool validUsage(Collision2D collision)
    {
        return true;
    }

    override
    public bool validTarget(GameObject collision)
    {
        return true;
    }

    public override void cardAction(GameObject position)
    {
        hero = battle.getCurrent().GetComponent<Hero>();
        GameObject Display = GameObject.FindWithTag("ActionCam");
        hero.AddMod("dodge", 20, 2);
        Display.GetComponent<DisplayScene>().BattleDisplaySelf(hero, new Vector3(600, 50, 0), "+ 20 Dodge" );
        battle.ViewBattle(0);
        battle.MinusAction();
    }

}
