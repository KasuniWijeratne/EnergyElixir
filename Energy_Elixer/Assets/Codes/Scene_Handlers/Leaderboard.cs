using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Leaderboard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    // private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake() {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        FetchAndDisplayLeaderboard();

        // highscoreEntryList = new List<HighscoreEntry>() {
        //     new HighscoreEntry{ score = 521854, name = "Tinal" },
        //     new HighscoreEntry{ score = 511867, name = "Himanshi" },
        //     new HighscoreEntry{ score = 531864, name = "Laksika" },
        //     new HighscoreEntry{ score = 466854, name = "Podini" },
        // };

        // AddHighscoreEntry(100000,"Ninuka");

        // string jsonString = PlayerPrefs.GetString("highscoreTable");
        // Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // for (int i=0; i<highscores.highscoreEntryList.Count; i++){
        //     for ( int j=i+1; j<highscores.highscoreEntryList.Count; j++){
        //         if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
        //             //swap
        //             HighscoreEntry tmp =  highscores.highscoreEntryList[i];
        //             highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
        //             highscores.highscoreEntryList[j] = tmp;
        //         }
        //     }
        // }
        // highscoreEntryTransformList = new List<Transform>();
        // foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
        //     CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        // }

        // Highscores highscores = new Highscores {highscoreEntryList = highscoreEntryList};
        // string json = JsonUtility.ToJson(highscores);
        // PlayerPrefs.SetString("highscoreTable", json);
        // PlayerPrefs.Save();
        // Debug.Log(PlayerPrefs.GetString("highscoreTable"));


        
    }

    private void FetchAndDisplayLeaderboard()
    {
        DatabaseHandler.Instance.GetAllPlayers(
            (successMsg) =>
            {
                List<DBPlayer> players = JsonConvert.DeserializeObject<List<DBPlayer>>(successMsg);
                                            
                

                // Convert DBPlayerList to Highscores
                Highscores highscores = new Highscores
                {
                    highscoreEntryList = new List<HighscoreEntry>()
                };

                foreach (var player in players)
                {
                    highscores.highscoreEntryList.Add(new HighscoreEntry { score = player.coins, name = player.nic });
                }

                // Sort players by marks in descending order
                for (int i=0; i<highscores.highscoreEntryList.Count; i++){
                    for ( int j=i+1; j<highscores.highscoreEntryList.Count; j++){
                        if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    //swap
                            HighscoreEntry tmp =  highscores.highscoreEntryList[i];
                            highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                            highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

                // Clear any existing entries in the container
                foreach (Transform child in entryContainer)
                {
                    if (child != entryTemplate)
                    {
                        Destroy(child.gameObject);
                    }
                }

                highscoreEntryTransformList = new List<Transform>();
                int countVar = 0;
                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
                {
                    countVar++;
                    if (countVar > 15){
                        break; // Exit the loop if count exceeds 17
                    }
                    else{CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);}
                    
                }
            },
            (errorMsg) =>
            {
                Debug.LogError("Error fetching player data: " + errorMsg);
            }
        );
    }
    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container , List<Transform> transformList) {

        float templateHeight = 45f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count+1;
        string rankString;
        switch (rank){
            default:
                rankString = rank + "th"; break;
                
            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;
        }

        entryTransform.Find("rank").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("score").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("name").GetComponent<Text>().text = name;

        entryTransform.Find("bg").gameObject.SetActive(rank % 2 ==1);

        if (rank ==1){
            entryTransform.Find("name").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("score").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("rank").GetComponent<Text>().color = Color.yellow;
        }
        
        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name) {
        //create highscore entry
        HighscoreEntry highscoreEntry = new HighscoreEntry {score = score, name= name };


        // Load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // add new entry 
        highscores.highscoreEntryList.Add(highscoreEntry);

        // save updated highscore table
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();




    }

    public class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

/*  *
* this class represent a single high score entry
* */
    [System.Serializable]
    public class HighscoreEntry{

        public int score;
        public string name;
    }

}