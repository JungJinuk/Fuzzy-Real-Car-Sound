using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class CarSound : MonoBehaviour
{




    public AudioClip lowAccelClip;                                              // Audio clip for low acceleration
    public AudioClip lowDecelClip;                                              // Audio clip for low deceleration
    public AudioClip highAccelClip;                                             // Audio clip for high acceleration
    public AudioClip highDecelClip;                                             // Audio clip for high deceleration
    public AudioClip startEngine;                                               // Audio clip for start engine
    public AudioClip defaultClip;

    public float pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    public float lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    public float lowPitchMax = 6f;                                              // The highest possible pitch for the low sounds
    public float highPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    public float maxRolloffDistance = 30;                                      // The maximum distance where rollof starts to take place
    public float dopplerLevel = 1;                                              // The mount of doppler effect used in the audio
    public bool useDoppler = true;                                              // Toggle for using doppler

    private AudioSource m_LowAccel; // Source for the low acceleration sounds
    private AudioSource m_LowDecel; // Source for the low deceleration sounds
    private AudioSource m_HighAccel; // Source for the high acceleration sounds
    private AudioSource m_HighDecel; // Source for the high deceleration sounds
    private AudioSource m_Start;   // Source for the start sounds
    private AudioSource m_default;  // Source for the default sounds

    private bool m_StartedSound; // flag for knowing if we have started sounds

    public float currentRPM = 1000;
    public int maxRPM = 7000;
    public int minRPM = 700;

    public int currentGear;
    public int beforeGear;

    public Text gearText = null;

    Transform needle;
    public string btnName = null;



    // sets up and adds new audio source to the gane object
    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = 0;
        return source;
    }

    private AudioSource StartDefaultSound(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.Play();
        source.volume = 0;
        return source;
    }


    private AudioSource StartEngine(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.Play();
        return source;
    }

    private void StartSound()
    {
        // setup the simple audio source
        m_HighAccel = SetUpEngineAudioSource(highAccelClip);
        
        // flag that we have started the sounds playing
        m_StartedSound = true;
    }


    // Use this for initialization
    void Start()
    {
        m_Start = StartEngine(startEngine);
        m_default = StartDefaultSound(defaultClip);
        StartSound();
        needle = gameObject.GetComponent<RectTransform>();
        currentGear = 1;
        beforeGear = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        m_Start.volume -= 0.004f;

        if (m_StartedSound)
        {
            // The pitch is interpolated between the min and max values, according to the car's revs.

            float pitch = ULerp(lowPitchMin, lowPitchMax, currentRPM / maxRPM);

            // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
            pitch = Mathf.Min(lowPitchMax, pitch);

            // for 1 channel engine sound, it's oh so simple:
            m_HighAccel.pitch = pitch * pitchMultiplier * highPitchMultiplier;
            m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            m_HighAccel.volume = 0.5f;
        }
        needleUpdate();
        gearSystem();

        if (currentGear == 1 && currentRPM == minRPM)
        {
            m_default.volume = 1f;
            m_default.pitch = 1.09f;
        }
        else
        {
            m_default.volume = 0.0f;
        }

        gearText.text = currentGear.ToString();
    }


    void needleUpdate()
    {
        float z = -Mathf.Lerp(0, 170, currentRPM / maxRPM);

        Quaternion roteNeedle = Quaternion.Euler(0, 0, z);
        needle.rotation = roteNeedle;
    }


    // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    void gearSystem()
    {
        if (currentGear == 1 && beforeGear == 1)
        {
            float accelRPMIncrease = 30.0f;
            float breakRPMdecrease = 80.0f;
            float defaultRPMdecrease = 5.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 3000)
            {
                currentGear = 2;
                beforeGear = 1;
                currentRPM = 1200;
            }
        }
        else if (currentGear == 1 && beforeGear == 2)
        {
            float accelRPMIncrease = 30.0f;
            float breakRPMdecrease = 80.0f;
            float defaultRPMdecrease = 5.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 3000)
            {
                currentGear = 2;
                beforeGear = 1;
                currentRPM = 1200;
            }
        }
        else if (currentGear == 2 && beforeGear == 1)
        {
            float accelRPMIncrease = 25.0f;
            float breakRPMdecrease = 90.0f;
            float defaultRPMdecrease = 6.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 4000)
            {
                currentGear = 3;
                beforeGear = 2;
                currentRPM = 1800;
            }
            else if (currentRPM < 1100)
            {
                currentGear = 1;
                beforeGear = 2;
                currentRPM = 2500;
            }
        }
        else if (currentGear == 2 && beforeGear == 3)
        {
            float accelRPMIncrease = 25.0f;
            float breakRPMdecrease = 90.0f;
            float defaultRPMdecrease = 6.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 5000)
            {
                currentGear = 3;
                beforeGear = 2;
                currentRPM = 1800;
            }
            else if (currentRPM < 2000)
            {
                currentGear = 1;
                beforeGear = 2;
                currentRPM = 2500;
            }
        }
        else if (currentGear == 3 && beforeGear == 2)
        {
            float accelRPMIncrease = 20.0f;
            float breakRPMdecrease = 150.0f;
            float defaultRPMdecrease = 7.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 5000)
            {
                currentGear = 4;
                beforeGear = 3;
                currentRPM = 3500;
            }
            else if (currentRPM < 1700)
            {
                currentGear = 2;
                beforeGear = 3;
                currentRPM = 3500;
            }
        }
        else if (currentGear == 3 && beforeGear == 4)
        {
            float accelRPMIncrease = 20.0f;
            float breakRPMdecrease = 150.0f;
            float defaultRPMdecrease = 7.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM > 6000)
            {
                currentGear = 4;
                beforeGear = 3;
                currentRPM = 3500;
            }
            else if (currentRPM < 2000)
            {
                currentGear = 2;
                beforeGear = 3;
                currentRPM = 3000;
            }
        }
        else if (currentGear == 4 && beforeGear ==3)
        {
            float accelRPMIncrease = 10.0f;
            float breakRPMdecrease = 200.0f;
            float defaultRPMdecrease = 5.0f;
            rpmUpdateGear(accelRPMIncrease, breakRPMdecrease, defaultRPMdecrease);
            if (currentRPM < 2500)
            {
                currentGear = 3;
                beforeGear = 4;
                currentRPM = 3200;
            }
        }



        if (currentRPM < minRPM)
            currentRPM = minRPM;
        if (currentRPM > maxRPM)
            currentRPM = maxRPM;
    }


    void rpmUpdateGear(float accelRPMIncrease, float breakRPMdecrease, float defaultRPMdecrease)
    {
        if (btnName == "AccelBtn")
        {
            currentRPM += accelRPMIncrease;
        }
        else if (btnName == "BreakBtn")
        {
            currentRPM -= breakRPMdecrease;
        }
        else
        {
            currentRPM -= defaultRPMdecrease;
            
        }
    }
}