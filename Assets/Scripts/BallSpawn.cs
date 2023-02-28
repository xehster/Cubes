using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawn : MonoBehaviour
{

    [SerializeField] private Transform uiTransform;
    [SerializeField] private GameObject uiElement;
    [SerializeField] private Text timerCounterText;
    private Vector3 targetPosition;
    public GameObject ballPrefab;

    private bool canFire = true;
    private int maxBallCount = 5;
    private int currentBallCount;

    private Plane groundPlane;
    
    private float timer;
    private float timeBetweenAddBall = 1;
    private float tillNewBallTime;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }


    private void ClickMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.transform.position = startPosition;
            Destroy(uiTransform.GetChild(currentBallCount - 1).gameObject);
            currentBallCount -= 1;
            ball.GetComponent<BallMovement>().StartMovement(startPosition, targetPosition);

            StartCoroutine(FireCooldown());
        }
    }

    private void Update()
    {
        Counter();


        if (canFire && Input.GetMouseButtonDown(0) && currentBallCount > 0)
        {
            ClickMouse();
        }

    }
    
    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(0.5f);
        canFire = true;
    }

    private void Counter()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAddBall)
        {
            if (currentBallCount <= maxBallCount)
            {
                Instantiate(uiElement, uiTransform);
                currentBallCount += 1;
                timer = 0f;
            }
        }
        
        tillNewBallTime = 1f - timer;
        if (currentBallCount >= maxBallCount)
        {
            timer = 0f;
            tillNewBallTime = 0f;
        }
        

        if (tillNewBallTime <= 0)
        {
            tillNewBallTime = 0f;
        }
        timerCounterText.text = $"New ball available in: {tillNewBallTime.ToString("0.##")}";
    }
}

