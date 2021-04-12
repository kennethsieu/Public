using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;
    // public int[] spawnDirs;

    private void Start() {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);

        // spawnDirs = new int[4];
        // spawnDirs = currRoom.transform.Find("SpawnPoint").GetComponent<RoomSpawner>().openingDirection;
    }

}
