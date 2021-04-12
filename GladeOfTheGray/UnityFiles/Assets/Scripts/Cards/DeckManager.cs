using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager: MonoBehaviour
{
    public GameObject pass;
    public GameObject move;

    #region Private Variables
    private List<GameObject> deck;
    private List<GameObject> discard;
    private List<GameObject> inPlay;
    private List<GameObject> clones = new List<GameObject>();
    private GameObject playerArea;
    #endregion

    #region Methods
    public void LoadDeck()
    {
        deck = new List<GameObject>();
        discard = new List<GameObject>();
        inPlay = new List<GameObject>();
        playerArea = GameObject.FindWithTag("PlayerArea");
        GameObject[] deckTemp = this.GetComponent<Hero>().getDeck();
        foreach(GameObject card in deckTemp)
        {
            discard.Add(card);
        }
        Shuffle();
    }


    public void Shuffle()
    {
        while(discard.Count != 0)
        {
            int temp = Random.Range(0, discard.Count);
            deck.Add(discard[temp]);
            discard.RemoveAt(temp);
        }
    }

    public void Draw()
    {
        if(deck.Count == 0 && discard.Count == 0)
        {
           
        } else if(deck.Count == 0)
        {
            Shuffle();
        }
        GameObject card = deck[0];
        GameObject clone = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
        clones.Add(clone);
        clone.transform.SetParent(playerArea.transform, false);
        deck.RemoveAt(0);
        inPlay.Add(card);
    }

    public void Draw(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Draw();
        }
    }

    public void EndTrun()
    {
        foreach(GameObject clone in clones)
        {
            Destroy(clone);
        }
        while (inPlay.Count != 0)
        {
            discard.Add(inPlay[0]);
            inPlay.RemoveAt(0);
        }
    }

    public void DrawPass()
    {
        GameObject clone = Instantiate(pass, new Vector3(0, 0, 0), Quaternion.identity);
        clones.Add(clone);
        clone.transform.SetParent(playerArea.transform, false);
    }

    public void DrawMove()
    {
        GameObject clone = Instantiate(move, new Vector3(0, 0, 0), Quaternion.identity);
        clones.Add(clone);
        clone.transform.SetParent(playerArea.transform, false);
    }


    public List<GameObject> getInPlay()
    {
        return clones;
    }

    public void Discard(GameObject card)
    {
        int index = clones.IndexOf(card);
        Destroy(clones[index]);
        clones.RemoveAt(index);
    }

    #endregion

    #region Start
    public void Start()
    {

    }
    #endregion

}
