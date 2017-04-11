using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccelPower : MonoBehaviour {

    public Text text = null;
    public Slider slider = null;
    float sliderValue;

    public float currentRPM = 1000;
    public float maxRPM = 7000;
    public float minRPM = 700;
    float defaultRPMdecreace = 1.5f;
    public AudioClip accelsound;
    AudioSource source;

	// Use this for initialization
	void Start ()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        //slider.Invoke("sound", 0);
        source = gameObject.GetComponent<AudioSource>();
	}

    void ValueChangeCheck()
    {
        sliderValue = slider.value;
    }

    void rpmCtrl()
    {
        currentRPM += ((sliderValue * 20f) - defaultRPMdecreace);

        if (currentRPM < minRPM)
            currentRPM = minRPM;
        if (currentRPM > maxRPM)
            currentRPM = maxRPM;

        text.text = "RPM : " + currentRPM.ToString();
    }

    public void sound()
    {
        if (sliderValue != 0)
        {
            source.PlayOneShot(source.clip);
            source.volume = slider.value;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        rpmCtrl();
	}
}
