using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

namespace StarterAssets
{
    public class ShootComponent : MonoBehaviour
    {
        public FirstPersonController FirstPersonController;
        public My_Weapon_Controller My_Weapon_Controller;

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
            //Debug.DrawRay(ShootPoint.position, ShootDirection.forward * 30, Color.red);
        }


        public void Shoot()
        {
            Vector3 shootPosition = ShootPoint.position;
            var direction = ShootDirection.forward;
            //var direction = ShootPoint.forward;

            if (Physics.Raycast(shootPosition, direction, out var hitInfo))
            {
                //Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
                //Debug.Log("Hit! Object = " + hitInfo.collider.name);

                if (hitInfo.collider.name == "Head")
                { 
                    Debug.Log("ХЕДШОТ1!!!");
                    //HeadShoot = true;
                    if (hitInfo.collider.TryGetComponent(out HeadShoot HeadShoot))
                    {

                        try
                        {
                            //Debug.Log("ХЕДШОТ2!!!");
                            HeadShoot.TakeHeadShoot(Damage*3);
                        }
                        catch { }
                    }
                }

                if (hitInfo.collider.name == "BackPack")
                {
                    Debug.Log("Урон по рюкзаку!");
                    if (hitInfo.collider.TryGetComponent(out BackPackDamage BackPackDamage))
                    {

                        try
                        {
                            //Debug.Log("ХЕДШОТ2!!!");
                            BackPackDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("Скрипт рюкзака не найден"); }
                    }
                }

                if (hitInfo.collider.name == "ShoulderPadCTRL_Left")
                {
                    Debug.Log("Урон по левому наплечнику!");
                    if (hitInfo.collider.TryGetComponent(out ShoulderPadLeftDamage ShoulderPadLeftDamage))
                    {

                        try
                        {
                            //Debug.Log("ХЕДШОТ2!!!");
                            ShoulderPadLeftDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("Скрипт левого наплечника не найден"); }
                    }
                }

                if (hitInfo.collider.name == "ShoulderPadCTRL_Right")
                {
                    Debug.Log("Урон по правому наплечнику!");
                    if (hitInfo.collider.TryGetComponent(out ShoulderPadRightDamage ShoulderPadRightDamage))
                    {

                        try
                        {
                            //Debug.Log("ХЕДШОТ2!!!");
                            ShoulderPadRightDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("Скрипт правого наплечника не найден"); }
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
                    Debug.Log("Урон по корпусу!");

                    try
                    {
                            enemyHealthComponent.TakeDamage(Damage);
                    }
                    catch { }

                }
                //СБРАСЫВАЕМ ИНДИКАТОР ХЕДШОТА
                //HeadShoot = false;
                
                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForceAtPosition(direction * _shoottForse, hitInfo.point, ForceMode.Impulse);

                }
            }
            //возврат прицела
            if (FirstPersonController.isSight)
            {
                My_Weapon_Controller.Shooting();
                //My_Weapon_Controller.Sighting();
                //My_Weapon_Controller.CinemachineCameraTarget.transform.position = My_Weapon_Controller.SightPoint.position;
            }
                

        }
    }
}
