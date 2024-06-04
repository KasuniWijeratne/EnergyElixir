using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalLight : MonoBehaviour
{
    // Start is called before the first frame update
    private bool playerInRange;
    public GameObject closeWindow;
    public static bool isWindowOpen;

    void Start() {
        isWindowOpen = false;
     }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(OpenWindow());
            isWindowOpen = true;
        }
    }

    private IEnumerator OpenWindow()
    {
        float elapsedTime = 0f;
        float duration = 2f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
            foreach (Material material in closeWindow.GetComponent<Renderer>().materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
            yield return null;
        }
        closeWindow.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
