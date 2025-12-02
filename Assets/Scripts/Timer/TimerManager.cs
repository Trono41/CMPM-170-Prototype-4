using Systems.Collections;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private static TimerManager _instance;
    public static TimerManager Instance { get { return _instance; } }
    
    LightningManager lightning;
    [SerializeField] private float timer;
    [SerializeField] private GameObject timerDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightning = LightningManager.Instance;
        StartCoroutine(Timer(timer));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer(float time)
    {
        timerDisplay.text = time.ToString();

        yield return timer;
    }
}
