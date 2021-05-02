using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stat
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] Progression_Character_Class[] character_classes = null;

        Dictionary<Character_Class, Dictionary<Stats, float[]>> lookup_table = null; 

        public float Get_Stat(Stats stat, Character_Class character_class, int level)
        {
            Build_Lookup();

            float[] levels = lookup_table[character_class][stat];

            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int Get_Levels(Stats stat, Character_Class character_class)
        {
            Build_Lookup();

            float[] levels = lookup_table[character_class][stat];
            return levels.Length;
        }

        private void Build_Lookup()
        {
            if(lookup_table != null) { return; }

            lookup_table = new Dictionary<Character_Class, Dictionary<Stats, float[]>>();

            foreach(Progression_Character_Class progression_class in character_classes)
            {
                var stat_lookup_table = new Dictionary<Stats, float[]>();

                foreach(Progression_Stat progression_stat in progression_class.stats)
                {
                    stat_lookup_table[progression_stat.stat] = progression_stat.levels;                                    
                }

                lookup_table[progression_class.character_class] = stat_lookup_table;
            }
        }

        [System.Serializable]
        class Progression_Character_Class
        {
            public Character_Class character_class;
            public Progression_Stat[] stats;
        }

        [System.Serializable]
        class Progression_Stat
        {
            public Stats stat;
            public float[] levels;
        }
    }
}