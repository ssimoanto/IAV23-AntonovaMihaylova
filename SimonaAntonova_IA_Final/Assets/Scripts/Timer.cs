using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image uiFill;
    public TextMeshProUGUI uiText;

    public float duration = 180.0f;
    float targetTime = 180.0f;
    bool done = false;

    void Update()
    {
        if (!done)
        {
            uiText.text = $"{(int)targetTime}";
            uiFill.fillAmount = Mathf.InverseLerp(0, duration, targetTime);
            targetTime -= Time.deltaTime;
            if (targetTime <= 0) done = true;
        }
    }
}
