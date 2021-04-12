using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance = null;

    #region Quest_variables
    public int num_quest_rooms;
    public int quest_difficulty;
    public string[] quest_loot;
    public Hero[] partyMembers;
    public bool endQuest;
    #endregion

    #region Dungeon_variables
    public string[] possible_dungeons;
    private int num_dungeons;
    public CamManager camera;
    #endregion

    #region Unity_functions
    
    private void Start() {
      if (Instance == null) {
        Instance = this;
        PlayerPrefs.SetFloat("Party Position x", 0f);
        PlayerPrefs.SetInt("currRoom", 0);
      } else if (Instance != this) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(gameObject);

      num_dungeons = possible_dungeons.Length;
    }

    #endregion

    #region Scene_transitions
    IEnumerator questComplete() {
        GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().text = "Congrats on completing the quest!";
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().text = "";
        SceneManager.LoadScene("Quests");
    }
    public void EndDungeon() {
      if (endQuest) {
        StartCoroutine(questComplete());
        return;
        // EndQuest();
        // change to IENumerator that says congrats on finishing quest
      }
        camera = FindObjectOfType<CamManager>();
        // SceneManager.LoadScene("Map");
        camera.switchMap();
    }

    public void NextDungeon() {
        PlayerPrefs.SetFloat("Party Position x", 0f);
        // if (num_quest_rooms == 1) {
        // if (endQuest) {
        //     EndQuest();
        // } else {
            camera = FindObjectOfType<CamManager>();
            // num_quest_rooms -= 1;
            // int rand_dungeon = Random.Range(0, num_dungeons);
            // SceneManager.LoadScene(possible_dungeons[rand_dungeon]);
            FindObjectOfType<PartyController>().updateStartPos();
            camera.switchDungeon();
        // }
    }

    public void EndQuest() {
      
      SceneManager.LoadScene("Quests");
    }
    #endregion
}
