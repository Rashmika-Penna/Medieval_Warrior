using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon_Config : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animator_override = null;
        [SerializeField] Weapon equipped_weapon = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] float weapon_damage = 5f;
        [SerializeField] float percentage_bonus = 0f;
        [SerializeField] float weapon_range = 2f;
        [SerializeField] bool is_right_handed = true;
        const string weapon_name = "Weapon";

        public Weapon Spawn(Transform right_hand, Transform left_hand, Animator animator)
        {
            Destroy_Old_Weapon(right_hand, left_hand);
            Weapon weapon = null;

            if(equipped_weapon != null)
            {
                Transform hand = Get_Transform(right_hand, left_hand);
                weapon = Instantiate(equipped_weapon, hand);
                weapon.gameObject.name = weapon_name;
            }

            var override_controller = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animator_override != null)
            {
                animator.runtimeAnimatorController = animator_override;
            }
            else if (override_controller != null)
            {
                animator.runtimeAnimatorController = override_controller.runtimeAnimatorController;
            }

            return weapon;
        }        

        private void Destroy_Old_Weapon(Transform right_hand, Transform left_hand)
        {
            Transform old_weapon = right_hand.Find(weapon_name);

            if(old_weapon == null)
            {
                old_weapon = left_hand.Find(weapon_name);
            }

            if(old_weapon == null) { return; }

            old_weapon.name = "DESTROYING";
            Destroy(old_weapon.gameObject);
        }

        private Transform Get_Transform(Transform right_hand, Transform left_hand)
        {
            Transform hand;

            if (is_right_handed) { hand = right_hand; }
            else hand = left_hand;
            return hand;
        }

        public bool Has_Projectile()
        {
            return projectile != null;
        }

        public void Launch_Projectile(Transform right_hand, Transform left_hand, Health target, GameObject instigator, float calculated_damage)
        {
            Projectile projectile_instance = Instantiate(projectile, Get_Transform(right_hand, left_hand).position, Quaternion.identity);
            projectile_instance.Set_Target(target, instigator, calculated_damage);
        }

        public float Get_Weapon_Range()
        {
            return weapon_range;
        }

        public float Get_Percentage_Bonus()
        {
            return percentage_bonus;
        }

        public float Get_Weapon_Damage()
        {
            return weapon_damage;
        }
    }
}