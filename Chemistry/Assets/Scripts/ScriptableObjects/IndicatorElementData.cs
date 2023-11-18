using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChemistryIndicator", menuName = "AddChemistryIndicator")]
    public class IndicatorElementData : ScriptableObject
    {
        public IndicatorName indicatorName;
        public Color colorIndicatorWithSourEnvironment;
        public Color colorIndicatorWithAlkalineEnvironment;
        public Color colorIndicatorWithNeutralEnvironment;
    }
    
    public enum IndicatorName
    {
        Litmus,
        Phenolphthalein,
        Methylorange
    }
}