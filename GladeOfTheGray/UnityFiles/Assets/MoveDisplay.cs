using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisplay : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject thing = this.transform.GetChild(0).gameObject;
            Vector3 currentPos = thing.transform.position;
            thing.transform.position = new Vector3(0, 0, 0);
        }
    }

    IEnumerator move()
    {
        GameObject thing = this.transform.GetChild(0).gameObject;
        Vector3 currentPos = thing.transform.position;
        Vector3 Gotoposition = new Vector3(0, 0, 0);
        float elapsedTime = 0;
        float waitTime = 3f;
        currentPos = transform.position;
        while (elapsedTime < waitTime)
        {
            thing.transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        transform.position = Gotoposition;
        yield return null;


    }
}
