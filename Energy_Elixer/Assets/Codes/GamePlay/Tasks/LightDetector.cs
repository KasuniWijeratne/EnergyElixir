using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public Transform sun;
    private Notifications notification;
    public float detectionRange = 100f;
    public LayerMask detectionLayer;
    public int hitThreshold = 50;
    private int hitCount = 0;
    private bool pointsIncreased = false;
    public static bool isBlocked = true;

    // Start is called before the first frame update
    void Start()
    {
        notification = FindObjectOfType<Notifications>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - sun.position;
        RaycastHit2D hit = Physics2D.Raycast(sun.position, direction, detectionRange, detectionLayer);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            hitCount++;
            if (hitCount >= hitThreshold && !pointsIncreased && isBlocked)
            {
                Debug.Log("Sun rays are hitting the solar panel!");
                isBlocked = false;
                if (notification != null)
                {
                    notification.getNotificationMessage("SolarPower", !isBlocked);
                }
                GameManager.score += 10;
                pointsIncreased = true;
            }
            
        }
        else
        {
            hitCount = 0;
            if (!isBlocked && pointsIncreased)
            {
                Debug.Log("The cloud is blocking the solar panel again!");
                isBlocked = true;
                if (notification != null)
                {
                    notification.getNotificationMessage("SolarPower", !isBlocked);
                }
                pointsIncreased = false ;
                GameManager.score -= 10;
            }

        }
    }

    
}
