using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gambler : Hero
{
    public Gambler()
    {
        setBaseSpeed(9, 12);
        setAttackDamage(7, 9);
        setMaxHealth(22);
        setBaseDodge(25);
        setBaseCrit(35);
        setDeathBlow(0);
        setAlive(true);
        modSetup();
    }

}
