using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

    public AudioClip[] backgroundMusic;
    private AudioSource source;
    int randomNum;
    float interval = 0f;

    void Start ()
    {
        randomNum = Random.Range(0, 3);
        source = gameObject.AddComponent<AudioSource>();
        source.clip = backgroundMusic[randomNum];
        source.loop = false;
        source.Play();
    }
	
	void Update ()
    {
        interval += Time.deltaTime;
        if (interval > 15.0f)
        {
            source.volume -= 0.002f;
            if (source.volume <= 0)
            {
                source.Stop();
            }
        }
	}
}
