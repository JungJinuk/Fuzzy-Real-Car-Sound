using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuscleEngine : MonoBehaviour
{
    //  idle, offlow, offmid, offhigh, onlow, onmid, onhigh
    public AudioClip[] MuscleEngineClips;
    float[] audioPitchs = new float[7];


    public float[] CalculatePitch(float currentRPM)
    { 
        if (currentRPM < 5000)
        {
            audioPitchs[0] = CarEngineManager.instance.OcataveToRatio(currentRPM / 5000);

        }
        audioPitchs[1] = CarEngineManager.instance.OcataveToRatio(currentRPM / 10000);
        audioPitchs[2] = CarEngineManager.instance.OcataveToRatio(currentRPM / 10000);
        audioPitchs[3] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[4] = CarEngineManager.instance.OcataveToRatio(currentRPM / 10000);
        audioPitchs[5] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.4f);
        audioPitchs[6] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);

        return audioPitchs;
    }

    public AudioClip[] GetClips()
    {
        return MuscleEngineClips;
    }
}
