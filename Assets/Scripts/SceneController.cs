using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public ImageSynthesis synth;

    public GameObject[] prefabs;
    public int maxObjectsPerFrame;

    private PoolController _pool;
    private Dictionary<string, GameObject> objs;

    //private ObjectPool<GameObject> _pool;
    // Start is called before the first frame update
    void Start()
    {
        objs = prefabs.ToDictionary(p => LayerMask.LayerToName(p.layer), p=> p );
        _pool = new PoolController(objs);
        GenerateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        synth.OnSceneChange();
        _pool.ReclaimAll();
        GenerateObjects();
    }

    private void GenerateObjects()
    {
        //generate random 0..3
        for (var i= 0; i < maxObjectsPerFrame; i++)
        {
            var obj = _pool.GetFromPool(objs.ElementAt(Random.Range(0,3)));
            var randomPosition = new Vector3(
                Random.Range(-11, 11),
                Random.Range(5, 11),
                Random.Range(11, -11)
            ); 

            obj.transform.position = randomPosition;
            obj.transform.rotation = Random.rotation;
            obj.GetComponent<Renderer>().material.color = Random.ColorHSV();
            var scale = Random.Range(-3, 3);
            obj.transform.localScale +=  new Vector3(
                scale,
                scale,
                scale
            );
        }
    }
}
