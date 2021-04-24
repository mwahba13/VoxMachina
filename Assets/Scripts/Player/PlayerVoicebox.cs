using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityStandardAssets.CrossPlatformInput;

public enum ESoundPitch
{
    High,
    Low,
    None
};


public class PlayerVoicebox : MonoBehaviour
{
    // Start is called before the first frame update

    #region Fields

    //control spellcasting with mouse not voice
    public bool mouseDebugMode;
    
    public float[] spectrum = new float[1024];
    public float threshold = 0.0f;
    public float microphonePitch;
    public VoiceboxTuningParams tuningParams;
    public ESoundPitch lastSound;

    //UI ELEMENTS
    [SerializeField] private GameObject spellcastUI;
    [SerializeField] private Image spellcastScrollerImage;
    [SerializeField] private Image firstSoundPanel;
    [SerializeField] private Image secondSoundPanel;
    [SerializeField] private Image thirdSoundPanel;
    
    //these keep track of the sounds made during spellcasting - should be more accurate
    private List<ESoundPitch> firstSoundArray = new List<ESoundPitch>();
    private List<ESoundPitch> secondSoundArray = new List<ESoundPitch>();
    private List<ESoundPitch> ThirdSoundArray = new List<ESoundPitch>();

    private bool isRecording = false;
    
    private int _minFreq;
    private int _maxFreq;


    
    //players must cast spell over the course of 3 seconds
    private float _spellTimer;
    
    private string _microphoneName;
    
    private AudioSource _audioSource;

    private KeywordRecognizer _recognizer;    

    #endregion
    

    
    
    
    void Start()
    {

        _spellTimer = 3.0f;
        
        
        _audioSource = GetComponent<AudioSource>();
        
        InitMicrophone();
        
        _audioSource.Play();
        
    }
    

    void InitMicrophone()
    {
        
        if (Microphone.devices.Length > 0)
        {
            _microphoneName = Microphone.devices[2];
            Debug.Log("Chosen Mic: " + _microphoneName);

            _audioSource.clip = Microphone.Start(_microphoneName, true, 5, 44100);
        }
        
    }

    void Update()
    {
        //if fire button pressed
        if (CrossPlatformInputManager.GetAxis("Fire1") == 1 && !isRecording)
        {
            Debug.Log("Start recording");
            isRecording = true;
            _spellTimer = 3.0f;
            
            //make UI visible
            spellcastUI.SetActive(true);
            
        }

        if (_audioSource.isPlaying)
        {
            if(!mouseDebugMode)
                lastSound = AnalyzeAudio();
            else
            {
                if (isRecording && CrossPlatformInputManager.GetAxis("Fire1") == 1)
                    lastSound = ESoundPitch.High;
                else
                    lastSound = ESoundPitch.Low;
            }

            if (isRecording)
            {
                //affect onscreen UI
                Vector3 imageLocalPos = spellcastScrollerImage.rectTransform.localPosition;
                if (lastSound.Equals(ESoundPitch.High))
                    spellcastScrollerImage.rectTransform.SetPositionAndRotation(new Vector3(imageLocalPos.x,400,imageLocalPos.z),Quaternion.identity);
                else if(lastSound.Equals(ESoundPitch.Low))
                    spellcastScrollerImage.rectTransform.SetPositionAndRotation(new Vector3(imageLocalPos.x,150,imageLocalPos.z),Quaternion.identity);


                ESoundPitch tempPitch;
                
                //add sound to appropriate array
                if (_spellTimer > 2.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, firstSoundArray);

                    if (_spellTimer < 2.5f)
                        ChangePanelColor(firstSoundPanel,tempPitch);
                        
                    
                }

                else if (_spellTimer > 1.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, secondSoundArray);
                    
                    if(_spellTimer < 1.5f)
                        ChangePanelColor(secondSoundPanel,tempPitch);
                }
                else if (_spellTimer > 0.0f)
                {
                    tempPitch = AddPitchToArray(lastSound, ThirdSoundArray);
                    
                    if(_spellTimer < 0.5f)
                        ChangePanelColor(thirdSoundPanel,tempPitch);
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

        _spellTimer -= Time.deltaTime;
        if (_spellTimer < 0.0f)
        {
            
            
            isRecording = false;
            spellcastUI.SetActive(false);
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


    void ChangePanelColor(Image panelImage, ESoundPitch pitch)
    {
        if (pitch.Equals(ESoundPitch.High))
            panelImage.color = tuningParams.highPitchColor;
        else
            panelImage.color = tuningParams.lowPitchColor;
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
        
        firstSoundPanel.color = Color.white;
        secondSoundPanel.color = Color.white;
        thirdSoundPanel.color = Color.white;
        
    }

    #endregion


}
