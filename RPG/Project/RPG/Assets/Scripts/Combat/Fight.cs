using System.Collections.Generic;
using UnityEngine;
using Utility;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stat;

namespace RPG.Combat
{
    public class Fight : MonoBehaviour, IAction, ISaveable, IModifier_Provider
    {
        [SerializeField] Weapon_Config default_weapon = null;
        [SerializeField] Transform right_hand = null;
        [SerializeField] Transform left_hand = null;
        [SerializeField] float time_between_attacks = 0.5f;

        Weapon_Config current_weapon_config;
        LazyValue<Weapon> current_weapon;
        Health target;
        private float time_since_attack = Mathf.Infinity;

        private void Awake()
        {
            current_weapon_config = default_weapon;
            current_weapon = new LazyValue<Weapon>(Setup_Default_Weapon);
        }

        private Weapon Setup_Default_Weapon()
        {
            return Attach_Weapon(default_weapon);
        }

        private void Start()
        {
            current_weapon.ForceInit();
        }

        private void Update()
        {
            time_since_attack += Time.deltaTime;

            if(target == null) { return; }

            if(target.Is_Dead()) { return; }

            if(!Is_In_Range(target.transform))
            {
                GetComponent<Mover>().Move_To(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                Attack_System();
            }
        }

        public void Equip_Weapon(Weapon_Config weapon)
        {
            current_weapon_config = weapon;
            current_weapon.value = Attach_Weapon(weapon);
        }

        private Weapon Attach_Weapon(Weapon_Config weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(right_hand, left_hand, animator);
        }

        public Health Get_Target()
        {
            return target;
        }

        private void Attack_System()
        {
            transform.LookAt(target.transform);

            if(time_since_attack > time_between_attacks)
            {
                Trigger_Attack();
                time_since_attack = 0;       
            }
        }

        private void Trigger_Attack()
        {
            GetComponent<Animator>().ResetTrigger("Stop_Attack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        public bool Can_Attack(GameObject combat_target)
        {
            if(combat_target == null)
            {
                return false;
            }

            if(!GetComponent<Mover>().Can_Move_To(combat_target.transform.position) && !Is_In_Range(combat_target.transform))
            {
                return false;
            }

            Health target_to_test = combat_target.GetComponent<Health>();
            return target_to_test != null && !target_to_test.Is_Dead();
        }

        // Aniamtion Event Melee
        private void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<Base_Stats>().Get_Stat(Stats.Damage);
            
            if(current_weapon.value != null)
            {
                current_weapon.value.On_Hit();
            }

            if (current_weapon_config.Has_Projectile())
            {
                current_weapon_config.Launch_Projectile(right_hand, left_hand, target, gameObject, damage);
            }
            else
            {
                target.Take_Damage(gameObject, damage);
            }            
        }

        // Animation Event Bow
        private void Shoot()
        {
            Hit();
        }

        private bool Is_In_Range(Transform target_transform)
        {
            return Vector3.Distance(transform.position, target_transform.transform.position) <= current_weapon_config.Get_Weapon_Range();
        }

        public void Attack(GameObject combat_target)
        {
            GetComponent<Action_Scheduler>().Start_Action(this);
            target = combat_target.GetComponent<Health>();
        }

        public void Cancel()
        {
            Stop_Attack();
            target = null;
            GetComponent<Mover>().Cancel();
        }   
        
        private void Stop_Attack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("Stop_Attack");
        }
        
        public IEnumerable<float> Get_Additive_Modifiers(Stats stat)
        {
            if(stat == Stats.Damage)
            {
                yield return current_weapon_config.Get_Weapon_Damage();
            }
        }

        public IEnumerable<float> Get_Percentage_Modifiers(Stats stat)
        {
            if (stat == Stats.Damage)
            {
                yield return current_weapon_config.Get_Percentage_Bonus();
            }
        }

        public object CaptureState()
        {
            return current_weapon_config.name;
        }

        public void RestoreState(object state)
        {
            string weapon_name = (string)state;
            Weapon_Config weapon = UnityEngine.Resources.Load<Weapon_Config>(weapon_name);
            Equip_Weapon(weapon);
        }
    }
}