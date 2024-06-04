using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvManager : MonoBehaviour
{
    public GameObject worseEnv;
    public GameObject badEnv;
    public GameObject goodEnv;
    public GameObject bestEnv;
    private GameObject currentEnvironment;

    // Start is called before the first frame update
    public int gameCondition;
    private int prevGameCondition;

    public float transitionDuration = 2f;

    void Awake(){
        PlayerManager.Instance.OnPlayerEnvironmentChanged += OnPlayerEnvironmentChanged;
    }
    void Start()
    {
        
        SetInitialEnv(goodEnv);
        prevGameCondition = gameCondition;
    }

    // Update is called once per frame

    void Update(){
        if(prevGameCondition != gameCondition){
            updateEnvirontment();
            prevGameCondition = gameCondition;
        }
    }

    private void OnPlayerEnvironmentChanged(object sender, int e)
    {
        gameCondition =  e;
    }

    private void SetInitialEnv(GameObject initialEnv){
        if(initialEnv != null){
            initialEnv.SetActive(true);
            currentEnvironment = initialEnv;
        }
    }

    void updateEnvirontment()
    {
       switch (gameCondition)
        {
            case 1:
                StartCoroutine(Transition(worseEnv));
                break;
            case 2:
                StartCoroutine(Transition(badEnv));
                break;
            case 3:
                StartCoroutine(Transition(goodEnv));
                break;
            case 4:
                StartCoroutine(Transition(bestEnv));
                break;
            default:
                break;
        }
        
    }

    private IEnumerator Transition(GameObject targetEnv)
    {
        targetEnv.SetActive(true);
        if(targetEnv == currentEnvironment)
        {
            yield break;
        }
        if (currentEnvironment != null)
        {
            yield return StartCoroutine(FadeInEnvironment(targetEnv));
            yield return StartCoroutine(FadeOutEnvironment(currentEnvironment));
        }
        currentEnvironment = targetEnv;

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
