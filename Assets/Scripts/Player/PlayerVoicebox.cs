using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerVoicebox : MonoBehaviour
{
    // Start is called before the first frame update

    public float[] spectrum = new float[1024];
    public float threshold = 0.0f;
    public float microphonePitch;
    public VoiceboxTuningParams tuningParams;

    [SerializeField] private string[] _keywords;
    
    private int _minFreq;
    private int _maxFreq;

    private string _microphoneName;
    
    private AudioSource _audioSource;

    private KeywordRecognizer _recognizer;
    
    void Start()
    {

        
        
        _audioSource = GetComponent<AudioSource>();
        
        InitMicrophone();
        
        _audioSource.Play();
        
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {

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
        

        if (_audioSource.isPlaying)
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
            
            
            if(microphonePitch > tuningParams.lowFreqFloor && microphonePitch < tuningParams.lowFreqCeiling)
                Debug.Log("Low Noise: " + microphonePitch);
            else if (microphonePitch > tuningParams.highFreqFloor && microphonePitch > tuningParams.highFreqCeiling) 
                Debug.Log("High Noise: " + microphonePitch);
            /*
            Debug.Log("Spectrum 0: " + spectrum[0]*100.0f);
            Debug.Log("Spectrum 1: " + spectrum[1]*100.0f);
            Debug.Log("Spectrum 2: " + spectrum[2]*100.0f);
            */
        }
        else
        {
            //InitMicrophone();
            Debug.Log("Play it again sam");
            //_audioSource.clip = Microphone.Start(_microphoneName, true, 10,44100);
            _audioSource.Play();
        }

    }
    




}
