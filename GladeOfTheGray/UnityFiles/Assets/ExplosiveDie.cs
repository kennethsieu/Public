using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDie : Card
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
        valid.Add(GameObject.FindWithTag("Enemy3"));
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

        List<Enemy> enemies = new List<Enemy>();
        List<Vector3> EnemyPos = new List<Vector3>();
        List<string> damage = new List<string>();

        Enemy enemy1 = GameObject.FindWithTag("Enemy1").GetComponent<HeroDisplay>().GetEnemy();
        Enemy enemy2 = GameObject.FindWithTag("Enemy2").GetComponent<HeroDisplay>().GetEnemy();
        Enemy enemy3 = GameObject.FindWithTag("Enemy3").GetComponent<HeroDisplay>().GetEnemy();

        if (enemy1.getCurHealth() != 0)
        {
            enemies.Add(enemy1);
            EnemyPos.Add(new Vector3(400, 100, 0));
            damage.Add(enemy1.decreaseHealth(hero.getAttackDamage() / 3, hero.getBaseCrit() + 10));
        }


        if (enemy2.getCurHealth() != 0)
        {
            enemies.Add(enemy2);
            EnemyPos.Add(new Vector3(200, 100, 0));
            damage.Add(enemy2.decreaseHealth(hero.getAttackDamage() / 3, hero.getBaseCrit() + 10));

        }

        if (enemy3.getCurHealth() != 0)
        {
            enemies.Add(enemy3);
            EnemyPos.Add(new Vector3(0, 100, 0));
            damage.Add(enemy3.decreaseHealth(hero.getAttackDamage() / 3, hero.getBaseCrit() + 10));
        }


        GameObject Display = GameObject.FindWithTag("ActionCam");

        Display.GetComponent<DisplayScene>().BattleDispayAllyAttack(hero, enemies, EnemyPos, new Vector3(700, 100, 0), damage);
        battle.ViewBattle(0);

        hero.GetComponent<DeckManager>().Draw();
    }
}
