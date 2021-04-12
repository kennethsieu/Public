using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulsive : Card
{
    #region Private Variables
    private List<GameObject> valid;
    private Hero hero;
    private BattleManager battle;
    #endregion
    public void Start()
    {
        valid = new List<GameObject>();
        valid.Add(GameObject.FindWithTag("Enemy1"));
        valid.Add(GameObject.FindWithTag("Enemy2"));
        valid.Add(GameObject.FindWithTag("Enemy3"));
        battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();

    }
    override
    public bool validUsage(Collision2D collison)
    {
        return true;
    }

    override
    public bool validTarget(GameObject collison)
    {
        return true;
    }
    override
    public void cardAction(GameObject pos)
    {
        hero = battle.getCurrent().GetComponent<Hero>();
        hero.GetComponent<DeckManager>().Draw(2);
    }
}
