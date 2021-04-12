using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    #region Party information
    public Hero[] partyMembers;
    private List<GameObject> partyMembers_obj;
    private DungeonManager dungeonManager;
    private QuestManager questManager;
    private string[] heroTags = {"Hero1", "Hero2", "Hero3"};
    #endregion

    #region movement_variables
    public float movespeed;
    float horizontal_input;
    private Rigidbody2D PartyRB;
    Vector2 currDir;
    // public float maxBattleCooldown = 1f;
    private float currBattleCooldown;
    private float curr_pos;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Unity_functions

    private void Start() {
        dungeonManager = FindObjectOfType<DungeonManager>();
        questManager = FindObjectOfType<QuestManager>();
        partyMembers = questManager.partyMembers;
        currBattleCooldown = Random.value;
        PartyRB = GetComponent<Rigidbody2D>();
        partyMembers_obj = new List<GameObject>();

        curr_pos = PlayerPrefs.GetFloat("Party Position x");
        for (int i = 0; i < partyMembers.Length; i++) {
            Hero member = partyMembers[i];
            if (member != null) {
                GameObject temp_member = GameObject.FindWithTag(heroTags[i]);
                temp_member.GetComponent<SpriteRenderer>().sprite = member.getImages()[0];
                temp_member.GetComponent<Rigidbody2D>().position = new Vector3(curr_pos + (float)i * 1.5f, 0f, 0f);
                partyMembers_obj.Add(temp_member);
            }
        }
        PartyRB.position = new Vector3(curr_pos, 0f, 0f);
    }

    private void Update() {
        horizontal_input = Input.GetAxisRaw("Horizontal");
        partyMove();
    }
    #endregion

    #region Movement
    public void updateStartPos() {
        // Set position of current party members
        if (DungeonManager.Instance != null) {
            curr_pos = PlayerPrefs.GetFloat("Party Position x");
        } else {
            curr_pos = -1.5f * partyMembers.Length;
        }
        for (int i = 0; i < partyMembers.Length; i++) {
            Hero member = partyMembers[i];
            if (member != null) {
                GameObject temp_member = GameObject.FindWithTag(heroTags[i]);
                temp_member.GetComponent<SpriteRenderer>().sprite = member.getImages()[0];
                temp_member.GetComponent<Rigidbody2D>().position = new Vector3(curr_pos + (float)i * 1.2f, 0f, 0f);
                partyMembers_obj.Add(temp_member);
            }
        }
        PartyRB.position = new Vector3(curr_pos, 0f, 0f);
    }

    private void partyMove() {
        Vector2 velocity;
        if (horizontal_input < 0) {
            velocity = Vector2.left * movespeed;
            currDir = Vector2.left;
        } else if (horizontal_input > 0) {
            velocity = Vector2.right * movespeed;
            currDir = Vector2.right;
        } else {
            velocity = Vector2.zero;
            // anim.SetBool("Moving", false);
        }
        foreach (GameObject member in partyMembers_obj) {
            member.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        PartyRB.velocity = velocity;
        if (velocity != Vector2.zero) {
            currBattleCooldown -= Time.deltaTime;
            float battle_probability = Random.value;
            if (currBattleCooldown < 0) {
                if (battle_probability < 0.25 && currBattleCooldown <= 0) {
                    currBattleCooldown = Random.value;
                    dungeonManager.StartBattle(PartyRB.position);
                } else {
                    currBattleCooldown = Mathf.Min(0.3f, Random.value);
                }
            }
        } 
    }

    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D obj) {
        if (obj.transform.CompareTag("Enemy")) {
            Debug.Log("Entered enemy collider T");
            Vector3 pos = this.transform.position;
            Debug.Log(pos);
            dungeonManager.StartBattle(pos);
        
        } else if (obj.transform.CompareTag("Door")) {
            Debug.Log("Entered door collider T");
            questManager.EndDungeon();
        }
    }
    #endregion

    // #region Interact_functions
    // private void Interact() {
    //     Debug.Log("Interact");
    //     RaycastHit2D[] hits = Physics2D.BoxCastAll(PartyRB.position + currDir, new Vector2(1f, 1f), 0f, Vector2.zero, 0f);
    //     foreach(RaycastHit2D hit in hits) {
    //         if (hit.transform.CompareTag("Item")) {
    //             Debug.Log("Hit item");
    //             hit.transform.GetComponent<Item>().Interact();
    //         }
    //     }
    // }
    // #endregion
}
