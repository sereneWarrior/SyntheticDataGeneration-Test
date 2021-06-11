using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public ImageSynthesis synth;

    public GameObject[] prefabs;
    public int maxObjectsPerFrame;

    private GameObject[] createdGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        createdGameObjects = new GameObject[maxObjectsPerFrame];
        GenerateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        synth.OnSceneChange();
        GenerateObjects();
    }

    private void GenerateObjects()
    {
        //TODO: Object pooling
        foreach(var obj in createdGameObjects)
        {
            Destroy(obj);
        }

        //generate random 0..3
        for (var i= 0; i < maxObjectsPerFrame; i++)
        {
            var randomPosition = new Vector3(
                Random.Range(-11, 11),
                Random.Range(5, 11),
                Random.Range(11, -11)
            ); 


            var randomObject = Instantiate(prefabs[Random.Range(0,prefabs.Length)],
            randomPosition, 
            Random.rotation);

            randomObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
            var scale = Random.Range(-3, 3);
            randomObject.transform.localScale +=  new Vector3(
                scale,
                scale,
                scale
            );

            createdGameObjects[i] = randomObject;
        }
    }
}
