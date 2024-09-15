using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private List<GameObject> bgTiles = new();

    [SerializeField]
    private List<GameObject> prefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int x = -100; x < 100; x++)
        {
            for (int y = -10; y < 1000; y++)
            {
                int prefabIndex = Random.Range(0, prefabs.Count);
                GameObject obj = Instantiate(prefabs[prefabIndex]);
                obj.transform.position = new Vector3(x, y, 0);
                bgTiles.Add(obj);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
