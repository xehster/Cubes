using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CubeMovement : MonoBehaviour
{
    private float moveSpeed = 5f; 
    private float minWaitTime = 1f; 
    private float maxWaitTime = 3f; 
    [SerializeField] private GameObject plane;
    private Vector3 planeSize; 

    private Vector3 targetPosition; 
    private bool isMoving = false; 
    private float waitTime; 
    private float rotationSpeed;
    private float maxRotationSpeed = 1f;
    private float distance;
    private CubeSpawn cubeSpawn;
    private Vector3 newRandomPos;


    private void Start()
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        plane = GameObject.FindGameObjectWithTag("Plane");
        planeSize = GetPlaneSize(plane);
    }

    private void Update()
    {
        MoveCube();
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
    
    private void MoveCube()
    {
        if (!isMoving)
        {
            waitTime -= Time.deltaTime;

            if (waitTime <= 0f)
            {
                targetPosition = GetRandomPointInPlane(plane);
                StartCoroutine(RotateCube(targetPosition, rotationSpeed));
                isMoving = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                isMoving = false;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }
    
    private IEnumerator RotateCube(Vector3 target, float rotationSpeed)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            distance = Vector3.Distance(transform.position, target);
            rotationSpeed = maxRotationSpeed * distance;
            yield return null;
        }
    }
    
    


}
