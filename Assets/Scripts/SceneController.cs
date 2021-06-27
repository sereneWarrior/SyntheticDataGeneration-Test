using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SceneController : MonoBehaviour
{
    public ImageSynthesis synth;
    public GameObject[] prefabs;
    public int maxObjectsPerFrame;

    private PoolController _pool;
    private Dictionary<string, GameObject> _objs;
    private Shape[] _activeObjects;
    private Dictionary<string, LinkedPool<GameObject>> d;

    //private ObjectPool<GameObject> _pool;
    // Start is called before the first frame update
    void Start()
    {
       // _objs = prefabs.ToDictionary(p => LayerMask.LayerToName(p.layer), p=> p );
        //_pool = new PoolController(_objs);
        _activeObjects = new Shape[maxObjectsPerFrame];
        d = new Dictionary<string, LinkedPool<GameObject>>();
        foreach (var p in prefabs)
        {
            d.Add(LayerMask.LayerToName(p.layer), 
                new LinkedPool<GameObject>(createFunc: () => Instantiate(p),
                actionOnGet: (g) => PoolHelper.OnActionGet(g),// set activ, settransform etc
                actionOnRelease: (g) => PoolHelper.OnActionRelease(g),
                actionOnDestroy: g => Destroy(g),
                collectionCheck: false,
                maxSize: 10));
                
        }
        
        GenerateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        synth.OnSceneChange();
        foreach (var obj in _activeObjects){
            d[obj.Label].Release(obj.Object);
        }
        //activeObjects.Clear;
        GenerateObjects();
    }

    private void GenerateObjects()
    {
        for (var i= 0; i < maxObjectsPerFrame; i++)
        {
            var rnd = Random.Range(0,3);
            var newObj = d.ElementAt(rnd).Value.Get();
            _activeObjects[i] = new Shape{
                Label = d.ElementAt(rnd).Key,
                Object = newObj
            };
        }
       /* for (var i= 0; i < maxObjectsPerFrame; i++)
        {
            var obj = _pool.GetFromPool(_objs.ElementAt(Random.Range(0,3)));
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
        }*/
    }
}
