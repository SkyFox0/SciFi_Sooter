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
        //public bool HeadShoot;
        //public EnemyHealthComponent EnemyHealthComponent;

        private void Update()
        {
            Debug.DrawRay(ShootPoint.position, ShootDirection.forward * 30, Color.red);
        }


        public void Shoot()
        {
            Vector3 shootPosition = ShootPoint.position;
            var direction = ShootDirection.forward;
            //var direction = ShootPoint.forward;

            if (Physics.Raycast(shootPosition, direction, out var hitInfo))
            {
                //Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
                Debug.Log("Hit! Object = " + hitInfo.collider.name);

                if (hitInfo.collider.name == "Head")
                { 
                    Debug.Log("’≈ƒÿŒ“1!!!");
                    //HeadShoot = true;
                    if (hitInfo.collider.TryGetComponent(out HeadShoot HeadShoot))
                    {

                        try
                        {
                            Debug.Log("’≈ƒÿŒ“2!!!");
                            HeadShoot.TakeHeadShoot(Damage*3);
                        }
                        catch { }
                    }
                }

                
                if (hitInfo.collider.TryGetComponent(out HealthComponent healthComponent))
                {

                    try
                    {                        
                        healthComponent.TakeDamage(Damage);
                    }
                    catch { }
                }
                

                if (hitInfo.collider.TryGetComponent(out EnemyHealthComponent enemyHealthComponent))
                {

                    try
                    {
                            enemyHealthComponent.TakeDamage(Damage);
                    }
                    catch { }

                }
                //—¡–¿—€¬¿≈Ã »Õƒ» ¿“Œ– ’≈ƒÿŒ“¿
                //HeadShoot = false;

                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForceAtPosition(direction * _shoottForse, hitInfo.point, ForceMode.Impulse);

                }
            }
        }
    }
}
