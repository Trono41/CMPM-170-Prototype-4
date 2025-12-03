using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private static TimerManager _instance;
    public static TimerManager Instance { get { return _instance; } }
    
    LightningManager lightning;
    Objective goal;
    PlayerHealth health;
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private float updatedLightingStrikeCount;
    [SerializeField] private float updatedLightningStrikeCycleTime;
    [SerializeField] private GameObject goalObject;
    [SerializeField] private GameObject player;
    private float currTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightning = LightningManager.Instance;
        goal = goalObject.GetComponent<Objective>();
        health = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal.win == true || health.lose == true)
        {
            timer = currTime;
        } 
        else
        {
            timer -= Time.deltaTime;
            currTime = timer;
        }

        if ((int)timer % 60 < 10)
        {
            timerDisplay.text = (int)timer / 60 + ":0" + (int)timer % 60;
        }
        else
        {
            timerDisplay.text = (int)timer / 60 + ":" + (int)timer % 60;
        }

        if (timer <= 0)
        {
            timer = 0;
            lightning.lightningStrikesPerCycle = updatedLightingStrikeCount;
            lightning.lightningStrikeCycleTime = updatedLightningStrikeCycleTime;
        }
    }
}
