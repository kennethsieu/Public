using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorchBearer : Enemy
{
    public TorchBearer()
    {
        setBaseSpeed(4, 12);
        setMaxHealth(13);
        setAttackDamage(2, 12);
        setBaseDodge(3);
        setBaseCrit(15);
        setCurHealth(getMaxHealth());
        setAlive(true);
        modSetup();


    }
    
    #region Serialized Variables
    [SerializeField]
    private Sprite[] images;
    #endregion

    #region Private Variables
    private int numberAttacks = 3;
    #endregion

    #region Methods
    override
    public void Attacks(int value)
    {
        //Loads the hero's in for easy finding of targets
        HeroDisplay pos1 = GameObject.FindWithTag("Hero1").GetComponent<HeroDisplay>();
        HeroDisplay pos2 = GameObject.FindWithTag("Hero2").GetComponent<HeroDisplay>();
        HeroDisplay pos3 = GameObject.FindWithTag("Hero3").GetComponent<HeroDisplay>();
        Hero hero1 = pos1.getHero();
        Hero hero2 = pos2.getHero();
        Hero hero3 = pos3.getHero();

        switch (value)
        {
            //Add attacks by adding cases
            case 1:

                StartCoroutine(Attack1());
                break;

            case 2:
                StartCoroutine(Attack2());
                break;
            
            case 3:
                StartCoroutine(Attack3());
                break;
        }

    }
    override
    public Sprite[] getImages()
    {
        return images;
    }

    override
    public int numAttack()
    {
        return numberAttacks;
    }


    #endregion

    #region Attacks
    IEnumerator Attack1()
    {
        GameObject scene = GameObject.FindWithTag("ActionCam");
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        HeroDisplay pos1 = GameObject.FindWithTag("Hero1").GetComponent<HeroDisplay>();
        HeroDisplay pos2 = GameObject.FindWithTag("Hero2").GetComponent<HeroDisplay>();
        HeroDisplay pos3 = GameObject.FindWithTag("Hero3").GetComponent<HeroDisplay>();
        Hero hero1 = pos1.getHero();
        Hero hero2 = pos2.getHero();
        Hero hero3 = pos3.getHero();

        List<Hero> heroes = new List<Hero>();
        List<Vector3> heroPos = new List<Vector3>();
        List<string> damages = new List<string>();
        if (hero1.isAlive())
        {
            heroes.Add(hero1);
            heroPos.Add(new Vector3(900, 100, 0));
        }
        if (hero2.isAlive())
        {
            heroes.Add(hero2);
            heroPos.Add(new Vector3(700, 100, 0));
        }
        if (hero3.isAlive())
        {
            heroes.Add(hero3);
            heroPos.Add(new Vector3(500, 100, 0));
        }


        battle.DisplayAbilityName("Daunting \n Flames");
        pos1.SelectTarget();
        pos2.SelectTarget();
        pos3.SelectTarget();


        yield return new WaitForSeconds(1);

        damages.Add(hero1.decreaseHealth(getAttackDamage() / 2 , getBaseCrit()));
        damages.Add(hero2.decreaseHealth(getAttackDamage() / 2, getBaseCrit()));
        damages.Add(hero3.decreaseHealth(getAttackDamage() / 2, getBaseCrit()));


        scene.GetComponent<DisplayScene>().BattleDispayEnemyAttack(heroes, this.GetComponent<Enemy>(), heroPos, new Vector3(0, 100, 0), damages);

        

        battle.ViewBattle(0);


    }

    IEnumerator Attack2()
    {
        GameObject scene = GameObject.FindWithTag("ActionCam");
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        HeroDisplay pos1 = GameObject.FindWithTag("Hero1").GetComponent<HeroDisplay>();
        HeroDisplay pos2 = GameObject.FindWithTag("Hero2").GetComponent<HeroDisplay>();
        HeroDisplay pos3 = GameObject.FindWithTag("Hero3").GetComponent<HeroDisplay>();
        Hero hero1 = pos1.getHero();
        Hero hero2 = pos2.getHero();
        Hero hero3 = pos3.getHero();

        List<Hero> heroes = new List<Hero>();
        List<Vector3> heroPos = new List<Vector3>();
        List<string> damages = new List<string>();

        int choice = Random.Range(0, 3);
        heroPos.Add(new Vector3(700, 100, 0));

        if (choice == 0)
        {
            pos1.SelectTarget();
        }
        else if (choice == 1)
        {
            pos2.SelectTarget();
        }
        else
        {
            pos3.SelectTarget();
        }

        battle.DisplayAbilityName("Blinding Light");
        yield return new WaitForSeconds(1);



        if (choice == 0)
        {
            heroes.Add(hero1);
            damages.Add(hero1.decreaseHealth(getAttackDamage(), getBaseCrit()));
            if(damages[0] != "Dodge")
            {
                hero1.AddMod("stun", 0, 1);
            }

        }
        else if (choice == 1)
        {
            heroes.Add(hero2);
            damages.Add(hero2.decreaseHealth(getAttackDamage(), getBaseCrit()));
            if (damages[0] != "Dodge")
            {
                hero2.AddMod("stun", 0, 1);
            }
        }
        else
        {
            heroes.Add(hero3);
            damages.Add(hero3.decreaseHealth(getAttackDamage(), getBaseCrit()));
            if (damages[0] != "Dodge")
            {
                hero3.AddMod("stun", 0, 1);
            }
        }


        scene.GetComponent<DisplayScene>().BattleDispayEnemyAttack(heroes, this.GetComponent<Enemy>(), heroPos, new Vector3(0, 100, 0), damages);

        battle.ViewBattle(0);
    }
    #endregion

    IEnumerator Attack3()
    {
        GameObject scene = GameObject.FindWithTag("ActionCam");
        BattleManager battle = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
        HeroDisplay pos1 = GameObject.FindWithTag("Enemy1").GetComponent<HeroDisplay>();
        HeroDisplay pos2 = GameObject.FindWithTag("Enemy2").GetComponent<HeroDisplay>();
        HeroDisplay pos3 = GameObject.FindWithTag("Enemy3").GetComponent<HeroDisplay>();
        Enemy enemy1 = pos1.GetEnemy();
        Enemy enemy2 = pos2.GetEnemy();
        Enemy enemy3 = pos3.GetEnemy();

        List<Damagable> enemies = new List<Damagable>();
        List<Vector3> pos = new List<Vector3>();
        List<string> buff = new List<string>();

        if (enemy1.isAlive())
        {
            enemies.Add(enemy1);
            pos.Add(new Vector3(0, 100, 0));
            buff.Add("+25% Damage");
        }
        if (enemy2.isAlive())
        {
            enemies.Add(enemy2);
            pos.Add(new Vector3(400, 100, 0));
            buff.Add("+25% Damage");

        }
        if (enemy3.isAlive())
        {
            enemies.Add(enemy3);
            pos.Add(new Vector3(800, 100, 0));
            buff.Add("+25% Damage");
        }


        battle.DisplayAbilityName("Rally to the Light");
        pos1.SelectTarget();
        pos2.SelectTarget();
        pos3.SelectTarget();


        yield return new WaitForSeconds(1);

        enemy1.AddMod("attack", Mathf.RoundToInt(enemy1.getAttackDamage() * 0.25f), 2);
        enemy2.AddMod("attack", Mathf.RoundToInt(enemy2.getAttackDamage() * 0.25f), 2);
        enemy3.AddMod("attack", Mathf.RoundToInt(enemy3.getAttackDamage() * 0.25f), 2);

        List<int> pose = new List<int>();
        pose.Add(1);
        pose.Add(1);
        pose.Add(1);




        scene.GetComponent<DisplayScene>().BattleDisplayBase(enemies, pos, buff, pose);

        battle.ViewBattle(0);
    }
}
