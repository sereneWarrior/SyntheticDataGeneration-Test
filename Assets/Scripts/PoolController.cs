using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : ScriptableObject
{
    private Dictionary<string, GameObject> _prefabs;
    private List<Shape> _activeObjects = new List<Shape>();
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();


   public PoolController(Dictionary<string, GameObject> prefabs)
   {
       _prefabs = prefabs;
     
        foreach(var label in _prefabs.Keys)
        {
            _poolDictionary[label] = new Queue<GameObject>();
        }
   }

   public GameObject GetFromPool(KeyValuePair<string, GameObject> objectToSpawn)
   {
        var label = objectToSpawn.Key;
        var prefab = objectToSpawn.Value;

        GameObject newObject;
        Queue<GameObject> pool;    

        if(!_poolDictionary.TryGetValue(label, out pool))
        {
            Debug.LogWarning($"There is no pool existing for the label {label}.");
            return null;
        }

            if(pool.Count <= 0)
            {
                newObject = Instantiate(prefab); // TODO: Transform needs to be set in SceneController
                pool.Enqueue(newObject);
            }
            else
            {
                newObject = pool.Dequeue();
            }
            
            newObject.SetActive(true);

            _activeObjects.Add(new Shape{
                Label = label,
                Object = newObject
            });
            return newObject;
   }

   public void ReclaimAll()
   {
       foreach(var obj in _activeObjects)
       {
           obj.Object.SetActive(false);
           obj.Object.transform.position = Vector3.zero;
           obj.Object.transform.localScale = Vector3.zero;
           obj.Object.transform.rotation = Quaternion.identity;
           
            _poolDictionary[obj.Label].Enqueue(obj.Object);
       }
       
   }
}
