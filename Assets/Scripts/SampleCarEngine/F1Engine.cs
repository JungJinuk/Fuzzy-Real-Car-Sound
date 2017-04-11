using UnityEngine;
using System.Collections;

public class F1Engine : MonoBehaviour
{
    //  idle, offlow, offmid, offhigh, onlow, onmid, onhigh
    public AudioClip[] F1EngineClips;
    float[] audioPitchs = new float[7];

    public AudioClip[] GetClips()
    {
        return F1EngineClips;
    }

    public float[] CalculatePitch(float currentRPM)
    {
        if (currentRPM < 3000)
        {
            audioPitchs[0] = CarEngineManager.instance.OcataveToRatio(currentRPM / 3000);
        }
        audioPitchs[1] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[2] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[3] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[4] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.4f);
        audioPitchs[5] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 0.4f);
        audioPitchs[6] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);

        return audioPitchs;
    }
}
