using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool is_homing = true;
        [SerializeField] float max_life_time = 10f;
        [SerializeField] GameObject[] destroy_on_hit = null;
        [SerializeField] float life_after_impact = 2f;
        [SerializeField] UnityEvent hit_sfx;
        Health target = null;
        GameObject instigator = null;
        float damage = 0f;

        private void Start()
        {
            transform.LookAt(Get_Aim_Location());
        }

        private void Update()
        {
            if(target == null) { return; }

            if(is_homing && !target.Is_Dead())
            {
                transform.LookAt(Get_Aim_Location());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void Set_Target(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, max_life_time);
        }

        private Vector3 Get_Aim_Location()
        {
            CapsuleCollider target_capsule = target.GetComponent<CapsuleCollider>();

            if (target_capsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * target_capsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != target) { return; }

            if(target.Is_Dead()) { return; }
            
            target.Take_Damage(instigator, damage);

            speed = 0;

            hit_sfx.Invoke();

            foreach(GameObject to_destroy in destroy_on_hit)
            {
                Destroy(to_destroy);
            }

            Destroy(gameObject, life_after_impact);            
        }
    }
}