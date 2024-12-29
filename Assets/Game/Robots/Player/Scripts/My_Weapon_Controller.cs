using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class My_Weapon_Controller : MonoBehaviour
    {
        public Animator Animator;
        private float LastShoot;
        //public Camera Camera;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        public Transform CameraPoint;
        public Transform SightPoint;
        public CinemachineVirtualCamera Camera;

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


        [Header("Ammo")]
        public int _totalAmmo;
        public int _maxWeaponAmmo = 10;
        public int _currentWeaponAmmo = 5;
        public int _maxAmmo = 300;
        public TextMesh _ammoText;
        public TextMesh Total_Ammo_Text;

        public FirstPersonController FirstPersonController;
        public Ui_Control Ui_Control;


        private void Start()
        {
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

        private void Update()
        {   //прицеливание
            CameraSightOn();
            CameraSightOff();
        }


        public void OnShoot(InputValue value)
        {
            if (!Animator.GetBool("isDead"))
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
            if ((_totalAmmo > 0) && Animator.GetBool("isNeedToReload") && !Animator.GetBool("isReloading") && Animator.GetBool("isGround") && (!Animator.GetBool("isDead")))
            {

                //Debug.Log("Перезарядка");
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
                    //Звук отсутствия патронов
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
            // игрок поднял патроны
            //PlaySound.clip = GetAmmo;
            PlaySound.PlayOneShot(GetAmmo);
            _totalAmmo += _ammo;
            if (_totalAmmo > _maxAmmo)
            {
                _totalAmmo = _maxAmmo;               
            }
            
            Total_Ammo_Text.text = _totalAmmo.ToString();

        }





            public void EndReload()
        {
            int _needToReload = _maxWeaponAmmo - _currentWeaponAmmo; // сколько надо для перезарядки
            //Уменьшение общего количества патронов
            
            if (_totalAmmo > _needToReload)   // патронов больше чем нужно
            {
                _totalAmmo = _totalAmmo - _needToReload;
                _currentWeaponAmmo = _maxWeaponAmmo;
            }
            else  // если патронов меньше чем нужно но больше 0 - частичная перезарядка
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
            if (!Animator.GetBool("isReloading") && Animator.GetBool("isGround") && (!Animator.GetBool("isDead")))
            {
                Animator.SetBool("isSight", true);
                FirstPersonController.isSight = true;
                Ui_Control.Scope_On();
                CinemachineCameraTarget.transform.localPosition = SightPoint.localPosition;
            }                        
        }

        public void CameraSightOn()
        {
            if (Animator.GetBool("isSight"))
            {
                if (Camera.m_Lens.FieldOfView > 30)
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
                if (Camera.m_Lens.FieldOfView < 65)
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
            //Debug.Log("Выключить прицеливание");
            CinemachineCameraTarget.transform.localPosition = CameraPoint.localPosition;
        }

    }
}
