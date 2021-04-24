using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "VoiceBoxTuning", menuName = "VoiceBoxTuning", order = 0)]
    public class VoiceboxTuningParams : ScriptableObject
    {
        


        public float lowFreqFloor;

        public float lowFreqCeiling;
        
        public float highFreqFloor;
        
        public float highFreqCeiling;
    }
}