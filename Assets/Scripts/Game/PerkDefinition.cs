using UnityEngine;

namespace BeatMayhem.Game
{
    [CreateAssetMenu(menuName = "BeatMayhem/Perk", fileName = "PerkDefinition")]
    public class PerkDefinition : ScriptableObject
    {
        public string perkName;
        [TextArea]
        public string description;
    }
}
