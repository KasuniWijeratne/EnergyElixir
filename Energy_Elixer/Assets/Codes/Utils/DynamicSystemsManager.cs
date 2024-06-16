using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

public interface IDynamicSystem 
{
    public IEnumerator ChangeEnvironmentStatusAsync();

    public float getScoreMultiplier();

    public float getPredictedConsumption();
}
 
public class DynamicSystemsManager : MonoBehaviour, IDynamicSystem
{
    PowerConsumptionAllResponse powerConsumptionAllTime;
    float previousPowerConsumption = 0.0f;
    float actualConsumptionRate = 0.0f;
    float predictedConsumptionRate = 0.0f;
    float scoreMultiplier = 1.0f;
    float environmentScore = 0.0f;


    void Awake(){
        APIHandler.Instance.FetchAllPowerConsumption(
            (response) => {
                powerConsumptionAllTime = JsonConvert.DeserializeObject<PowerConsumptionAllResponse>(response);
            },
            (error) => {
                Debug.LogError("Error fetching power consumption data: " + error);
            }      
        );
    }

    public IEnumerator ChangeEnvironmentStatusAsync()
    {
        throw new System.NotImplementedException();
    }

    public float getScoreMultiplier()
    {
        throw new System.NotImplementedException();
    }

    public float getPredictedConsumption()
    {
        throw new System.NotImplementedException();
    }

    public float getCurrentConsumption()
    {
        throw new System.NotImplementedException();
    }

}