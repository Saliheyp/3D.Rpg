using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName ="Weapons/MakeNewWeapon",order =0)]
    
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animationOverride = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isRightHand = true;
        const string weaponName = "Weapon";




        public void Spawn (Transform rightHandTransform,Transform leftHandTransform, Animator animator) 
        {
            DestroyOldWeapon(rightHandTransform,leftHandTransform);
            if (weaponPrefab !=null)
            {
                Transform handTransform;
                if (isRightHand)
                {
                    handTransform = rightHandTransform;
                }
                else
                {
                    handTransform = leftHandTransform;
                }
                GameObject weapon = Instantiate(weaponPrefab,handTransform);
                weapon.name = weaponName;

            }
            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
            }
            
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        public void LunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health target) 
        {
            Transform handTransform;
            if (isRightHand)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
                
            }
            Projectile projectileInstance = Instantiate(projectile, handTransform.position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
        private void DestroyOldWeapon (Transform rightHand, Transform leftHand) 
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name ="Destroy";
            Destroy(oldWeapon.gameObject);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
        public float GetRange()
        {
            return weaponRange;
        }
    }
}