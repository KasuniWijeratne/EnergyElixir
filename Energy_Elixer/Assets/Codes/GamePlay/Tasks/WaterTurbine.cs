using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTurbine : MonoBehaviour
{
    public static bool isRaining;
    public Animator animator;
    private Notifications notification;

    private float ParticleCollisionStartTime;
    private float ParticleNoCollideTime = 0.2f;

    private bool pointsIncreased = false;
    int hitCount = 0;

    // Update is called once per frame

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned in the Inspector in water turbine.");
        }

        notification = FindObjectOfType<Notifications>();
        if (notification == null)
        {
            Debug.LogError("No Notifications object found in the scene.");
        }
    }

    void FixedUpdate()
    {
        animator.SetBool("isRaining", isRaining);

        if (isRaining && Time.time - ParticleCollisionStartTime >= ParticleNoCollideTime)
        {
            isRaining = false;
        }
    }

    private void OnParticleCollision(GameObject particle)
    {
        hitCount++;
        ParticleCollisionStartTime = Time.time;
        if (hitCount > 200)
        {
            isRaining = true;

            if (!pointsIncreased)
            {
                if (notification != null)
                {
                    notification.getNotificationMessage("HydroPower", isRaining);
                }
                GameManager.score += 20;
                pointsIncreased = true;
            }
        }
    }
}
