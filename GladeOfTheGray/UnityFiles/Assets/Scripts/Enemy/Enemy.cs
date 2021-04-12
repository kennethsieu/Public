using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Damagable
{ 
    public abstract void Attacks(int value);

    public abstract int numAttack();

    public override string decreaseHealth(int value, int critchance)
    {
        string DidCrit = "";
        if (Random.Range(0, 100) < getBaseDodge())
        {
            return "Dodge";
        }
        else
        {
            if (Random.Range(0, 100) < critchance)
            {
                value *= 2;
                DidCrit = "Crit: ";
            }
            int damage = value + Random.Range(-2, 2);
            setCurHealth(getCurHealth() - damage);

            if (getCurHealth() <= 0)
            {
                Death();
            }
            return DidCrit + damage.ToString();
        }
    }

    public override void Death()
    {
        setAlive(false);
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        battle.DeathAndRemove(this.GetComponent<Enemy>());
    }

    public void EnemyTurn()
    {
        int numAttacks = numAttack();
        Attacks(Random.Range(1, numAttacks + 1));
    }

}
