using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Editor Variables
    [SerializeField]
    private bool menu_card = false;

    [SerializeField]
    private bool hero_card = false;

    [SerializeField]
    private bool quest_card = false;

    public enum CardType
    {
        BlackMarket,
        Guild,
        Inventory,
        DeckEditor,
        Options,
        Tutorial
    }

    [ConditionalHide("menu_card", true)]
    [Tooltip("The type of card")]
    public CardType card_type;

    public enum heroType
    {
        Witch,
        Gambler
    }

    [ConditionalHide("hero_card", true)]
    public heroType hero_Type;

    [ConditionalHide("quest_card", true)]
    public string scene;
    #endregion

    #region Variables
    [HideInInspector]
    public Transform p_parent = null;

    private Vector3 p_scale;

    private bool onCard = false;

    private GameObject placeHolder = null;

    [HideInInspector]
    public Transform placeHolderParent = null;
    #endregion

    #region Start
    private void Start()
    {
        p_scale = transform.localScale;

        switch (hero_Type)
        {
            case heroType.Witch:
                gameObject.AddComponent(typeof(Witch));
                break;

            case heroType.Gambler:
                gameObject.AddComponent(typeof(Gambler));
                break;
        }
        

    }
    #endregion

    #region Drag Method
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        p_parent = this.transform.parent;
        placeHolderParent = p_parent;

        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        this.transform.position = pointerEventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }

        int newSiblinIndex = placeHolderParent.childCount;

        for (int i = 0; i < placeHolderParent.childCount; i += 1)
        {
            if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
            {
                newSiblinIndex = i;
                
                if (placeHolder.transform.GetSiblingIndex() < newSiblinIndex)
                {
                    newSiblinIndex -= 1;
                }

                break;
            }
        }

        placeHolder.transform.SetSiblingIndex(newSiblinIndex);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        this.transform.SetParent(p_parent);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeHolder);
    }
    #endregion

    #region Hover Method
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        onCard = true;

        StartCoroutine(enlarge(pointerEventData));
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        onCard = false;
        StopCoroutine(enlarge(pointerEventData));
        transform.localScale = p_scale;
    }

    IEnumerator enlarge(PointerEventData pointerEventData)
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 newScale = new Vector3(1.5f, 1.5f, 1.5f);

        if (onCard && !pointerEventData.dragging)
        {
            transform.localScale = newScale;
        }

        yield return null;
    }
    #endregion
}
