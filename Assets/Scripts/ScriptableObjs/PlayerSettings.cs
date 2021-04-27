using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        public bool isInvertY;
    }
}