using UnityEngine;
using System.Collections;

public class F40Engine : MonoBehaviour
{
    //  idle, offlow, offmid, offhigh, onlow, onmid, onhigh
    public AudioClip[] F40EngineClips;
    float[] audioPitchs = new float[7];

    public AudioClip[] GetClips()
    {
        return F40EngineClips;
    }

    public float[] CalculatePitch(float currentRPM)
    {
        if (currentRPM < 6000)
        {
            audioPitchs[0] = CarEngineManager.instance.OcataveToRatio(currentRPM / 6000);
            audioPitchs[4] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) + 0.2f);
        }
        audioPitchs[1] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.8f);
        audioPitchs[2] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.8f);
        audioPitchs[3] = CarEngineManager.instance.OcataveToRatio(currentRPM / 10000);
        audioPitchs[5] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.4f);
        audioPitchs[6] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.8f);

        return audioPitchs;
    }
}
