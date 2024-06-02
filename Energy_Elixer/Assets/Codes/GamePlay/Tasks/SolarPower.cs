using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPower : MonoBehaviour
{
    private Notifications notification;
    private bool pointsIncreased = false;
    int hitCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        notification = FindObjectOfType<Notifications>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnParticleCollision(GameObject gameObject)
    {
        hitCount++;
        if (hitCount > 100)
        {
            if (gameObject.CompareTag("SunRays") && !pointsIncreased)
            {
                if (notification != null)
                {
                    notification.getNotificationMessage("SolarPower", true);
                }
                GameManager.score += 10;
                pointsIncreased = true;
            }   
        }
    }

}