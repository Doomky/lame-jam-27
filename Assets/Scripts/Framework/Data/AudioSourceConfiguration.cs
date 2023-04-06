using UnityEngine;

namespace Framework.Data.Audio
{
    [CreateAssetMenu(menuName = "Framework/Data/System/AudioSourceConfiguration")]
    public class AudioSourceConfiguration : ScriptableObject
    {
        [SerializeField] 
        public float panStereo = 0;
        
        [SerializeField] 
        public float dopplerLevel = 0;
        
        [SerializeField] 
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Custom;
        
        [SerializeField] 
        public AudioSourceCurveType customCurveType = AudioSourceCurveType.CustomRolloff;
        
        [SerializeField] 
        public AnimationCurve customCurve = AnimationCurve.Linear(0, 1, 1, 0);
        
        [SerializeField] 
        public float maxDistance = 20;
    }
}