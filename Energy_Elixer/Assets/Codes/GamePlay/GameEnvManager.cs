using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvManager : MonoBehaviour
{
    public GameObject badEnv;
    public GameObject goodEnv;
    private GameObject currentEnvironment;

    public ParallaxBG1 parallaxBG1;
    // Start is called before the first frame update
    public int gameCondition;
    private int prevGameCondition;

    public float transitionDuration = 2f;
    void Start()
    {
        
         updateEnvirontment();
    }

    // Update is called once per frame

    void Update(){
        if(prevGameCondition != gameCondition){
            updateEnvirontment();
            prevGameCondition = gameCondition;
        }
    }

    void updateEnvirontment()
    {
       switch (gameCondition)
        {
            case 1:
                StartCoroutine(Transition(badEnv));
                break;
            case 2:
                StartCoroutine(Transition(goodEnv));
                break;
            default:
                break;
        }
        
    }

    private IEnumerator Transition(GameObject to)
    {
        if(to == currentEnvironment)
        {
            yield break;
        }
        if (currentEnvironment != null)
        {
            StartCoroutine(FadeOutEnvironment(currentEnvironment));
        }

        if (to != null){
            to.SetActive(true);
            StartCoroutine(FadeInEnvironment(to));
        }

        currentEnvironment = to;

    }

    private IEnumerator FadeOutEnvironment(GameObject environment)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / transitionDuration);
            SetEnvironmentAlpha(environment, alpha);
            yield return null;
        }
        environment.SetActive(false);
    }
    private IEnumerator FadeInEnvironment(GameObject environment)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / transitionDuration);
            SetEnvironmentAlpha(environment, alpha);
            yield return null;
        }
    }

    private void SetEnvironmentAlpha(GameObject environment, float alpha)
    {
        Renderer[] renderers = environment.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
    }
}
}
