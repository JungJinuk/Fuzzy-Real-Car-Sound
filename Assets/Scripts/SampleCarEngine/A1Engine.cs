using UnityEngine;
using System.Collections;

public class A1Engine : MonoBehaviour
{
    //  idle, offlow, offmid, offhigh, onlow, onmid, onhigh
    public AudioClip[] A1EngineClips;
    float[] audioPitchs = new float[7];

    public AudioClip[] GetClips()
    {
        return A1EngineClips;
    }

    public float[] CalculatePitch(float currentRPM)
    {
        if (currentRPM < 5000)
        {
            audioPitchs[0] = currentRPM / 5000;
        }
        audioPitchs[1] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[2] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[3] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[4] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[5] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);
        audioPitchs[6] = CarEngineManager.instance.OcataveToRatio((currentRPM / 10000) - 1f);

        return audioPitchs;
    }
}
