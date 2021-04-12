using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottoms;
    public GameObject[] tops;
    public GameObject[] lefts;
    public GameObject[] rights;
    public GameObject closed;

    public GameObject currRoom;
    public GameObject endRoom;
    public float waitTime = 2f;

    public List<GameObject> rooms;
    public QuestManager quest;
    public static RoomTemplates Instance = null;

    #region Unity_functions
    private void Awake() {
        quest = FindObjectOfType<QuestManager>();
    }

    private void Update() {
        currRoom = rooms[PlayerPrefs.GetInt("currRoom")];
        if (waitTime <= 0) {
            quest.num_quest_rooms = rooms.Count;
            // endRoom = rooms[rooms.Count - 1];
            SpriteRenderer endSprite = endRoom.transform.Find("Destroyer").GetComponent<SpriteRenderer>();
            endSprite.enabled = true;
            endSprite.color = new Color(0, 0, 0, 255);
        } else {
            waitTime -= Time.deltaTime;
        }
        Transform child = currRoom.transform.Find("Destroyer");
        SpriteRenderer highlight = child.GetComponent<SpriteRenderer>();
        highlight.enabled = true;
        highlight.color = new Color(255, 0, 0, 155);
    }


    #endregion

}
