using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Draggable;
using UnityEngine.SceneManagement;
using UnityEditor;

public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Dropzone Variables
    [SerializeField]
    private bool dropzone = false;

    [ConditionalHide("dropzone", true)]
    public GameObject acceptUI;

    [ConditionalHide("dropzone", true)]
    public GameObject questUI;

    [ConditionalHide("dropzone", true)]
    public GameObject[] quests;
    #endregion

    #region Acceptzone Variables
    [SerializeField]
    private bool acceptzone = false;

    [ConditionalHide("acceptzone", true)]
    public Button acceptButton;

    [ConditionalHide("acceptzone", true)]
    public GameObject goButton;

    [ConditionalHide("acceptzone", true)]
    public GameObject heroSelect;

    [ConditionalHide("acceptzone", true)]
    public GameObject heroUI;

    [ConditionalHide("acceptzone", true)]
    public GameObject[] heroes;
    #endregion

    #region Variables
    private Transform childParent;

    private CardType type;

    [HideInInspector]
    public List<GameObject> deactivated = new List<GameObject>();

    private Text textbox;
    #endregion

    #region Start Method
    private void Start()
    {
        textbox = GameObject.Find("Canvas/Text").GetComponent<Text>();
    }
    #endregion

    #region Drop Methods
    public void OnDrop(PointerEventData pointerEventData)
    {
        Draggable d = pointerEventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            childParent = d.p_parent;
            d.p_parent = this.transform;
            type = d.card_type;
        }

        if (dropzone)
        {
            OnlyOne(d);

            //StartCoroutine(CheckType());
            TypeCheck();
            
            pointerEventData.pointerDrag.GetComponent<Outline>().enabled = false;
        }

        if (acceptzone)
        {
            OnlyOne(d);

            Button ab = Instantiate(acceptButton, transform.parent);
            ab.name = "acceptButton";
            pointerEventData.pointerDrag.GetComponent<Outline>().enabled = false;
        }
    }
    IEnumerator CheckType()
    {
        Transform bg = transform.parent.GetChild(0);
        Image sprite;
        float elapsedtime;

        switch (type)
        {
            case CardType.BlackMarket:
                sprite = bg.GetComponent<Image>();
                elapsedtime = 0;

                while (sprite.color != Color.black)
                {
                    sprite.color = Color.Lerp(sprite.color, Color.black, elapsedtime / 50f);
                    elapsedtime += Time.deltaTime;

                    yield return null;
                }
                yield return null;
                break;

            case CardType.Guild:
                sprite = bg.GetComponent<Image>();
                elapsedtime = 0;

                while (sprite.color != Color.blue)
                {
                    sprite.color = Color.Lerp(sprite.color, Color.blue, elapsedtime / 50f);
                    elapsedtime += Time.deltaTime;

                    yield return null;
                }
                yield return null;
                break;

            case CardType.DeckEditor:
                sprite = bg.GetComponent<Image>();
                elapsedtime = 0;

                while (sprite.color != Color.white)
                {
                    sprite.color = Color.Lerp(sprite.color, Color.white, elapsedtime / 50f);
                    elapsedtime += Time.deltaTime;

                    yield return null;
                }
                yield return null;
                break;

            case CardType.Inventory:
                sprite = bg.GetComponent<Image>();
                elapsedtime = 0;

                while (sprite.color != Color.green)
                {
                    sprite.color = Color.Lerp(sprite.color, Color.green, elapsedtime / 50f);
                    elapsedtime += Time.deltaTime;

                    yield return null;
                }
                yield return null;
                break;

            case CardType.Options:
                sprite = bg.GetComponent<Image>();
                elapsedtime = 0;

                while (sprite.color != Color.gray)
                {
                    sprite.color = Color.Lerp(sprite.color, Color.gray, elapsedtime / 50f);
                    elapsedtime += Time.deltaTime;

                    yield return null;
                }
                yield return null;
                break;
        }
    }

    private void TypeCheck()
    {
        Transform bg = transform.parent.GetChild(0);
        Image image = bg.GetComponent<Image>();
        DestroyPrevPanel();
        GameObject q;
        GameObject a;

        switch (type)
        {
            case CardType.BlackMarket:
                image.color = Color.black;
                q = Instantiate(questUI, transform.parent);
                q.name = "questUI";
                a = Instantiate(acceptUI, transform.parent);
                a.name = "acceptUI";
                AddQuests(q);

                textbox.text = "Please Choose a Quest to Pursue";

                break;

            case CardType.Guild:
                image.color = Color.blue;
                q = Instantiate(questUI, transform.parent);
                q.name = "questUI";
                a = Instantiate(acceptUI, transform.parent);
                a.name = "acceptUI";
                AddQuests(q);

                textbox.text = "Please Choose a Quest to Pursue";

                break;

            case CardType.Inventory:
                image.color = Color.white;
                break;

            case CardType.DeckEditor:
                image.color = Color.green;
                break;

            case CardType.Options:
                image.color = Color.gray;
                break;

            case CardType.Tutorial:
                bool yn = EditorUtility.DisplayDialog("Tutorial", "Do you want to start the tutorial?", "Yes", "No");

                if (yn)
                {
                    Debug.Log("tutorial load");
                    //SceneManager.LoadScene("tutorial");
                }
                break;
        }
    }

    private void DestroyPrevPanel()
    {
        GameObject q = GameObject.Find("questUI");
        GameObject a = GameObject.Find("acceptUI");

        Destroy(q);
        Destroy(a);
    }
    
    private void OnlyOne(Draggable d)
    {
        if (transform.childCount >= 1)
        {
            transform.GetChild(0).SetParent(childParent);
            childParent = d.p_parent;
            d.p_parent = transform;
        }
    }
    #endregion

    #region Hover Methods
    public void OnPointerEnter(PointerEventData pointerEventData)
    { 

        if (pointerEventData.pointerDrag == null)
        {
            return;
        }

        Draggable d = pointerEventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }

        if (pointerEventData.pointerDrag != null && (dropzone || acceptzone))
        {
            Outline card = pointerEventData.pointerDrag.GetComponent<Outline>();
            card.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null)
        {
            return;
        }

        Draggable d = pointerEventData.pointerDrag.GetComponent<Draggable>();

        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.p_parent;
        }

        if (pointerEventData.pointerDrag != null && (dropzone || acceptzone))
        {
            Outline card = pointerEventData.pointerDrag.GetComponent<Outline>();
            card.enabled = false;
        }
    }
    #endregion

    #region Quest Methods
    private void AddQuests(GameObject parent)
    {
        /*for (int i = 0; i < 3; i += 1)
        {
            GameObject questcard = Instantiate(quests[Random.Range(0, quests.Length)], parent.transform);
            questcard.name = "quest" + i;
        }*/

        foreach (GameObject go in quests)
        {
            Instantiate(go, parent.transform);
        }
    }

    public void AcceptQuest()
    {
        Deactivate(new string[] { "hand", "questUI" });

        GameObject canvas = GameObject.Find("Canvas");

        GameObject[] instance = new GameObject[] { goButton, heroSelect, heroUI };
        foreach (GameObject go in instance)
        {
            GameObject call = Instantiate(go, canvas.transform);
            call.name = go.name;
        }

        GameObject heroSelection = GameObject.Find("heroSelect");

        foreach (GameObject hero in heroes)
        {
            GameObject hr = Instantiate(hero, heroSelection.transform);
            hr.name = hero.name;
        }

        textbox = GameObject.Find("Canvas/Text").GetComponent<Text>();
        textbox.text = "Please Choose Your Heroes";
    }

    public void goToQuest()
    {
        textbox = GameObject.Find("Canvas/Text").GetComponent<Text>();
        Hero[] selectedHeroes = GameObject.Find("heroUI").GetComponentsInChildren<Hero>();

        QuestManager questManager = GameObject.FindObjectOfType<QuestManager>();

        if (selectedHeroes.Length == 3)
        {
            questManager.partyMembers = new Hero[selectedHeroes.Length];
            for (int i = 0; i < selectedHeroes.Length; i += 1)
            {
                questManager.partyMembers[i] = Resources.Load<Hero>("prefabs/" + selectedHeroes[i].name);
            }

            SceneManager.LoadScene("Dungeon");
            Activate(new List<string>() { "hand", "questUI" }); 
        }
        else
        {
            textbox.text = "Please Choose 3 heros to join you";
        }
        

    }
    #endregion

    #region Activate Objects Methods
    private void Deactivate(string[] str)
    {
        DropArea next = GameObject.Find("Canvas/dropzone").GetComponent<DropArea>();
        foreach (string go in str)
        {
            GameObject found = GameObject.Find(go);
            next.deactivated.Add(found);
            found.SetActive(false);
        }
    }

    private void Activate()
    {
        DropArea next = GameObject.Find("Canvas/dropzone").GetComponent<DropArea>();
        foreach (GameObject go in next.deactivated)
        {
            go.SetActive(true);
        }
    }

    private void Activate(List<string> str)
    {
        DropArea next = GameObject.Find("Canvas/dropzone").GetComponent<DropArea>();
        foreach (GameObject go in next.deactivated)
        {
            if (str.Contains(go.name))
            {
                go.SetActive(true);
            }
        }
    }
    #endregion
}
