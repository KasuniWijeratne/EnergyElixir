using System.Collections;
using UnityEngine;

public class ParallaxBG1 : MonoBehaviour
{
    public Transform mainCam;
    private Transform middleBG;
    private Transform sideBG;

    public GameObject bestMiddleBG;
    public GameObject bestSideBG;
    public GameObject goodMiddleBG;
    public GameObject goodSideBG;
    public GameObject badMiddleBG;
    public GameObject badSideBG;
    public GameObject worstMiddleBG;
    public GameObject worstSideBG;

    private GameEnvManager gameEnvManager;

    private GameObject currentMiddleBG;
    private GameObject currentSideBG;
    public float length = 28f;
    public float trasitionDuration = 1f;

    private int currentGameCondition;


    void Start()
    {
        gameEnvManager = FindObjectOfType<GameEnvManager>();
        if (gameEnvManager == null)
        {
            Debug.LogError("GameEnvManager not found in the scene.");
        }

        SetInitialBackgrounds(goodMiddleBG, goodSideBG);

    }
    // Update is called once per frame
    void Update()
    {
        if (mainCam.position.x > middleBG.position.x)
        {
            sideBG.position = middleBG.position + Vector3.right * length;
        }
        if (mainCam.position.x < middleBG.position.x)
        {
            sideBG.position = middleBG.position + Vector3.left * length;
        }
        if (mainCam.position.x > sideBG.position.x || mainCam.position.x < sideBG.position.x)
        {
            Transform z = middleBG;
            middleBG = sideBG;
            sideBG = z;

        }
        
            if(currentGameCondition != gameEnvManager.gameCondition){
                currentGameCondition = gameEnvManager.gameCondition;
                switch (currentGameCondition)
                {
                    case 1:
                        ChangeBackground(worstMiddleBG, worstSideBG);
                        break;
                    case 2:
                        ChangeBackground(badMiddleBG, badSideBG);
                        break;
                    case 3:
                        ChangeBackground(goodMiddleBG, goodSideBG);
                        break;
                    case 4:
                        ChangeBackground(bestMiddleBG, bestSideBG);
                        break;
                    default:
                        break;
                }
        }
    }


    private void ChangeBackground(GameObject newMiddleBG, GameObject newSideBG)
    {
        StartCoroutine(SwitchBackground(newMiddleBG, newSideBG));
    }

    private void SetInitialBackgrounds(GameObject initialMiddleBG, GameObject initialSideBG)
    {
        currentMiddleBG = initialMiddleBG;
        currentSideBG = initialSideBG;
        middleBG = currentMiddleBG.transform;
        sideBG = currentSideBG.transform;
        currentMiddleBG.SetActive(true);
        currentSideBG.SetActive(true);
    }

    private IEnumerator SwitchBackground(GameObject newMiddleBG, GameObject newSideBG)
    {
        newMiddleBG.SetActive(true);
        newSideBG.SetActive(true);

        newMiddleBG.transform.position = middleBG.position;
        newSideBG.transform.position = sideBG.position;


        middleBG = newMiddleBG.transform;
        sideBG = newSideBG.transform;

        if (currentMiddleBG != null && currentSideBG != null)
        {
            yield return StartCoroutine(FadeInBackground(newMiddleBG));
            yield return StartCoroutine(FadeInBackground(newSideBG));

            yield return StartCoroutine(FadeOutBackground(currentMiddleBG));
            yield return StartCoroutine(FadeOutBackground(currentSideBG));
        }

        currentMiddleBG = newMiddleBG;
        currentSideBG = newSideBG;

    }

    private IEnumerator FadeOutBackground(GameObject bg)
    {
        float elapsedTime = 0f;
        Renderer[] renderers = bg.GetComponentsInChildren<Renderer>();
        while (elapsedTime < trasitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / trasitionDuration);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    Color color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }
            yield return null;
        }
        bg.SetActive(false);
    }

    private IEnumerator FadeInBackground(GameObject bg)
    {
        float elapsedTime = 0f;
        Renderer[] renderers = bg.GetComponentsInChildren<Renderer>();
        while (elapsedTime < trasitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / trasitionDuration);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    Color color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }
            yield return null;
        }
    }
}
