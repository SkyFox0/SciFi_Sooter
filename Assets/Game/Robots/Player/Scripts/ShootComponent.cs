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
            //int layerMask = 1 << 8;

            Vector3 shootPosition = ShootPoint.position;
            var direction = ShootDirection.forward;
            //var direction = ShootPoint.forward;

            if (Physics.Raycast(shootPosition, direction, out var hitInfo, 50f))   //, layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
                //Debug.Log("Hit! Object = " + hitInfo.collider.name);

                if (hitInfo.collider.name == "Head")
                { 
                    Debug.Log("������1!!!");
                    //HeadShoot = true;
                    if (hitInfo.collider.TryGetComponent(out HeadShoot HeadShoot))
                    {

                        try
                        {
                            //Debug.Log("������2!!!");
                            HeadShoot.TakeHeadShoot(Damage*3);
                        }
                        catch { }
                    }
                }

                if (hitInfo.collider.name == "BackPack")
                {
                    Debug.Log("���� �� �������!");
                    if (hitInfo.collider.TryGetComponent(out BackPackDamage BackPackDamage))
                    {

                        try
                        {
                            //Debug.Log("������2!!!");
                            BackPackDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("������ ������� �� ������"); }
                    }
                }

                if (hitInfo.collider.name == "ShoulderPadCTRL_Left")
                {
                    Debug.Log("���� �� ������ ����������!");
                    if (hitInfo.collider.TryGetComponent(out ShoulderPadLeftDamage ShoulderPadLeftDamage))
                    {

                        try
                        {
                            //Debug.Log("������2!!!");
                            ShoulderPadLeftDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("������ ������ ���������� �� ������"); }
                    }
                }

                if (hitInfo.collider.name == "ShoulderPadCTRL_Right")
                {
                    Debug.Log("���� �� ������� ����������!");
                    if (hitInfo.collider.TryGetComponent(out ShoulderPadRightDamage ShoulderPadRightDamage))
                    {

                        try
                        {
                            //Debug.Log("������2!!!");
                            ShoulderPadRightDamage.TakeDamage(Damage);
                        }
                        catch { Debug.Log("������ ������� ���������� �� ������"); }
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
                    Debug.Log("���� �� �������!");

                    try
                    {
                            enemyHealthComponent.TakeDamage(Damage);
                    }
                    catch { }

                }
                //���������� ��������� �������
                //HeadShoot = false;
                
                if (hitInfo.rigidbody != null)
                {
                    hitInfo.rigidbody.AddForceAtPosition(direction * _shoottForse, hitInfo.point, ForceMode.Impulse);

                }
            }
            //������� �������
            if (FirstPersonController.isSight)
            {
                My_Weapon_Controller.Shooting();
                //My_Weapon_Controller.Sighting();
                //My_Weapon_Controller.CinemachineCameraTarget.transform.position = My_Weapon_Controller.SightPoint.position;
            }
                

        }
    }
}
