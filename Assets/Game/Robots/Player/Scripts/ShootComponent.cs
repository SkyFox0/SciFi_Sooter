using UnityEngine;

namespace StarterAssets
{
    public class ShootComponent : MonoBehaviour
    {

        [Header("Damage")]
        //[SerializeField, Min(0f)] private float _bulletForse = 100f;
        public float _shoottForse = 100f;
        public Transform ShootPoint;
        public Transform ShootDirection;
        public int Damage;


        public void Shoot()
        {
            Vector3 shootPosition = ShootPoint.position;
            var direction = ShootDirection.forward;
            //var direction = ShootPoint.forward;

            if (Physics.Raycast(shootPosition, direction, out var hitInfo))
            {
                Debug.Log("Hit! Object = " + hitInfo.collider.name);
                Debug.Log(direction.ToString());
                if (hitInfo.collider.TryGetComponent(out HealthComponent healthComponent))
                {

                    try
                    {
                        //healthComponent.HitSound.Play();
                        healthComponent.TakeDamage(Damage);
                    }
                    catch { }
                }

                if (hitInfo.collider.TryGetComponent(out EnemyHealthComponent enemyHealthComponent))
                {

                    try
                    {
                        //healthComponent.HitSound.Play();
                        enemyHealthComponent.TakeDamage(Damage);
                    }
                    catch { }

                }

                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForceAtPosition(direction * _shoottForse, hitInfo.point, ForceMode.Impulse);

                }
            }
        }
    }
}
