using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public float offsetY;
    public List<GameObject> terrainObjects;
    private GameObject spawnObject;
    private int lastIndex = 4;

    //private void Start()
    //{
    //    CheckPosition();
    //}

    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void OnGetPointEvent(int obj)
    {
        CheckPosition();
    }

    public void CheckPosition()
    {
        if (transform.position.y - Camera.main.transform.position.y < offsetY / 2)
        {
            SpawnTerrain();
        }
    }
    private void SpawnTerrain()
    {
        int randomIndex = Random.Range(0, terrainObjects.Count);

        while (lastIndex == randomIndex)
        {
            randomIndex = Random.Range(0, terrainObjects.Count);
        }
        lastIndex = randomIndex;
        spawnObject = terrainObjects[randomIndex];

        switch (randomIndex)
        {
            case 0:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
                break;
            case 1:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY - 3, 0);
                break;
            case 2:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY + 3, 0);
                break;
            case 3:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY + 6, 0);
                break;
            case 4:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY + 6, 0); ;
                break;
            default:
                transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
                break;
        }

        //if(randomIndex == 3 || randomIndex == 4)
        //{
        //    transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY + 6, 0);
        //}
        //else
        //{
        //    transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
        //}
        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }
}
