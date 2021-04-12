using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    #region Public
    public Hero[] Heroes;

    public Enemy[] Enemies;

    #endregion

    public List<int> curHealth;


    #region Serialized Variables


    [SerializeField]
    private HeroDisplay[] display;

    [SerializeField]
    private HeroDisplay[] display2;


    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject ImageHolder;

    [SerializeField]
    private Text AbilityName;

    [SerializeField]
    private GameObject PlayerArea;

    [SerializeField]
    private Text actionsDisplay;

    [SerializeField]
    private GameObject Mask;

    #endregion


    #region Private Variables
    private List<Hero> mutable;
    private List<Enemy> mutable2;
    private List<Damagable> order;
    private int actions;
    private int EnemyDeath;
    private GameObject current;
    #endregion


    #region StartMethods
    public void Start()
    {
        #region Load in Sprites
        //DungeonManager dungeon = FindObjectOfType<DungeonManager>();
        //Heroes = dungeon.dungeonHeroes;
        //curHealth = dungeon.currentHealth;

        mutable = new List<Hero>();
        mutable2 = new List<Enemy>();


        EnemyDeath = 0;

        foreach (Hero hero in Heroes)
        {
            mutable.Add(Instantiate(hero, new Vector3(0, 0, 0), Quaternion.identity));
        }
        
        for (int i = 0; i < 3; i++)
        {
            display[i].setChar(mutable[i], background);
        }

        if (curHealth != null)
        {
            for (int i = 0; i < 3; i++)
            {
                mutable[i].setCurHealth(curHealth[i]);
            }
        }


        //Load in Enemy Sprites

        foreach (Enemy enemy in Enemies)
        {
            mutable2.Add(Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < 3; i++)
        {
            display2[i].setChar(mutable2[i], background);
        }

        foreach(Hero hero in mutable)
        {
            DeckManager temp = hero.GetComponent<DeckManager>() as DeckManager;
            temp.LoadDeck();
        }
        #endregion

        #region Order of Attacks
        order = new List<Damagable>();

        foreach (Enemy enemy in mutable2)
        {
            order.Add(enemy);
        }

        foreach (Hero hero in mutable)
        {
            order.Add(hero);
        }

        SortbySpeed();
        StartCoroutine(turnController());
        
        #endregion

    }
    #endregion

    #region Methods
    public void DisplayAbilityName(string name)
    {
        StartCoroutine(updateAbility(name));
    }

    public void ViewBattle(int delay)
    {
        StartCoroutine(CameraSwap(delay));
    }

    public void SortbySpeed()
    {
        List<Damagable> temp = new List<Damagable>();
        while(order.Count != 0)
        {
            Damagable fastest = order[0];
            int index = 0;
            for(int i = 0; i < order.Count; i++)
            {
                if(order[index].getSpeed() < order[i].getSpeed())
                {
                    fastest = order[i];
                    index = i;
                }
            }
            temp.Add(fastest);
            order.RemoveAt(index);
        }
        order = temp;
    }

    public void NextTrun()
    {
        getCurrent().modUpdate();
        order.Add(order[0]);
        order.RemoveAt(0);
    }

    public void MinusAction()
    {
        actions -= 1;
    }
    public void AddAction()
    {
        actions += 1;
    }

    public void AddAction(int num)
    {
        for(int i = 0; i < num; i++)
        {
            AddAction();
        }
    }

    public Damagable getCurrent()
    {
        return order[0];
    }

    public void DeathAndRemove(Damagable character)
    {
        if(character.GetComponent<Enemy>() != null)
        {
            if (order.Remove(character))
            {
                EnemyDeath++;
            }
            int index = mutable2.IndexOf(character.GetComponent<Enemy>());
            display2[index].enemyDie();
        } else
        {
            if (order.Remove(character))
            {
             
            }
            int index = mutable.IndexOf(character.GetComponent<Hero>());
            display[index].GetComponent<Image>().color = Color.red;
        }

    }

    public GameObject currentChar()
    {
        return current;
    }


    public void PassTurn()
    {
        actions = 0;
    }

    public void PosSwap(Hero hero1, Hero hero2)
    {
        int index = mutable.IndexOf(hero1);
        int index2 = mutable.IndexOf(hero2);

        mutable[index] = hero2;
        mutable[index2] = hero1;
    }
        
    #endregion

    #region Coroutine
    IEnumerator CameraSwap(int time)
    {
        yield return new WaitForSeconds(time);
        RectTransform bt = Mask.GetComponent<RectTransform>() as RectTransform;
        bt.SetSiblingIndex(2);
        yield return new WaitForSeconds(1.8f);
        bt.SetAsFirstSibling();
    }

   IEnumerator updateAbility(string ability)
   {
        AbilityName.GetComponent<Text>().text = ability;
        yield return new WaitForSeconds(1);
        AbilityName.GetComponent<Text>().text = "";
   }

   IEnumerator turnController()
    {
        while (true)
        {
            if(EnemyDeath >= 3)
            {
                Hero[] outHeroes = new Hero[3];
                for (int i = 0; i < mutable.Count; i++)
                {
                    outHeroes[i] = display[i].getHero();
                    curHealth[i] = display[i].getHero().getCurHealth() + 7;
                }
                GameObject dungeon = GameObject.FindWithTag("DungeonManager");
                dungeon.GetComponent<DungeonManager>().currentHealth = curHealth;
          
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene("Dungeon");
            } else
            {
                if (order[0].GetComponent<Enemy>() != null)
                {
                    Enemy enemy = order[0].GetComponent<Enemy>() as Enemy;
                    int index = mutable2.IndexOf(enemy);
                    display2[index].transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 0, 0, 0.07f);
                    current = display2[index].gameObject;

                    enemy.EnemyTurn();

                    yield return new WaitForSeconds(3);
                    display2[index].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                    NextTrun();

                }
                else
                {
                    actions = 1;
                    Hero hero = order[0].GetComponent<Hero>() as Hero;
                    int index = mutable.IndexOf(hero);
                    display[index].transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 0, 0, 0.07f);
                    current = display[index].gameObject;
                    hero.HeroTurn();
                    yield return new WaitUntil(() => actions == 0);
                    hero.EndTurn();
                    yield return new WaitForSeconds(1);
                    display[index].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                    NextTrun();
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    #endregion

    #region Updates
    public void Update()
    {
        if(order[0].GetComponent<Hero>() != null)
        {
            actionsDisplay.text = "Actions Remaining: " + actions.ToString();
        } else
        {
            actionsDisplay.text = "";
        }
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Hero[] outHeroes = new Hero[3];
            for (int i = 0; i < mutable.Count; i++)
            {
                outHeroes[i] = mutable[i];
            }
            GameObject dungeon = GameObject.FindWithTag("DungeonManager");
            for (int i = 0; i < mutable.Count; i++)
                {
                    curHealth[i] = mutable[i].getCurHealth();
                }
            dungeon.GetComponent<DungeonManager>().currentHealth = curHealth;
            SceneManager.LoadScene("Dungeon");
        }



    }


    #endregion
}
