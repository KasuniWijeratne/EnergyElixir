using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestSceneHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.OnPlayerEnvironmentChanged += OnPlayerEnvironmentChanged;
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void OnPlayerEnvironmentChanged(object sender, int e)
    {
        Text.text ="" + e;
    }
}
