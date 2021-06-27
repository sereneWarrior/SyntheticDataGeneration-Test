using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolHelper
{
   public static void OnActionGet(GameObject gameObject)
   {
        var randomPosition = new Vector3(
                Random.Range(-11, 11),
                Random.Range(50, 110),
                Random.Range(11, -11)
            ); 

        gameObject.transform.position = randomPosition;
        gameObject.transform.rotation = Random.rotation;
        gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
        var scale = Random.Range(-3, 3);
        gameObject.transform.localScale +=  new Vector3(
            scale,
            scale,
            scale
        );
       gameObject.SetActive(true);
   }

    public static void OnActionRelease(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
