using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 -> bottom door, 2 -> top door
    // 3 -> left door, 4 -> right door

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    void Start() {

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn() {
        if (!spawned) {
            if (openingDirection == 2) {
                rand = Random.Range(0, templates.bottoms.Length);
                Instantiate(templates.bottoms[rand], transform.position, templates.bottoms[rand].transform.rotation);
            } else if (openingDirection == 1) {
                rand = Random.Range(0, templates.tops.Length);
                Instantiate(templates.tops[rand], transform.position, templates.tops[rand].transform.rotation);
            } else if (openingDirection == 4) {
                rand = Random.Range(0, templates.lefts.Length);
                Instantiate(templates.lefts[rand], transform.position, templates.lefts[rand].transform.rotation);
            } else if (openingDirection == 3) {
                rand = Random.Range(0, templates.rights.Length);
                Instantiate(templates.rights[rand], transform.position, templates.rights[rand].transform.rotation);
            }
            spawned = true;
        }
    }

    // void OnTriggerEnter2D(Collider2D collision) {
    //     if (collision.CompareTag("Respawn")) {
    //         try
    //         {
    //             if (!collision.GetComponent<RoomSpawner>().spawned && !spawned)
    //             {
    //                 Instantiate(templates.closed, transform.position, Quaternion.identity);
    //                 Destroy(gameObject);
    //             }
    //         }
    //         catch (System.Exception e)
    //         {
    //             Destroy(gameObject);
    //         }

    //         spawned = true;
    //     } 
    // }
}
