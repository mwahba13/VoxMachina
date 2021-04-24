using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "VoiceBoxTuning", menuName = "VoiceBoxTuning", order = 0)]
    public class VoiceboxTuningParams : ScriptableObject
    {

        public Color lowPitchColor;

        public float lowFreqFloor;

        public float lowFreqCeiling;

        public Color highPitchColor;
        
        public float highFreqFloor;
        
        public float highFreqCeiling;
    }
}