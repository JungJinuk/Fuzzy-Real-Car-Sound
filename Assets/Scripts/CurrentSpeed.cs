//using UnityEngine;
//using System.Collections;
//using System;

//public class CurrentSpeed : MonoBehaviour
//{

//    float blockTimer = 0;
//    int currentGear;
//    float shiftSpeed = 0.05f;
//    float currentGearRatio;

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//    }

//    public float currentSpeed(float currentRPM, int gearRatio)
//    {
//        return (float)((currentRPM * 60.0 * 1.752616) / (1000 * gearRatio * 3.4));
//    }

//    public void startTimer()
//    {
//        blockTimer += Time.deltaTime;
//        if (blockTimer > 0.025f)
//            blockTimer = 0;
//    }

//    public void setCurrentGear(int i)
//    {
//        if (i == currentGear)
//            return;
//        startTimer();
//        if (blockTimer == 0)
//            StartCoroutine(startShifting(i));
//    }

//    public IEnumerable startShifting(int i)
//    {
//        currentGear = 0;
//        yield return new WaitForSeconds(shiftSpeed);
//        currentGear = i;
//    }

//    public float currentTorque(float currentRPM)
//    {
//        float PS = currentRPM * 454 / 7000;
//        float engineTorque = ((PS * 75 * 60) / (2 * Mathf.PI * currentRPM));
//        float wheelTorque = (float)(engineTorque * 3.4 * currentGearRatio);
//        return wheelTorque;
//    }
//}
