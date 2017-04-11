using UnityEngine;
using System.Collections;
using DotFuzzy;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CarEngineManager : MonoBehaviour
{
    public static CarEngineManager instance;

    private FuzzyEngine fuzzyEngineLoad = null;
    private FuzzyEngine fuzzyEngineIdle = null;
    private FuzzyEngine fuzzyEngineOffLow = null;
    private FuzzyEngine fuzzyEngineOffMid = null;
    private FuzzyEngine fuzzyEngineOffHigh = null;
    private FuzzyEngine fuzzyEngineOnLow = null;
    private FuzzyEngine fuzzyEngineOnMid = null;
    private FuzzyEngine fuzzyEngineOnHigh = null;

    // UI
    public Text currentGearText = null;
    public Text currentRPMText = null;
    public Text currentSpeedText = null;
    public Text throttleText = null;
    public Text loadText = null;
    //public Text idleVolumeText = null;
    //public Text offlowVolumeText = null;
    //public Text offmidVolumeText = null;
    //public Text offhighVolumeText = null;
    //public Text onlowVolumeText = null;
    //public Text onmidVolumeText = null;
    //public Text onhighVolumeText = null;
    //public Text idlePitchText = null;
    //public Text offlowPitchText = null;
    //public Text offmidPitchText = null;
    //public Text offhighPitchText = null;
    //public Text onlowPitchText = null;
    //public Text onmidPitchText = null;
    //public Text onhighPitchText = null;
    public Text nowCarText = null;
    public Slider accelSlider = null;
    public RectTransform rpmNeedle = null;


    private double[] gears = { 3, 1, 0.7, 0.5, 0.3, 0.2, 0.1 };
    private float[] gearRatios = { 1, 3.21f, 2.05f, 1.43f, 1.1f, 0.9f, 0.76f };
    private int currentRpm = 1000;
    private float currentSpeed = 0;
    private int currentThrottle = 0;
    private double loadMulti = 0;
    private int currentGear = 0;
    private double tireRadius = 1.752616;

    private float[] audioPitchs;
    private float idle;
    private float OnLow;
    private float OnMid;
    private float OnHigh;
    private float OffLow;
    private float OffMid;
    private float OffHigh;

    private string gearBtnName = String.Empty;
    private string carBtnName = String.Empty;
    private int soundSample;

    private A1Engine a1Engine = null;
    private F1Engine f1Engine = null;
    private F40Engine f40Engine = null;
    private MuscleEngine muscleEngine = null;

    private AudioClip[] engineClips = null;
    private AudioSource[] engineSources = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        loadFuzzy();
        engineSources = new AudioSource[7];
        MakeAudioSources();

        a1Engine = GameObject.FindObjectOfType<A1Engine>();
        f1Engine = GameObject.FindObjectOfType<F1Engine>();
        f40Engine = GameObject.FindObjectOfType<F40Engine>();
        muscleEngine = GameObject.FindObjectOfType<MuscleEngine>();

        ChangeEngineAudioClip(CarBtnName);

        accelSlider.onValueChanged.AddListener(delegate { valueChangeCheck(); });
        StartCoroutine(doWork());
    }

    //  make bowl to get sourceclip
    private void MakeAudioSources()
    {
        for(int i = 0; i < 7; ++i)
        {
            engineSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }


    void ChangeEngineAudioClip(string carBtnName)
    {
        switch (carBtnName)
        {
            case "A1":
                {
                    engineClips = a1Engine.GetClips();
                    soundSample = 1;
                }
                break;
            case "F1":
                {
                    engineClips = f1Engine.GetClips();
                    soundSample = 2;
                }
                break;
            case "F40":
                {
                    engineClips = f40Engine.GetClips();
                    soundSample = 3;
                }
                break;
            case "Muscle":
                {
                    engineClips = muscleEngine.GetClips();
                    soundSample = 4;
                }
                break;
            default:
                {
                    engineClips = muscleEngine.GetClips();
                    soundSample = 4;
                }
                break;
        }
        setUpEngineAudioSource(engineClips);
    }

    //  sets up and adds new audio source to the game object
    private void setUpEngineAudioSource(AudioClip[] clips)
    {
        for(int i = 0; i < 7; ++i)
        {
            engineSources[i].clip = clips[i];
            engineSources[i].loop = true;
            engineSources[i].time = Random.Range(0f, clips[i].length);
            engineSources[i].volume = 0;
            engineSources[i].Play();
        }
    }

    //  work function, calculate load and current-rpm
    IEnumerator doWork()
    {
        while (true)
        {
            CurrentSpeed();
            calculateLoad();
            if (currentRpm < 10000)
            {
                currentRpm = (int)(currentRpm + (loadMulti * gears[currentGear]));
                setFuzzyInput();
            }
            else
            {
                if (loadMulti < 0)
                {
                    currentRpm = (int)(currentRpm + loadMulti);
                    setFuzzyInput();
                }
                else
                {
                    currentRpm = 10000;
                    setFuzzyInput();
                }
            }
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }

    private void SetAudioPitch()
    {
        calculateVolumes();

        float rpm = (float)currentRpm;
        if (rpm > 0 && rpm < 10000)
        {
            switch (soundSample)
            {
                case 1:
                    {
                        audioPitchs = a1Engine.CalculatePitch(rpm);
                    }
                    break;
                case 2:
                    {
                        audioPitchs = f1Engine.CalculatePitch(rpm);
                    }
                    break;
                case 3:
                    {
                        audioPitchs = f40Engine.CalculatePitch(rpm);
                    }
                    break;
                case 4:
                    {
                        audioPitchs = muscleEngine.CalculatePitch(rpm);
                    }
                    break;
            }

            for(int i = 0; i < 7; ++i)
            {
                engineSources[i].pitch = audioPitchs[i];
            }
        }
    }

    //  initiate fuzzy engine and load xml data
    private void loadFuzzy()
    {
        fuzzyEngineLoad = new FuzzyEngine();
        fuzzyEngineIdle = new FuzzyEngine();
        fuzzyEngineOffLow = new FuzzyEngine();
        fuzzyEngineOffMid = new FuzzyEngine();
        fuzzyEngineOffHigh = new FuzzyEngine();
        fuzzyEngineOnLow = new FuzzyEngine();
        fuzzyEngineOnMid = new FuzzyEngine();
        fuzzyEngineOnHigh = new FuzzyEngine();

        fuzzyEngineLoad.Load("engineLoad2");
        fuzzyEngineIdle.Load("engineVolIdle");
        fuzzyEngineOffLow.Load("engineVolOffLow");
        fuzzyEngineOffMid.Load("engineVolOffMid");
        fuzzyEngineOffHigh.Load("engineVolOffHigh");
        fuzzyEngineOnLow.Load("engineVolOnLow");
        fuzzyEngineOnMid.Load("engineVolOnMid");
        fuzzyEngineOnHigh.Load("engineVolOnHigh");
    }

    //  set fuzzy load input variables
    private void setFuzzyInput()
    {
        fuzzyEngineLoad.LinguisticVariableCollection.Find("throttle").InputValue = (double)currentThrottle;
        fuzzyEngineLoad.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
    }

    //  defuzzify fuzzyload
    private void calculateLoad()
    {
        loadMulti = Convert.ToDouble(fuzzyEngineLoad.Defuzzify().ToString());
    }

    //  set fuzzy volume input variables, defuzzify fuzzyvolumes, set all volume values
    private void calculateVolumes()
    {
        fuzzyEngineIdle.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineIdle.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        idle = (float.Parse(fuzzyEngineIdle.Defuzzify().ToString()) / 100);
        if (idle <= 100 && idle >= 0) { engineSources[0].volume = idle; }

        fuzzyEngineOffLow.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOffLow.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OffLow = (float.Parse(fuzzyEngineOffLow.Defuzzify().ToString()) / 100);
        if (OffLow <= 100 && OffLow >= 0) { engineSources[1].volume = OffLow; }

        fuzzyEngineOffMid.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOffMid.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OffMid = (float.Parse(fuzzyEngineOffMid.Defuzzify().ToString()) / 100);
        if (OffMid <= 100 && OffMid >= 0) { engineSources[2].volume = OffMid; }

        fuzzyEngineOffHigh.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOffHigh.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OffHigh = (float.Parse(fuzzyEngineOffHigh.Defuzzify().ToString()) / 100);
        if (OffHigh <= 100 && OffHigh >= 0) { engineSources[3].volume = OffHigh; }

        fuzzyEngineOnLow.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOnLow.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OnLow = (float.Parse(fuzzyEngineOnLow.Defuzzify().ToString()) / 100);
        if (OnLow <= 100 && OnLow >= 0) { engineSources[4].volume = OnLow; }

        fuzzyEngineOnMid.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOnMid.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OnMid = (float.Parse(fuzzyEngineOnMid.Defuzzify().ToString()) / 100);
        if (OnMid <= 100 && OnMid >= 0) { engineSources[5].volume = OnMid; }

        fuzzyEngineOnHigh.LinguisticVariableCollection.Find("load").InputValue = loadMulti;
        fuzzyEngineOnHigh.LinguisticVariableCollection.Find("rpm").InputValue = (double)currentRpm;
        OnHigh = (float.Parse(fuzzyEngineOnHigh.Defuzzify().ToString()) / 100);
        if (OnHigh <= 100 && OnHigh >= 0) { engineSources[6].volume = OnHigh; }
    }


    //  XNA pitch to Unity pitch
    public float OcataveToRatio(float octave)
    {
        return Mathf.Pow(10, (octave * Mathf.Log10(2)));
    }

    //  throttle slider value handler
    private void valueChangeCheck()
    {
        currentThrottle = (int)(accelSlider.value);
    }

    public string CarBtnName
    {
        get { return carBtnName; }
        set { carBtnName = value; }
    }

    public string GearBtnName
    {
        get { return gearBtnName; }
        set { gearBtnName = value; }
    }

    //  when carswitch button down called
    public void OnSwitchSampleCar(GameObject obj)
    {
        CarBtnName = obj.name;
        ChangeEngineAudioClip(CarBtnName);
    }

    // when gear button called
    public void OnGearBtn(GameObject obj)
    {
        GearBtnName = obj.name;

        if (GearBtnName == "GearUp")
        {
            shiftUp();
        }
        else if (GearBtnName == "GearDown")
        {
            shiftDown();
        }
    }

    private void shiftUp()
    {
        if (currentGear > 0 && currentGear < 6)
        {
            currentGear++;
            if (currentRpm >= 4000)
            {
                currentRpm = currentRpm - 3000;
            }
            else
            {
                currentRpm = 1000;
            }
        }

        if (currentGear == 0)
        {
            currentGear++;
            currentRpm = 1000;
        }
    }

    private void shiftDown()
    {
        if (currentGear == 1)
        {
            currentGear--;
        }

        if (currentGear > 1 && currentGear <= 6)
        {
            currentGear--;
            if (currentRpm != 1000)
            {
                currentRpm = currentRpm + 1000;
            }
        }
    }


    void Update()
    {
        UpdateUI();
        SetAudioPitch();
        NeedleUpdate();
    }

    void NeedleUpdate()
    {
        float z = -Mathf.Lerp(-48, 179, (float)currentRpm/10000);

        Quaternion roteNeedle = Quaternion.Euler(0, 0, z);
        rpmNeedle.rotation = roteNeedle;
    }

    private void UpdateUI()
    {
        currentRPMText.text = "RPM : " + currentRpm;
        loadText.text = "Load : " + (float)loadMulti + "%";
        currentGearText.text = currentGear != 0 ? ("Gear : " + currentGear) : "Gear : N";
        throttleText.text = currentThrottle + "%";
        currentSpeedText.text = (int)currentSpeed + " km/h";
        nowCarText.text = CarBtnName == String.Empty ? "Muscle" : CarBtnName;

        //idleVolumeText.text = "Idle : " + (idle * 100);
        //offlowVolumeText.text = "offlow : " + (OffLow * 100);
        //offmidVolumeText.text = "offmid : " + (OffMid * 100);
        //offhighVolumeText.text = "offhigh : " + (OffHigh * 100);
        //onlowVolumeText.text = "onlow : " + (OnLow * 100);
        //onmidVolumeText.text = "onmid : " + (OnMid * 100);
        //onhighVolumeText.text = "onhigh : " + (OnHigh * 100);

        //idlePitchText.text = "Idle : " + engineSources[0].pitch;
        //onlowPitchText.text = "offlow : " + engineSources[1].pitch;
        //onmidPitchText.text = "offmid : " + engineSources[2].pitch;
        //onhighPitchText.text = "offhigh : " + engineSources[3].pitch;
        //offlowPitchText.text = "onlow : " + engineSources[4].pitch;
        //offmidPitchText.text = "onmid : " + engineSources[5].pitch;
        //offhighPitchText.text = "onhigh : " + engineSources[6].pitch;
    }

    private void CurrentSpeed()
    {
        if (currentGear == 0)
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= 0.3f;
            }
            else if (currentSpeed <= 0)
            {
                currentSpeed = 0;
            }
        }
        else
        {
            currentSpeed = (float)((currentRpm * 60.0f * tireRadius) / (1000 * gearRatios[currentGear] * 3.4));
        }
    }

}