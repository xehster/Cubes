using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private const float radius = 2f;
    private const float speed = 10f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float journeyLength;
    private float startTime;
    private bool isStart;

    public void StartMovement(Vector3 start, Vector3 target)
    {
        startPosition = start;
        targetPosition = target;
        journeyLength = Vector3.Distance(startPosition, targetPosition);
        startTime = Time.time;
        isStart = true;
    }

    void Update()
    {
        if (!isStart) return;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = CalculateParabolicHeight(fracJourney);
        CheckSphereOverlap();
    }
    

    void CheckSphereOverlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Cube"))
            {
                collider.GetComponent<CubeView>().InteractWithCube();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            Destroy(gameObject);
        }

    }


    private Vector3 CalculateParabolicHeight(float t)
    {
        float x = Mathf.Lerp(startPosition.x, targetPosition.x, t);
        float y = Mathf.Lerp(startPosition.y, targetPosition.y, t) + 1f * Mathf.Sin(t * Mathf.PI);
        float z = Mathf.Lerp(startPosition.z, targetPosition.z, t);

        float arcFactor = t * (1 - t); 
        Vector3 arcOffset = new Vector3(0f, arcFactor * 2, 0f); 
        return new Vector3(x, y, z) + arcOffset; 
    }
    
}