using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StarterAssets
{
    public class My_Weapon_Controller : MonoBehaviour
    {
        public Animator Animator;         
        public FirstPersonController FirstPersonController;
        public Ui_Control Ui_Control;
        private float LastShoot;        

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        public Transform CameraPoint;
        public Transform SightPoint;
        public Transform SightPointShoot;
        public CinemachineVirtualCamera Camera;
        public float _sigthFocus = 30;
        public float _normalFocus = 65;

        [Header("Shoot")]
        public ShootComponent ShootComponent;
        public EGA_DemoLasers EGA_DemoLasers;
        public float _shootSpeed;

        [Header("Sounds")]
        public AudioSource PlaySound;
        public AudioClip Shoot;
        public AudioClip Reload;
        public AudioClip Empty;
        public AudioClip Hit;
        public AudioClip GetAmmo;
        public AudioClip GetGrenades;

        [Header("Switch Weapon")]
        public bool isRifle;
        public bool isGrenade;

        [Header("Ammo")]
        public int _totalAmmo;
        public int _maxWeaponAmmo = 10;
        public int _currentWeaponAmmo = 5;
        public int _maxAmmo = 300;
        public TextMesh _ammoText;
        public TextMesh Total_Ammo_Text;

        

        [Header("Grenades")]
        public float _throwForce;
        public float _forceTimer;
        public bool _isThrowing;
        public enum TypeGrenade
        {
            EMP,
            Explosive,
            Smoke,
            Flame,
            Bomb
        };
        public TypeGrenade _typeGrenade = TypeGrenade.EMP;
        public int _totalEMPGrenades = 0;
        public int _totalEXPLGrenades = 0;
        public int _maxGrenades = 5;
        
        public GameObject EMPGrenade;
        public GameObject EXPLGrenade;

        private GameObject Instante;
        public Transform ThrowPoint;
        public GameObject CameraDirection;
        public TMP_Text GrenadeText;
        public Image GrenadeBar;


        private void Start()
        {
            isRifle = true;
            isGrenade = false;

            _ammoText.text = _currentWeaponAmmo.ToString();
            Total_Ammo_Text.text = _totalAmmo.ToString();
            LastShoot = Time.time;
            //Animator = GetComponentInChildren<Animator>();
            _shootSpeed = 0.5f;
            Animator.SetBool("isReloading", false);
            if (_currentWeaponAmmo < _maxWeaponAmmo)
            {
                Animator.SetBool("isNeedToReload", true);
            }
            FirstPersonController = GetComponent<FirstPersonController>();
        }

        private void FixedUpdate()
        {   //прицеливание
            CameraSightOn();
            CameraSightOff();
            Camerashooting();
        }

        public void OnShoot(InputValue value)
        {
            if (!Animator.GetBool("isDead")&&!Animator.GetBool("isRun"))
            {
                if ((Time.time - LastShoot > _shootSpeed) && (_currentWeaponAmmo > 0) && (!Animator.GetBool("isReloading")))  //_shootSpeed = 0.5f;
                {
                    Animator.SetTrigger("Shoot");
                    ShootComponent.Shoot();
                    EGA_DemoLasers.Shoot();

                    PlaySound.clip = Shoot;
                    PlaySound.Play();
                    
                    LastShoot = Time.time;
                    _currentWeaponAmmo -= 1;
                    _ammoText.text = _currentWeaponAmmo.ToString();
                    Animator.SetBool("isNeedToReload", true);
                    //Debug.Log("Shoot!");
                }
                else
                { 
                    if (!Animator.GetBool("isReloading"))
                    {
                        PlaySound.clip = Empty; //патроны закончились
                        PlaySound.Play();
                    }
                    
                }
            }
                
        }

        public void OnReload(InputValue value)
        {
            if ((_totalAmmo > 0) && Animator.GetBool("isNeedToReload") && !Animator.GetBool("isReloading") 
                && Animator.GetBool("isGround") && (!Animator.GetBool("isDead") && !Animator.GetBool("isRun")))
            {

                //Debug.Log("ѕерезар€дка");
                Ui_Control.Scope_Off();
                Animator.SetTrigger("Reload");
                //------
                //&& !Animator.GetBool("isMove") 
                Animator.SetBool("isMove", false);
                Animator.SetBool("isReloading", true);
                PlaySound.clip = Reload;
                PlaySound.Play();
                //Animator.SetBool("isReloading", true);                
            }
            else
            {
                if (_totalAmmo == 0)
                {
                    //«вук отсутстви€ патронов
                    PlaySound.clip = Empty; //патроны закончились
                    PlaySound.Play();
                }
            }
        }

        public void OnSelectWeapon(InputValue value)
        {
            Debug.Log("Scroll!");
        }


        public void AddAmmo(int _ammo)
        {
            // игрок подн€л патроны
            //PlaySound.clip = GetAmmo;
            PlaySound.PlayOneShot(GetAmmo);
            _totalAmmo += _ammo;
            if (_totalAmmo > _maxAmmo)
            {
                _totalAmmo = _maxAmmo;               
            }
            
            Total_Ammo_Text.text = _totalAmmo.ToString();
        }

        //  -----------гранаты-------------
        public void AddEMPGrenades(int _grenades)
        {
            // игрок подн€л гранаты
            PlaySound.PlayOneShot(GetGrenades);
            _totalEMPGrenades += _grenades;
            GrenadeText.text = _totalEMPGrenades.ToString();
        }

        /*public void AddEXPLGrenades(int _grenades)
        {
            // игрок подн€л гранаты
            PlaySound.PlayOneShot(GetGrenades);
            _totalEXPLGrenades += _grenades;
        }*/

        public void OnGrenade(InputValue value)
        {
            if (!_isThrowing)
            {
                //«апуск таймера нажати€ кнопки метани€ гранаты
                _forceTimer = Time.time;
                _isThrowing = true;
            }
            else
            {
                if (Animator.GetBool("isDead"))
                {
                    _forceTimer = 0.1f;
                    Debug.Log("Ѕросок не удалс€");                    
                }
                else
                {
                    //остановка таймера нажати€ кнопки метани€ гранаты
                    _forceTimer = Time.time - _forceTimer;
                    Debug.Log("¬рем€ нажати€ броска" + _forceTimer.ToString());
                }
                if (_forceTimer > 2f)
                {
                    _forceTimer = 2f;
                }
                if (!Animator.GetBool("isDead") && _forceTimer < 0.5f)
                {
                    _forceTimer = 0.5f;
                }
                _throwForce = 15 * _forceTimer;
                ThrowGrenade();
            }            
        }

        private void Update()
        {
            if (_isThrowing)
            {
                GrenadeBar.fillAmount = (Time.time - _forceTimer) / 2f;
            }
            else
            {
                GrenadeBar.fillAmount = 0f;

            }
            
        }

        public void ThrowGrenade()
        {
            if (!Animator.GetBool("isMove") && Animator.GetBool("isGround") && !Animator.GetBool("isDead") && !Animator.GetBool("isRun")) 
            {
                // ѕриготовить гранату к броску - анимаци€...
                //isRifle = false;  // убрали винтовку
                //isGrenade = true;  // достали гранату
                if (_totalEMPGrenades > 0 && !Animator.GetBool("isThrowing"))   // если гранаты есть
                {
                    // бросок гранаты
                    Animator.SetBool("isThrowing", true);
                    Instante = Instantiate(EMPGrenade, ThrowPoint.position, ThrowPoint.transform.rotation);
                    Instante.GetComponent<Rigidbody>().AddForce(CameraDirection.transform.forward * _throwForce, ForceMode.VelocityChange); // ForceMode.Force // ForceMode.Acceleration // ForceMode.Impulse 
                    Instante.GetComponent<Rigidbody>().AddForce(CameraDirection.transform.up * 3f, ForceMode.VelocityChange); // ForceMode.Force // ForceMode.Acceleration // ForceMode.Impulse 
                    Instante.GetComponent<Rigidbody>().AddTorque(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f), ForceMode.VelocityChange);
                    //Instante.GetComponent<Rigidbody>().AddForce(ThrowPoint.transform.up * 5, ForceMode.VelocityChange);
                    //Instante.GetComponent<Rigidbody>().AddForce(ThrowPoint.transform.right * 4, ForceMode.VelocityChange);
                    //Grenade.GetComponent<Rigidbody>().AddForce(Random.Range(-1, 1), 3, Random.Range(-1, 1), ForceMode.VelocityChange); // ForceMode.Force // ForceMode.Acceleration // ForceMode.Impulse 

                    _totalEMPGrenades -= 1;
                    GrenadeText.text = _totalEMPGrenades.ToString();

                }
                else
                {
                    // «вук неудачи
                    PlaySound.clip = Empty; //нет гранат
                    PlaySound.Play();
                }
            }            
            _isThrowing = false;
        }



        public void EndReload()
        {
            int _needToReload = _maxWeaponAmmo - _currentWeaponAmmo; // сколько надо дл€ перезар€дки
            //”меньшение общего количества патронов
            
            if (_totalAmmo > _needToReload)   // патронов больше чем нужно
            {
                _totalAmmo = _totalAmmo - _needToReload;
                _currentWeaponAmmo = _maxWeaponAmmo;
            }
            else  // если патронов меньше чем нужно но больше 0 - частична€ перезар€дка
            {                
                _currentWeaponAmmo += _totalAmmo;
                _totalAmmo = 0; // _totalAmmo - (_maxWeaponAmmo - _currentWeaponAmmo);
            }
            
            
            _ammoText.text = _currentWeaponAmmo.ToString();
            Total_Ammo_Text.text = _totalAmmo.ToString();
            //-----
            Animator.SetBool("isReloading", false);
            Animator.SetBool("isNeedToReload", false);
        }

        public void OnSight(InputValue value)
        {
            if (!Animator.GetBool("isReloading") && Animator.GetBool("isGround") && (!Animator.GetBool("isDead") && !Animator.GetBool("isRun")))
            {
                Animator.SetBool("isSight", true);
                FirstPersonController.isSight = true;
                Ui_Control.Scope_On();
                //CinemachineCameraTarget.transform.localPosition = SightPoint.localPosition;
                CinemachineCameraTarget.transform.position = SightPoint.position;
            }                        
        }

        public void Sighting()
        {
            CinemachineCameraTarget.transform.position = SightPoint.position;
        }

        public void Shooting()
        {
            CinemachineCameraTarget.transform.position = SightPointShoot.position;

            Invoke("Sighting", 0.3f);
            
        }

        public void Camerashooting()
        {
            if (Animator.GetBool("isSight") && Animator.GetBool("isShooting"))
            {
                if (Camera.m_Lens.FieldOfView < _sigthFocus + 10)
                {
                    Camera.m_Lens.FieldOfView += 11f;
                    Ui_Control.Scope_On();
                }                
            }
        }



        public void CameraSightOn()
        {
            if (Animator.GetBool("isSight") && !Animator.GetBool("isShooting") && !Animator.GetBool("isRun"))
            {
                if (Camera.m_Lens.FieldOfView > _sigthFocus)
                {
                    Camera.m_Lens.FieldOfView -= 3f;
                    Ui_Control.Scope_On();

                }
                if (Camera.m_Lens.Dutch > -10)
                {
                    Camera.m_Lens.Dutch -= 0.5f;
                }
            }
        }

        public void CameraSightOff()
        {
            if (!Animator.GetBool("isSight"))
            {
                if (Camera.m_Lens.FieldOfView < _normalFocus)
                {
                    Camera.m_Lens.FieldOfView += 3f;
                }
                if (Camera.m_Lens.Dutch < 0)
                {
                        Camera.m_Lens.Dutch += 0.5f;
                }
            }
        }

        public void OnSightOff(InputValue value)
        {
            Animator.SetBool("isSight", false);
            FirstPersonController.isSight = false;
            Ui_Control.Scope_Off();
            //Camera.m_Lens.FieldOfView = 60f;
            //Debug.Log("¬ыключить прицеливание");
            //CinemachineCameraTarget.transform.localPosition = CameraPoint.localPosition;
            CinemachineCameraTarget.transform.position = CameraPoint.position;
        }

    }
}
