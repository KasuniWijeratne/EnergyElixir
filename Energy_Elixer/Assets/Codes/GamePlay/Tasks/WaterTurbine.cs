using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTurbine : MonoBehaviour
{
    bool isRaining;
    public Animator animator;

    private float ParticleCollisionStartTime;
    private float ParticleNoCollideTime = 0.2f;
    int hitCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("isRaining", isRaining);
        Debug.Log(isRaining);

        if (isRaining && Time.time - ParticleCollisionStartTime >= ParticleNoCollideTime)
        {
            isRaining = false;
        }
    }

    private void OnParticleCollision(GameObject particle)
    {
        hitCount++;
        ParticleCollisionStartTime = Time.time;
        Debug.Log("Raining");
        if(hitCount > 200)
            isRaining = true;
    }

}
