using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Damagable : MonoBehaviour
{
    #region Stats
    private int[] base_speed_range = new int[2];
    private int cur_Health;
    private int max_Health;
    private int[] base_damage_range = new int[2];
    private int base_dodge;
    private int base_crit;
    private int deathblow;
    private bool living;

    
    

    public int getBaseSpeed()
    {
        return Random.Range(base_speed_range[0], base_damage_range[1]);
    }

    public int getMaxHealth()
    {
        return max_Health;
    }

    public int getCurHealth()
    {
        return cur_Health;
    }

    public int getAttackDamage()
    {
        return Random.Range(base_damage_range[0], base_damage_range[1]) + getModValue("attack");
    }

    public int getBaseDodge()
    {
        return base_dodge + getModValue("dodge");
    }

    public int getBaseCrit()
    {
        return base_crit;
    }

    public int getDeathBlow()
    {
        return deathblow;
    }

    public bool isAlive()
    {
        return living;
    }

    public void setBaseSpeed(int lower, int upper)
    {
        base_speed_range[0] = lower;
        base_speed_range[1] = upper;
    }

    public void setCurHealth(int x)
    {
        if(x < 0)
        {
            cur_Health = 0;
        } else if(x >= getMaxHealth())
        {
            cur_Health = getMaxHealth();
        } else
        {
            cur_Health = x;
        }
    }

    public void setMaxHealth(int x)
    {
        max_Health = x;
    }

    public void setAttackDamage(int lower, int upper)
    {
        base_damage_range[0] = lower;
        base_damage_range[1] = upper;
    }

    public void setBaseDodge(int x)
    {
        base_dodge = x;
    }

    public void setBaseCrit(int x)
    {
        base_crit = x;
    }

    public void setDeathBlow(int x)
    {
        deathblow = x;
    }

    public void setAlive(bool x)
    {
        living = x;
    }

    #endregion

    #region Modifers
    private Dictionary<string, int> ModValue = new Dictionary<string, int>();
    private Dictionary<string, int> ModTime = new Dictionary<string, int>();


    public virtual void modSetup()
    {
        ModValue.Add("dodge", 0);
        ModTime.Add("dodge", 0);
        ModValue.Add("stun", 0);
        ModTime.Add("stun", 0);
        ModValue.Add("attack", 0);
        ModTime.Add("attack", 0);
    }


    public virtual void modUpdate()
    {
        ModTime["dodge"] = Mathf.Max(0, ModTime["dodge"] - 1); 
        if(ModTime["dodge"] == 0)
        {
            ModValue["dodge"] = 0;
        }

        ModTime["stun"] = Mathf.Max(0, ModTime["stun"] - 1);
        if (ModTime["stun"] == 0)
        {
            ModValue["stun"] = 0;
        }

        ModTime["attack"] = Mathf.Max(0, ModTime["attack"] - 1);
        if (ModTime["attack"] == 0)
        {
            ModValue["attack"] = 0;
        }
    }

    public virtual void AddMod(string name, int value, int time)
    {
        ModValue[name] = value;
        ModTime[name] = time;
    }

    public int getModValue(string name)
    {
        return ModValue[name];
    }

    public int getModTime(string name)
    {
        return ModTime[name];
    }


    #endregion


    public virtual int getSpeed()
    {
        return getBaseSpeed();
    }

    public abstract Sprite[] getImages();



    public virtual string increaseHealth(float value)
    {
        string DidCrit = "";
        if (Random.Range(0, 100) < base_crit)
        {
            value *= 2;
            DidCrit = "Crit: ";
        }
        int heal = Mathf.RoundToInt(value * max_Health) + Random.Range(-3, 3);
        cur_Health = Mathf.Min(max_Health, cur_Health + heal);
        return DidCrit + heal.ToString();
    }

    public abstract string decreaseHealth(int value, int critchance);

    public abstract void Death();






}
