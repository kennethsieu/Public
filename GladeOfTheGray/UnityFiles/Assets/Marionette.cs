using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marionette : Hero
{
    public Marionette()
    {
        setBaseSpeed(3, 5);
        setAttackDamage(4, 10);
        setMaxHealth(45);
        setBaseDodge(10);
        setBaseCrit(10);
        setDeathBlow(0);
        setAlive(true);
        modSetup();
    }
}
