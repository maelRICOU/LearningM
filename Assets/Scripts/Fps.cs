using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    const float updateInterval = .5f;
    float accum;
    float frames;
    float timeleft;

    Text text;

    float fpsCount;

    private void Start()
    {
        timeleft = updateInterval;
        text = GetComponent<Text>();
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if(timeleft <= 0)
        {
            fpsCount = accum / frames;

            text.text = fpsCount.ToString("f1");

            if(fpsCount < 30)
            {
                text.color = Color.red;
            }
            else
            if(fpsCount < 60)
            {
                text.color = Color.yellow;
            }
            else
            {
                text.color = Color.cyan;
            }

            timeleft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }
}
