using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private SpriteRenderer m_Renderer;
    private RoomTemplates templates;
    private QuestManager quest;
    public bool[] openingDir;

    private void Start() {
        m_Renderer = GetComponent<SpriteRenderer>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        quest = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();
    }

    bool checkOpening(Destroyer thisRoom, GameObject curr) {
        bool left = (thisRoom.transform.position.x == curr.transform.position.x - 10f) && (thisRoom.transform.position.y == curr.transform.position.y); 
        bool right = (thisRoom.transform.position.x == curr.transform.position.x + 10f) && (thisRoom.transform.position.y == curr.transform.position.y);
        bool down = (thisRoom.transform.position.y == curr.transform.position.y - 10f) && (thisRoom.transform.position.x == curr.transform.position.x);
        bool up = (thisRoom.transform.position.y == curr.transform.position.y + 10f) && (thisRoom.transform.position.x == curr.transform.position.x);
        bool[] currOpenings = curr.transform.Find("Destroyer").GetComponent<Destroyer>().openingDir;
        if (left && thisRoom.openingDir[3] && currOpenings[2]) return true;
        if (right && thisRoom.openingDir[2] && currOpenings[3]) return true;
        if (up && thisRoom.openingDir[0] && currOpenings[1]) return true;      
        if (down && thisRoom.openingDir[1] && currOpenings[0]) return true;
        return false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Destroy(other.gameObject);
    }

    private void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        Debug.Log("Hovering over room");
        if (this.transform == templates.currRoom.transform.Find("Destroyer")) {
            return;
        }
        bool left = (this.transform.position.x == templates.currRoom.transform.position.x - 10) && (this.transform.position.y == templates.currRoom.transform.position.y); 
        bool right = (this.transform.position.x == templates.currRoom.transform.position.x + 10) && (this.transform.position.y == templates.currRoom.transform.position.y);
        bool down = (this.transform.position.y == templates.currRoom.transform.position.y - 10) && (this.transform.position.x == templates.currRoom.transform.position.x);
        bool up = (this.transform.position.y == templates.currRoom.transform.position.y + 10) && (this.transform.position.x == templates.currRoom.transform.position.x);
        m_Renderer.enabled = true;
        if (checkOpening(this, templates.currRoom)) {
            Debug.Log("Possible room");
            m_Renderer.material.color = new Color(0, 255, 255, 150);
                
        } else {
            m_Renderer.material.color = new Color(85, 255, 255, 255);
        }
        
    }

    private void OnMouseExit() {
        m_Renderer.enabled = false;
    }

    private void OnMouseDown() {
        Debug.Log("Mouse down");
        if (!checkOpening(this, templates.currRoom)) {
            return;
        }
        if (this.transform == templates.endRoom.transform.Find("Destroyer")) {
            quest.endQuest = true;
        } 
        for (int i = 0; i < templates.rooms.Count; i++) {
            if (this.transform == templates.rooms[i].transform.Find("Destroyer")) {
                PlayerPrefs.SetInt("currRoom", i);
            }
        }
        templates.currRoom = this.transform.parent.gameObject;
        
        quest.NextDungeon();

    }
}
