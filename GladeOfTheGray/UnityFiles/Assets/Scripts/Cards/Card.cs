using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public abstract bool validUsage(Collision2D collision);

    public abstract bool validTarget(GameObject collision);

    public abstract void cardAction(GameObject position);
        

}
