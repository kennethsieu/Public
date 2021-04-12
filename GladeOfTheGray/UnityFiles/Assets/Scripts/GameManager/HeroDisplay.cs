using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDisplay : MonoBehaviour
{
    #region Serializard variables
    [SerializeField]
    private HealthBarDisplay healthBar;

    #endregion


    #region Private varibles
    private Damagable Char;
    private Color start;

  
    private GameObject backdrop;
    private HealthBarDisplay HealthBar;

    #endregion

    public void setChar(Damagable character, GameObject background)
    {
        Char = character;
        backdrop = background;
        HealthBar = Instantiate(healthBar, new Vector3(0, 0 ,0), Quaternion.identity);
        HealthBar.transform.SetParent(this.gameObject.transform, false);
        HealthBar.SetHealthBar(Char);
        this.gameObject.GetComponent<Image>().sprite = Char.getImages()[0];
        this.gameObject.transform.SetParent(backdrop.transform, false);
    }


    public void updateChar(Damagable character){
        Char = character;
        this.gameObject.GetComponent<Image>().sprite = Char.getImages()[0];
        HealthBar.SetHealthBar(Char);
    }


    public void enemyDie()
    {
        this.gameObject.GetComponent<Image>().sprite = Char.getImages()[3];
    }

    public void SelectTarget()
    {
        StartCoroutine(Select());
    }


    public Hero getHero()
    {
        return Char.GetComponent<Hero>();
    }


    public Enemy GetEnemy()
    {
        return Char.GetComponent<Enemy>();
    }
    public void Start()
    {
        StartCoroutine(SquashAndStetch());

    }

    #region Animation
    IEnumerator SquashAndStetch()
    {
        float height = this.gameObject.GetComponent<RectTransform>().rect.height;
        RectTransform rt = this.gameObject.GetComponent<RectTransform>() as RectTransform;
        float elapsedTime = 0;
        float scalar = 10;
        float time_scalar = 1.5f;
        while (true)
        {
            rt.sizeDelta = new Vector2(rt.rect.width, height + (Mathf.Sin(elapsedTime * time_scalar) * scalar));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Select()
    {
        this.transform.GetChild(0).GetComponent<Image>().color = new Vector4(0, 1, 1, 0.15f);
        yield return new WaitForSeconds(1);
        this.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
    }
    #endregion

    public void OnCollisionEnter2D(Collision2D collision)
    {
        start = this.transform.GetChild(0).GetComponent<Image>().color;
        if (collision.gameObject.GetComponent<Card>() != null)
        {
            Card card = collision.gameObject.GetComponent<Card>();

            if (card.validTarget(this.gameObject))
            {
                this.transform.GetChild(0).GetComponent<Image>().color = new Vector4(0, 1, 1, 0.15f);
            } else
            {
                this.transform.GetChild(0).GetComponent<Image>().color = new Vector4(0, 0, 0, 0.15f);
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        this.transform.GetChild(0).GetComponent<Image>().color = start;
    }


}
