using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource GameAudioSource;

    public static SoundController Instance;

    private void Awake() 
    {
        Instance = this;
    }

    void Start()
    {
        GameAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
