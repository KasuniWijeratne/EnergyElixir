using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class VisualNovelHandler : MonoBehaviour {

    #region variables
    public string scriptFilePath;
    public string characterLocationFilePath = "Assets/Resources/VNScripts/1st_scene_characters.csv"; 
    private string defaultScriptFilePath = "Assets/Resources/VNScripts/1st_scene_test.csv";

    [SerializeField] BackgroundManager backgroundManager;
    [SerializeField] ChoiceManager choiceManager;
    [SerializeField] ConversationManager dialogueManager;
    [SerializeField] GameObject backBtn;

    // Start is called before the first frame update

    private Dictionary<int, string[]> script;
    string[] currentLine;
    int currentLineNumber;
    bool direction; //true for forward, false for backward
    string currentLeftCharacter;
    string currentRightCharacter;
    string currentMiddleCharacter;
    List<string[]> backgroundHistory = new List<string[]>(); // [0] - enabled, [1] - location
    System.Action<string> ReturnData;

    string[][] choices;
    #endregion

    private void Awake() {
        choiceManager.SetOnselectionMade(OnChoiceMade);
        dialogueManager.SetNextBtnPushedAction(OnNextBtnPressed);
        if(scriptFilePath == null) {
            scriptFilePath = defaultScriptFilePath;
            UpdateScript(scriptFilePath);
        }

        StopVisualNovel();
    }

    void Start() {
        dialogueManager.SetCharacterLocations(ReadCharacterLocationCSV(characterLocationFilePath));
    }

    //internal functions
    public void StartVisualNovel(int lineNumber = 0) {
        direction = true;
        backBtn.SetActive(true);
        updateLine(lineNumber);
    }

    public void StopVisualNovel() {
        choiceManager.disableChoiceManager();
        dialogueManager.disableConversationManager();
        EnableBackground(false);
        backBtn.SetActive(false);
    }

    private void updateLine(int lineNumber = 0) {
        if(lineNumber == -1) {
            PreviousScene();
            return;
        }else if (lineNumber < -1) {
            ScriptFinished();
            return;
        }
        else if (!script.ContainsKey(lineNumber)) {
            Debug.Log("Line number: "+ lineNumber + " not found in script");
            return;
        }
        string[] data = script[lineNumber];
        currentLineNumber = lineNumber;
        currentLine = data;

        if (data[0] == "Dialogue") {
            string LeftCharacter = null;
            string RightCharacter = null;
            string MiddleCharacter = null;

            

            if (data[5] != "_") {
                if (data[5] != currentLeftCharacter) {
                    LeftCharacter = data[5];
                }
            }
            if (data[6] != "_") {
                if (data[6] != currentRightCharacter) {
                    RightCharacter = data[6];
                }
            }
            if (data[7] != "_") {
                if (data[7] != currentMiddleCharacter) {
                    MiddleCharacter = data[7];
                }
            }
            
            choiceManager.disableChoiceManager();
            dialogueManager.UpdateCharacters(LeftCharacter, RightCharacter,MiddleCharacter);
            dialogueManager.UpdateConversation(data[3], data[4]);
        }
        else if (data[0] == "Question") { // at data[5] - choice background location data[6] - choice image location
            direction = true;
            dialogueManager.disableConversationManager();
            if (data[5]  != "_") {
                choiceManager.SetChoiceBackground(Resources.Load<Sprite>(data[5]));
            }

            choices = new string[0][];
            do{
                data = script[currentLineNumber];
                currentLine = data;
                string[] choice = { data[4], data[6],data[2] };
                Array.Resize(ref choices, choices.Length + 1);
                choices[choices.Length - 1] = choice;

                currentLineNumber++;
            }while(script[currentLineNumber][0] == "Question");
            choiceManager.UpdateChoices(choices);
            
        }
        else if (data[0] == "Background") {
            if(direction){ //forward
                EnableBackground(data[3] == "enable");
                ChangeBackground(data[4]);
                backgroundHistory.Add(new string[]{ data[3], data[4] });
                updateLine(int.Parse(data[2]));
            }
            else{
                if (backgroundHistory.Count > 0) {
                    string[] lastBackground = backgroundHistory[backgroundHistory.Count - 1];
                    EnableBackground(lastBackground[0] == "enable");
                    ChangeBackground(lastBackground[1]);
                    backgroundHistory.RemoveAt(backgroundHistory.Count - 1);
                    updateLine(int.Parse(data[1]));
                }
            }
        }
        else if (data[0] == "Return") {
            ReturnData(data[3]);
            if(direction)
                updateLine(int.Parse(data[2]));

            else 
                updateLine(int.Parse(data[1]));
            //return data[3] to the caller and load next line

        }
    }

    private void ScriptFinished()
    {
        ReturnData("~F");
        StopVisualNovel();
    }

    private void PreviousScene()
    {
        ReturnData("~P");
    }

    public void UpdateScriptFilePath(string filePath) {
        scriptFilePath = filePath;
        UpdateScript(scriptFilePath);
    }
    //background functions
    void ChangeBackground(string backgroundName) {
        if (backgroundName == "_") return;
        backgroundManager.ChangeBackground(backgroundName);
    }

    void EnableBackground(bool enable = false) {
        backgroundManager.EnableBackground(enable);
    }

    //ChoiceManager functions
    public void OnChoiceMade(string choice) {
        updateLine(int.Parse(choice));
    }

    //conversation functions
    public void OnNextBtnPressed() {
        direction = true;
        updateLine(int.Parse(currentLine[2]));
    }

    public void OnBackBtnPressed() {
        //Debug.Log("Back button pressed");
        direction = false;
        updateLine(int.Parse(currentLine[1]));
    }
    //extract data from csv
    public static Dictionary<int, string[]> ReadCSV(string filePath) {
        // Dictionary to store the CSV data
        Dictionary<int, string[]> csvData = new Dictionary<int, string[]>();
        

        try {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(filePath);

            // Process each line
            foreach (string line in lines) {
                // Split the line by commas
                string[] values = line.Split(',');

                // Ensure there are at least two values (one key and one value)
                if (values.Length > 1) {
                    // The first value is the key
                    string key = values[0];

                    // The rest of the values are the associated data
                    string[] data = new string[values.Length - 1];
                    Array.Copy(values, 1, data, 0, values.Length - 1);

                    // Add to the dictionary
                    csvData.Add(int.Parse(key), data);
                }
            }
        }
        catch (Exception ex) {
            Debug.Log("An error occurred while reading the CSV file: " + ex.Message);
        }

        return csvData;
    }

    Dictionary<string, string> ReadCharacterLocationCSV(string filePath) {
        // Dictionary to store the CSV data
        Dictionary<string, string> csvData = new Dictionary<string, string>();

        try {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(filePath);

            // Process each line
            foreach (string line in lines) {
                // Split the line by commas
                string[] values = line.Split(',');

                // Ensure there are at least two values (one key and one value)
                if (values.Length > 1) {
                    // The first value is the key
                    string key = values[0];

                    // The rest of the values are the associated data
                    string data = values[1];

                    // Add to the dictionary
                    csvData.Add(key, data);
                }
            }
        }
        catch (Exception ex) {
            Debug.Log("An error occurred while reading the CSV file: " + ex.Message);
        }

        return csvData;
    }
    public void UpdateScript(string filePath) {
        backgroundHistory.Clear();
        script = ReadCSV(filePath);
    }

    public void SetCharacterLocationFilePath(string filePath) {
        characterLocationFilePath = filePath;
    }
    public void SetReturnDataMethod(System.Action<string> returnData) {
        ReturnData = returnData;
    }
}