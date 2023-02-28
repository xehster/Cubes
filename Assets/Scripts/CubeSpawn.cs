
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CubeSpawn : MonoBehaviour
{
    private const string counterText = "Cubes Destroyed";
    private List<CubeView> cubes = new List<CubeView>();
    [SerializeField] GameObject plane;
    [SerializeField] private Text cubeCounter;
    private int initialCubeCount = 3;
    [SerializeField] private CubeView cubePrefab;
    private CubeMovement cubeMovement;
    private Vector3 planeSize;
    private int cubeDestroyedCounter;
    private int minSpawnTime = 1;
    private int maxSpawnTime = 3;
   
    private void Start()
    {
        GetPlaneSize(plane);
        planeSize = GetPlaneSize(plane);
        InitialCubesSpawn();
        
    }


    private void InitialCubesSpawn()
    {

        for (int i = 0; i < initialCubeCount; i++)
        {
            Vector3 randomPoint = GetRandomPointInPlane(plane);
            SpawnCube(randomPoint);
        }
    }

    private void SpawnCube(Vector3 position)
    {
        CubeView spawnedCube = Instantiate(cubePrefab, position, Quaternion.identity);
        cubes.Add(spawnedCube);
        spawnedCube.OnCubeClick += OnCubeDestroy;
    }

    private void OnCubeDestroy(CubeView cubeView)
    {
        ChangeCounter(1);
        cubes.Remove(cubeView);
        Destroy(cubeView.gameObject);
        StartCoroutine(RespawnCube());
    }

    private void ChangeCounter(int count)
    {
        cubeDestroyedCounter += count;
        cubeCounter.text = $"{counterText}: {cubeDestroyedCounter}";
    }

    public IEnumerator RespawnCube()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

        if (cubes.Count < initialCubeCount)
        {
            Vector3 randomPoint = GetRandomPointInPlane(plane);
            SpawnCube(randomPoint);
        }
    }
    
    private Vector3 GetRandomPointInPlane(GameObject plane)
    {
        
        float randomX = Random.Range(-planeSize.x / 2 + 1, planeSize.x / 2 - 1);
        float randomZ = Random.Range(-planeSize.z / 2 + 1, planeSize.z / 2 - 1);

        return new Vector3(randomX, 0.5f, randomZ);
    }
    
    private Vector3 GetPlaneSize(GameObject plane)
    {
        Renderer renderer = plane.GetComponent<Renderer>();
        Vector3 planeSize = renderer.bounds.size;

        return planeSize;
    }
    
}
