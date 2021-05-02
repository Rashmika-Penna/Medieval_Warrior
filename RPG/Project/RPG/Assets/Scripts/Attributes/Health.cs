using Utility;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;
using RPG.Stat;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerated_health = 70f;
        [SerializeField] Take_Damage_Event take_damage;
        [SerializeField] UnityEvent death;

        [System.Serializable]
        public class Take_Damage_Event : UnityEvent<float>
        {

        }

        LazyValue<float> health;
        private bool is_dead = false;

        private void Awake()
        {
            health = new LazyValue<float>(Get_Initial_Health);
        }

        private float Get_Initial_Health()
        {
            return GetComponent<Base_Stats>().Get_Stat(Stats.Health);
        }

        private void Start()
        {
            health.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<Base_Stats>().On_Level_Up += Regenerate_Health;
        }

        private void OnDisable()
        {
            GetComponent<Base_Stats>().On_Level_Up -= Regenerate_Health;
        }

        public bool Is_Dead()
        {
            return is_dead;
        }

        public float Get_Max_Health_Points()
        {
            return GetComponent<Base_Stats>().Get_Stat(Stats.Health);
        }

        public void Take_Damage(GameObject instigator, float damage)
        {
            health.value = Mathf.Max(health.value - damage, 0);

            if (health.value == 0)
            {
                death.Invoke();
                Death();
                Award_XP(instigator);
            }
            else
            {
                take_damage.Invoke(damage);
            }
        }

        public void Heal(float restore_health)
        {
            health.value = Mathf.Min(health.value + restore_health, Get_Max_Health_Points());            
        }

        public float Get_Health_Points()
        {
            return health.value;
        }

        public float Get_Health_Percentage()
        {
            return Get_Health_Fraction() * 100;
        }

        public float Get_Health_Fraction()
        {
            return health.value / GetComponent<Base_Stats>().Get_Stat(Stats.Health);
        }

        private void Death()
        {
            if(is_dead) { return; }

            is_dead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<Action_Scheduler>().Cancel_Current_Action();
        }

        private void Award_XP(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();

            if(exp == null) { return; }

            exp.Gain_XP(GetComponent<Base_Stats>().Get_Stat(Stats.XP_Reward));
        }

        private void Regenerate_Health()
        {
            float regenerated_health_points = GetComponent<Base_Stats>().Get_Stat(Stats.Health) * (regenerated_health / 100);
            health.value = Mathf.Max(health.value, regenerated_health_points);
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if(health.value == 0)
            {
                Death();
            }
        }
    }
}