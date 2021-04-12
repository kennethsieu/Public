using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Card
{
    override
    public bool validUsage(Collision2D collison)
    {
        return false;
    }

    override
    public bool validTarget(GameObject collison)
    {
        return false;
    }

    override
    public void cardAction(GameObject pos)
    {

    }


}
    

