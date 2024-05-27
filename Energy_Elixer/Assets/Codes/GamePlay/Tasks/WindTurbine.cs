using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    public Animator animator;
    public static bool isWindy;
    // Start is called before the first frame update

void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned in the Inspector.");
        }
        isWindy = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (animator != null)
        {animator.SetBool("isWindy", isWindy);
    }

}
    public void SwitchOnTurbine(){
        isWindy = true;
        GameManager.score += 12;
    }
}
