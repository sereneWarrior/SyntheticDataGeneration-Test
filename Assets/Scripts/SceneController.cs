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

    private LabeledObject[] _activeObjects;
    private Dictionary<string, LinkedPool<GameObject>> _pools;

    void Start()
    {
        _activeObjects = new LabeledObject[maxObjectsPerFrame];
        _pools = new Dictionary<string, LinkedPool<GameObject>>();
        foreach (var prefab in prefabs)
        {
            _pools.Add(LayerMask.LayerToName(prefab.layer), 
                new LinkedPool<GameObject>(createFunc: () => Instantiate(prefab),
                actionOnGet: g => g.OnActionGet(),
                actionOnRelease: g => g.OnActionRelease(),
                actionOnDestroy: g => Destroy(g),
                collectionCheck: false,
                maxSize: maxObjectsPerFrame)); 
        }
        GenerateObjects();
        StartCoroutine("WaitBeforeGeneration");
    }

    IEnumerator WaitBeforeGeneration()
    {
        for(;;)
        {
            yield return new WaitForSeconds(2f);
            synth.OnSceneChange();
            // TODO: Find better solution to release all spawned object
            foreach (var obj in _activeObjects){
                _pools[obj.Label].Release(obj.Object);
            }
            GenerateObjects();
        }
    }

    private void GenerateObjects()
    {
        for (var i= 0; i < maxObjectsPerFrame; i++)
        {
            var rnd = Random.Range(0,prefabs.Length);
            var newObj = _pools.ElementAt(rnd).Value.Get();
            _activeObjects[i] = new LabeledObject{
                Label = _pools.ElementAt(rnd).Key,
                Object = newObj
            };
        }
    }
}
