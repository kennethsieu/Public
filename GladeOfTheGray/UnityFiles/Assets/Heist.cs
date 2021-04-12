using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heist : Card
{
    #region Private Variables
    private List<GameObject> valid;
    private Hero hero;
    private BattleManager battle;
    #endregion
    public void Start()
    {
        valid = new List<GameObject>();
        if (GameObject.FindWithTag("Enemy1").GetComponent<HeroDisplay>().GetEnemy().isAlive())
        {
            valid.Add(GameObject.FindWithTag("Enemy1"));
        } else if (GameObject.FindWithTag("Enemy2").GetComponent<HeroDisplay>().GetEnemy().isAlive())
        {
            valid.Add(GameObject.FindWithTag("Enemy2"));
        } else
        {
            valid.Add(GameObject.FindWithTag("Enemy3"));
        }
        battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();

    }
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
    public void cardAction(GameObject pos)
    {
        hero = battle.getCurrent().GetComponent<Hero>();
        List<GameObject> hand = new List<GameObject>();
        hand = hero.GetComponent<DeckManager>().getInPlay();
        int count = 0;
        foreach (GameObject card in hand)
        {
            if (card.GetComponent<Treasure>() != null)
            {
                count++;
                hero.GetComponent<DeckManager>().Discard(card);
            }
        }

        HeroDisplay display = pos.GetComponent<HeroDisplay>();
        Enemy enemy = display.GetEnemy();

        List<Enemy> enemies = new List<Enemy>();
        List<Vector3> EnemyPos = new List<Vector3>();
        List<string> damage = new List<string>();

        enemies.Add(enemy);
        EnemyPos.Add(new Vector3(0, 100, 0));
        damage.Add(enemy.decreaseHealth(hero.getAttackDamage() * (1 + count), hero.getBaseCrit()));

        GameObject Display = GameObject.FindWithTag("ActionCam");


        Display.GetComponent<DisplayScene>().BattleDispayAllyAttack(hero, enemies, EnemyPos, new Vector3(700, 100, 0), damage);
        battle.ViewBattle(0);

        battle.MinusAction();

    }
}
