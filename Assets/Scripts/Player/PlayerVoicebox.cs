using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public enum ESoundPitch
{
    High,
    Low,
    None,
    All,
};


public class PlayerVoicebox : MonoBehaviour
{
    // Start is called before the first frame update

    #region Fields

    //control spellcasting with mouse not voice
    public bool mouseDebugMode;
    

    public float cooldownDuration;
    private float _cooldownTimer;
    
    public float[] spectrum = new float[1024];
    public float threshold = 0.0f;
    public float microphonePitch;
    public VoiceboxTuningParams tuningParams;
    public ESoundPitch lastSound;

    

    public Material redMat;
    public Material greenMat;
    public Material whiteMat;
    
    //for changing color/material of enemy weakness
    [SerializeField] private GameObject _pitchScreen;
    
    [SerializeField] private GameObject _pitchMat1;
    [SerializeField] private Light _pitchLight1;
    
    [SerializeField] private GameObject _pitchMat2;
    [SerializeField] private Light _pitchLight2;
    
    [SerializeField] private GameObject _pitchMat3;
    [SerializeField] private Light _pitchLight3;
    
    //these keep track of the sounds made during spellcasting - should be more accurate
    private List<ESoundPitch> firstSoundArray = new List<ESoundPitch>();
    private List<ESoundPitch> secondSoundArray = new List<ESoundPitch>();
    private List<ESoundPitch> ThirdSoundArray = new List<ESoundPitch>();
    private List<ESoundPitch> soundLibrary = new List<ESoundPitch>();
    
    
    private ESoundPitch[] soundArray = new ESoundPitch[3];

    //private PlayerSpellcast _spellcast;
    
    private bool isRecording = false;
    
    private int _minFreq;
    private int _maxFreq;


    
    //players must cast spell over the course of 3 seconds
    private float _spellTimer;
    
    private string _microphoneName;
    
    private AudioSource _audioSource;
    private PlayerAudioManager _audioManager;
    
    public Animator _animator;
    
    
    #endregion
    

    
    
    
    void Start()
    {

        _spellTimer = 3.0f;
        _cooldownTimer = 0.0f;
        //_spellcast = GetComponent<PlayerSpellcast>();
        _audioSource = GetComponent<AudioSource>();
        _audioManager = GetComponentInChildren<PlayerAudioManager>();
       // InitMicrophone();
        
        _audioSource.Play();
        
    }
    
/*
    void InitMicrophone()
    {
        
        if (Microphone.devices.Length > 0)
        {
            _microphoneName = Microphone.devices[2];
            Debug.Log("Chosen Mic: " + _microphoneName);

            _audioSource.clip = Microphone.Start(_microphoneName, true, 5, 44100);
        }
        
    }
*/
    void Update()
    {
    
        //if fire button pressed
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) 
            && !isRecording 
            && _cooldownTimer < 0.0f)
        {
            isRecording = true;
            _animator.SetBool("isRecording",true);
            _spellTimer = 3.0f;
            _audioManager.PlayTargetHitClip();
            //make UI visible
            //spellcastUI.SetActive(true);
            
        }

        if (true)
        {

            if (isRecording && Input.GetMouseButtonDown(0))
                lastSound = ESoundPitch.Low;
            else if(isRecording && Input.GetMouseButtonDown(1))
                lastSound = ESoundPitch.High;
                
            
            
            //check if sound was made and add to library

            if (isRecording)
            {
                //affect onscreen UI
                if (lastSound.Equals(ESoundPitch.High))
                    _pitchScreen.GetComponent<Renderer>().material = greenMat;
                else
                    _pitchScreen.GetComponent<Renderer>().material = redMat;



                ESoundPitch tempPitch;
                
                //add sound to appropriate array
                if (_spellTimer > 2.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, firstSoundArray);
                    SetAnimation(tempPitch);

                    if (_spellTimer < 2.5f)
                    {
                        ChangePanelColor(_pitchMat1,_pitchLight1,tempPitch);
                        soundArray[0] = tempPitch;
                    }

                }

                else if (_spellTimer > 1.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, secondSoundArray);
                    SetAnimation(tempPitch);

                    if (_spellTimer < 1.5f)
                    {
                        soundArray[1] = tempPitch;
                        ChangePanelColor(_pitchMat2,_pitchLight2,tempPitch);


                    }
                }
                else if (_spellTimer > 0.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, ThirdSoundArray);
                    SetAnimation(tempPitch);

                    if (_spellTimer < 0.5f)
                    {
                        soundArray[2] = tempPitch;
                        ChangePanelColor(_pitchMat3,_pitchLight3,tempPitch);


                    }
                }

            }

                
           
        }
        else
        {
            //InitMicrophone();
            //Debug.Log("Play it again sam");
            //_audioSource.clip = Microphone.Start(_microphoneName, true, 10,44100);
            _audioSource.Play();
        }
        
        _cooldownTimer -= Time.deltaTime;
        _animator.SetFloat("cooldownTimer",_cooldownTimer);
        
        _spellTimer -= Time.deltaTime;
        if (_spellTimer < 0.0f && isRecording)
        {
            _cooldownTimer = cooldownDuration;
           // _spellcast.CastSpell(soundArray);
           _audioManager.PlayTargetSuccessClip();
            GameEventSystem.current.PlayerCastSpell(soundArray);
            isRecording = false;
            _animator.SetBool("isRecording",false);
            //spellcastUI.SetActive(false);
            CleanupUI();
           // DeterminePitches();
        }

    }

    
    
    
    
    
    
    
    
    #region AudioFunctions

    
    //adds pitch to given array and returns the most common pitch in that array
    ESoundPitch AddPitchToArray(ESoundPitch pitch,List<ESoundPitch> list)
    {
        list.Add(pitch);

        int highCount = 0;
        int lowCount = 0;
        
        foreach (var pit in list)
        {           
            if (pit.Equals(ESoundPitch.High))
                highCount++;
            else
                lowCount++;
        }

        if (highCount > lowCount)
            return ESoundPitch.High;
        else
            return ESoundPitch.Low;

    }
    
    void DeterminePitches()
    {
        int highCount = 0;
        int lowCount = 0;

        ESoundPitch firstPitch;
        ESoundPitch secondPitch;
        ESoundPitch thirdPitch;

        foreach (var pitch in firstSoundArray)
        {
            if (pitch.Equals(ESoundPitch.High))
                highCount++;
            else
                lowCount++;
            
        }

        if (highCount > lowCount)
            firstPitch = ESoundPitch.High;
        else
            firstPitch = ESoundPitch.Low;


        highCount = 0;
        lowCount = 0;
        foreach (var pitch in secondSoundArray)
        {
            if (pitch.Equals(ESoundPitch.High))
                highCount++;
            else
                lowCount++;
            
        }

        if (highCount > lowCount)
            secondPitch = ESoundPitch.High;
        else
            secondPitch = ESoundPitch.Low;
        
        highCount = 0;
        lowCount = 0;
        foreach (var pitch in ThirdSoundArray)
        {
            if (pitch.Equals(ESoundPitch.High))
                highCount++;
            else
                lowCount++;
            
        }

        if (highCount > lowCount)
            thirdPitch = ESoundPitch.High;
        else
            thirdPitch = ESoundPitch.Low;
        
        
        Debug.Log("First Pitch: " + firstPitch);
        Debug.Log("Second Pitch: " + secondPitch);
        Debug.Log("Third Pitch: " + thirdPitch);
        
        
        
        

    }


    void ChangePanelColor(GameObject obj,Light light, ESoundPitch pitch)
    {
        if (pitch.Equals(ESoundPitch.High))
        {
            obj.GetComponent<Renderer>().material = greenMat;
            light.color = Color.green;
        }
        else
        {
            obj.GetComponent<Renderer>().material = redMat;
            light.color = Color.red;
        }

    }

    void SetAnimation(ESoundPitch pit)
    {
        if (pit.Equals(ESoundPitch.High))
            _animator.SetBool("isHigh",true);
        else 
            _animator.SetBool("isHigh",false);
    }
    
    
    ESoundPitch AnalyzeAudio()
    {
        _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxV = 0.0f;
        int maxN = 0;

        for (int i = 0; i < spectrum.Length; i++)
        {
            if(!(spectrum[i]>maxV) || !(spectrum[i] > threshold))
                continue;

            maxV = spectrum[i];
            maxN = i;
        }

        float freqN = maxN;

        if (maxN > 0 && maxN < spectrum.Length - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }


        microphonePitch = freqN * (44100 / 2) / spectrum.Length;


        if (microphonePitch > tuningParams.lowFreqFloor && microphonePitch < tuningParams.lowFreqCeiling)
        {
            Debug.Log("Low Noise: " + microphonePitch);
            return ESoundPitch.Low;
        }
            
        else if (microphonePitch > tuningParams.highFreqFloor && microphonePitch > tuningParams.highFreqCeiling)
        {
            Debug.Log("High Noise: " + microphonePitch);
            return ESoundPitch.High;
            
        }
        


        return lastSound;
        /*
        Debug.Log("Spectrum 0: " + spectrum[0]*100.0f);
        Debug.Log("Spectrum 1: " + spectrum[1]*100.0f);
        Debug.Log("Spectrum 2: " + spectrum[2]*100.0f);
        */
    }



    #endregion

    #region Utilities

    void CleanupUI()
    {
        firstSoundArray.Clear();
        secondSoundArray.Clear();
        ThirdSoundArray.Clear();

        _pitchScreen.GetComponent<Renderer>().material = whiteMat;
        _pitchMat1.GetComponent<Renderer>().material = whiteMat;
        _pitchMat2.GetComponent<Renderer>().material = whiteMat;
        _pitchMat3.GetComponent<Renderer>().material = whiteMat;
        
        _pitchLight1.color = Color.yellow;
        _pitchLight2.color = Color.yellow;
        _pitchLight3.color = Color.yellow;

    }
    

    #endregion


}
