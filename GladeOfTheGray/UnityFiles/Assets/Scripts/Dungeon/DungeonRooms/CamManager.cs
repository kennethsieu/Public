using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public Camera cam_dungeon;
    public Camera cam_map;

    private void Awake() {
        cam_dungeon.gameObject.SetActive(true);
        cam_map.gameObject.SetActive(false);
    }

    public void switchMap() {
        cam_map.gameObject.SetActive(true);
        cam_dungeon.gameObject.SetActive(false);
    }

    public void switchDungeon() {
        cam_dungeon.gameObject.SetActive(true);
        cam_map.gameObject.SetActive(false);
    }
}
