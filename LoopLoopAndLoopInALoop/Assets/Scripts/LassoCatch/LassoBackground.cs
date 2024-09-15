using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private List<GameObject> bgTiles = new();
    private List<GameObject> fgTiles = new();

    [SerializeField]
    private List<GameObject> bgPrefabs;

    [SerializeField]
    private List<GameObject> fgPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int x = -100; x < 100; x++)
        {
            for (int y = -10; y < 1000; y++)
            {
                int prefabIndex = Random.Range(0, bgPrefabs.Count);
                GameObject obj = Instantiate(bgPrefabs[prefabIndex], transform);
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
