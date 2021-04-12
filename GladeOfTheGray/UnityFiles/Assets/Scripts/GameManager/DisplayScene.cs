using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScene : MonoBehaviour
{
    [SerializeField]
    private GameObject ImageHolder;

    [SerializeField]
    private GameObject background;



    public void BattleDispayEnemyAttack(List<Hero> ally, Enemy enemy, List<Vector3> allyPos, Vector3 enemyPos, List<string> damage)
    {
        List<GameObject> stores = new List<GameObject>();
        
        GameObject temp = Instantiate(ImageHolder, enemyPos, Quaternion.identity);
        temp.GetComponent<Image>().sprite = enemy.getImages()[1];
        temp.transform.SetParent(this.gameObject.transform, false);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector3(500, 550, 0);
        stores.Add(temp);

        for (int i = 0; i < ally.Count; i++)
        {
            temp = Instantiate(ImageHolder, allyPos[i], Quaternion.identity);
            temp.GetComponent<Image>().sprite = ally[i].getImages()[2];
            temp.GetComponentInChildren<Text>().text = damage[i];
            temp.transform.SetParent(this.gameObject.transform, false);
            if (!ally[i].isAlive())
            {
                StartCoroutine(Death(temp));
            }
            stores.Add(temp);
        }

        foreach (GameObject thing in stores)
        {
            Destroy(thing, 1.8f);
        }
    }

    public void BattleDispayAllyAttack(Hero ally, List<Enemy> enemy, List<Vector3> enemyPos, Vector3 allyPos, List<string> damage)
    {
        List<GameObject> stores = new List<GameObject>();

        GameObject temp = Instantiate(ImageHolder, allyPos, Quaternion.identity);
        temp.GetComponent<Image>().sprite = ally.getImages()[1];
        temp.transform.SetParent(this.gameObject.transform, false);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector3 (500, 550, 0);
        stores.Add(temp);

        for (int i = 0; i < enemy.Count; i++)
        {
           
            temp = Instantiate(ImageHolder, enemyPos[i], Quaternion.identity);
            temp.GetComponent<Image>().sprite = enemy[i].getImages()[2];
            temp.transform.SetParent(this.gameObject.transform, false);
            temp.GetComponentInChildren<Text>().text = damage[i];
            if(enemy[i].getCurHealth() == 0)
            {
                StartCoroutine(Death(temp));
            }
            stores.Add(temp);
        }

        foreach (GameObject thing in stores)
        {
            Destroy(thing, 1.8f);
        }
    }

    public void BattleDisplaySelf(Damagable target, Vector3 pos, string text="")
    {
        GameObject temp = Instantiate(ImageHolder, pos, Quaternion.identity);
        temp.transform.SetParent(this.gameObject.transform, false);
        temp.GetComponent<Image>().sprite = target.getImages()[0];
        temp.GetComponentInChildren<Text>().text = text;
        Destroy(temp, 1.8f);
        
    }

    public void BattleDisplayBase(List<Damagable> targets, List<Vector3> pos, List<string> text, List<int> pose)
    {
        List<GameObject> stores = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < targets.Count; i++)
        {
            temp = Instantiate(ImageHolder, pos[i], Quaternion.identity);
            temp.GetComponent<Image>().sprite = targets[i].getImages()[pose[i]];

            temp.transform.SetParent(this.gameObject.transform, false);

            if(pose[i] == 1)
            {
                temp.GetComponent<RectTransform>().sizeDelta = new Vector3(500, 550, 0);
            }
            temp.GetComponentInChildren<Text>().text = text[i];
            stores.Add(temp);
        }


        foreach (GameObject thing in stores)
        {
            Destroy(thing, 1.8f);
        }
    }

    IEnumerator Death(GameObject image)
    {
        float elapsedTime = 0;
        Color start = image.GetComponent<Image>().color;
        while (image.GetComponent<Image>().color != Color.black)
        {
            image.GetComponent<Image>().color = Color.Lerp(start, Color.black, elapsedTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }



}
