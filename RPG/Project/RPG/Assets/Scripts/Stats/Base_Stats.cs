using System;
using Utility;
using UnityEngine;

namespace RPG.Stat
{
    public class Base_Stats : MonoBehaviour
    {
        [Range(1,99)] [SerializeField] int starting_level = 1;
        [SerializeField] Character_Class character_class;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject level_up_effect = null;
        [SerializeField] bool use_modifiers = false;

        public event Action On_Level_Up;

        LazyValue<int> current_level;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            current_level = new LazyValue<int>(Calculate_Level);
        }

        private void Start()
        {
            current_level.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.On_XP_Gained += Update_Level;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.On_XP_Gained -= Update_Level;
            }
        }

        private void Update_Level()
        {
            int new_level = Calculate_Level();

            if(new_level > current_level.value)
            {
                current_level.value = new_level;
                Level_Up_Effect();
                On_Level_Up();
            }
        }

        private void Level_Up_Effect()
        {
            Instantiate(level_up_effect, transform);
        }

        public float Get_Stat(Stats stat)
        {
            return (Get_Base_Stat(stat) + Get_Additive_Modifier(stat)) * (1 + Get_Percentage_Modifiers(stat)/100);
        }

        private float Get_Base_Stat(Stats stat)
        {
            return progression.Get_Stat(stat, character_class, Get_Level());
        }
        
        public int Get_Level()
        {
            return current_level.value;
        }

        private float Get_Additive_Modifier(Stats stat)
        {
            if(!use_modifiers) { return 0; }

            float total = 0;

            foreach(IModifier_Provider provider in GetComponents<IModifier_Provider>())
            {
                foreach(float modifier in provider.Get_Additive_Modifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float Get_Percentage_Modifiers(Stats stat)
        {
            if(!use_modifiers) { return 0; }

            float total = 0;

            foreach (IModifier_Provider provider in GetComponents<IModifier_Provider>())
            {
                foreach (float modifier in provider.Get_Percentage_Modifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int Calculate_Level()
        {
            Experience experience = GetComponent<Experience>();

            if(experience == null) { return starting_level; }

            float current_xp = GetComponent<Experience>().Get_XP_Points();
            int max_level = progression.Get_Levels(Stats.XP_Levelup, character_class);

            for(int level = 1; level <= max_level; level++)
            {
                float xp_to_levelup = progression.Get_Stat(Stats.XP_Levelup, character_class, level);

                if(xp_to_levelup > current_xp)
                {
                    return level;
                }
            }

            return max_level + 1;
        }
    }
}