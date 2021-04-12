using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingCards : Card
{
    #region Private Variables
    private List<GameObject> valid;
    private Hero hero;
    private BattleManager battle;
    public void Start()
    {
        valid = new List<GameObject>();
        valid.Add(GameObject.FindWithTag("Enemy1"));
        valid.Add(GameObject.FindWithTag("Enemy2"));
        battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        hero = battle.getCurrent().GetComponent<Hero>();
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
        HeroDisplay display = position.GetComponent<HeroDisplay>();
        Enemy enemy = display.GetEnemy();

        List<Enemy> enemies = new List<Enemy>();
        List<Vector3> EnemyPos = new List<Vector3>();
        List<string> damage = new List<string>();

        enemies.Add(enemy);
        EnemyPos.Add(new Vector3(0, 100, 0));
        damage.Add(enemy.decreaseHealth(hero.getAttackDamage(), hero.getBaseCrit()));

        GameObject Display = GameObject.FindWithTag("ActionCam");


        Display.GetComponent<DisplayScene>().BattleDispayAllyAttack(hero, enemies, EnemyPos, new Vector3(700, 100, 0), damage);
        battle.ViewBattle(0);

        battle.MinusAction();
    }
}
