using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    #region Public Variables 
    public Enemy[] spawnEnemies;

    public Hero[] dungeonHeroes;

    public Enemy[] dungeonEnemies;

    public Vector3 position;

    public List<int> currentHealth;

    private QuestManager quest;

    public static DungeonManager Instance = null;
    #endregion

    #region Unity_functions
    
    private void Start() {
      if (Instance == null) {
        Instance = this;
        quest = FindObjectOfType<QuestManager>();
        dungeonHeroes = quest.partyMembers;
        currentHealth = new List<int>();
        for (int i = 0; i < dungeonHeroes.Length; i++) {
          if (dungeonHeroes[i] != null) {
            currentHealth.Add(dungeonHeroes[i].getMaxHealth());
          }
        }
      } else if (Instance != this) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);

    }

    #endregion

    #region Scene_transitions
    public void StartBattle(Vector3 player_position) { 
      Debug.Log("Go to battle scene");
      dungeonEnemies = new Enemy[3];
      for (int i = 0; i < dungeonEnemies.Length; i++) {
        int rand = Random.Range(0, spawnEnemies.Length);
        dungeonEnemies[i] = spawnEnemies[rand];
      }
      position = player_position;
      PlayerPrefs.SetFloat("Party Position x", position.x);
      SceneManager.LoadScene("Battle");
    }

    // public IEnumerator questComplete() {
    //     // if (SceneManager.GetActiveScene().name == "Dungeon") {
    //       GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().Text = "Congrats on completing the quest!";
    //       yield return new WaitForSeconds(0.5f);
    //       GameObject.FindGameObjectWithTag("AbiltyName").GetComponent<Text>().Text = "";
    //     // }
    // }

    #endregion
}
