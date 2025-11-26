using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningIndicator : MonoBehaviour
{
    public Color targetColor = new Color(1, 0, 0, 1); 
    public Color reverseTargetColor = new Color(1, 0, 0, 0);
    public Material materialToChange;
    private bool _flip = true;

    public LightningManager caller;
    private float _lifespan = 1f;
    
    void Start()
    {
        _lifespan = Time.unscaledTime + _lifespan;
        materialToChange = gameObject.GetComponent<SpriteRenderer>().material;
        StartCoroutine(LerpFunction(reverseTargetColor, 0.2f));
    }

    private void Update()
    {
        float time = Time.unscaledTime;
        if (time > _lifespan)
        { 
            caller.LightningStrike(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = materialToChange.color;

        while (time < duration)
        {
            materialToChange.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        materialToChange.color = endValue;
        _flip = !_flip;
        StartCoroutine(LerpFunction(_flip ? reverseTargetColor : targetColor, 0.2f));
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
