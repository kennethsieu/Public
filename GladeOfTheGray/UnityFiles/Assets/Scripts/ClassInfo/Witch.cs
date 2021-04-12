using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch: Hero
{
    public Witch()
    {
        setBaseSpeed(6, 9);
        setAttackDamage(9,15);
        setMaxHealth(30);
        setBaseDodge(15);
        setBaseCrit(25);
        setDeathBlow(0);
        setAlive(true);
        modSetup();
    }

}
