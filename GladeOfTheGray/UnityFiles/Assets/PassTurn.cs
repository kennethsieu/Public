using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurn : Card
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
        return true; 
    }

    override
    public bool validTarget(GameObject collision)
    {
        return true;
    }

    override
    public void cardAction(GameObject position)
    {
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        battle.PassTurn();
    }
}
