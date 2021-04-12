using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero: Damagable
{
    #region Private Variables
    [SerializeField]
    private GameObject[] deck;

    [SerializeField]
    private Sprite[] images;
    #endregion

    public GameObject[] getDeck()
    {
        return deck;
    }
    override
    public Sprite[] getImages()
    {
        return images;
    }

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
            if (getCurHealth() == 0)
            {
                if (Random.Range(0, 100) < getDeathBlow())
                {
                    Death();
                }
            }
            int damage = value;
            setCurHealth(getCurHealth() - damage);
            return DidCrit + damage.ToString();
        }
    }

    public override void Death()
    {
        setAlive(false);
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        battle.DeathAndRemove(this.GetComponent<Hero>());
    }

    public void HeroTurn()
    {
        DeckManager deck = this.GetComponent<DeckManager>();
        if (getModTime("stun") != 0)
        {
            deck.DrawPass();
        } else
        {
            deck.DrawMove();
            deck.Draw(4);
            deck.DrawPass();
        }

    }

    public void EndTurn()
    {
        DeckManager deck = this.GetComponent<DeckManager>();
        deck.EndTrun();
    }

}
