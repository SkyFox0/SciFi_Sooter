using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using Unity.Mathematics;

namespace StarterAssets
{
    public class My_Weapon_Controller : MonoBehaviour
    {
        public Animator Animator;         
        public FirstPersonController FirstPersonController;
        public Ui_Control Ui_Control;
        private float LastShoot;        

        [Header("Cinemachine")]
        public PlayerCameraController PlayerCameraController;

        //--------------------------------------

        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        public float _cameraspeed;
        public Transform CameraTargetPosition;
        public Transform IdleCameraPoint;
        public Transform GuardIdleCameraPoint;
        public Transform GuardMoveCameraPoint;
        public Transform GuardRunCameraPoint;

        public Transform SightPoint;
        public Transform SightPointShoot;
        public CinemachineVirtualCamera Camera;
        public float _sigthFocus = 30;
        public float _normalFocus = 65;

        [Header("UI")]
        public Image _imageX;
        public Image _image0;

        //--------------------------------------

        [Header("Shoot")]
        public ShootComponent ShootComponent;
        public EGA_DemoLasers EGA_DemoLasers;
        public float _shootSpeed;

        [Header("Sounds")]
        public AudioSource PlaySound;
        public AudioSource Shoot;
        public AudioClip Reload;
        public AudioClip Empty;
        public AudioClip Hit;
        public AudioClip GetAmmo;
        public AudioClip GetGrenades;
        public AudioClip NoAmmo1;
        public AudioClip NoAmmo2;
        public AudioClip NoGrenades1;
        public AudioClip NoGrenades2;
        public AudioClip AmmoFull1;
        public AudioClip AmmoFull2;
        public AudioClip NeedToReload;        
        private bool NoAmmo;
        private bool NoGrenade;

        [Header("Switch Weapon")]
        public bool isRifle;
        public bool isGrenade;
        public bool isLight;
        public Light WeaponLight;

        [Header("Ammo")]
        public int _totalAmmo = 195;
        public int _maxWeaponAmmo = 10;
        public int _currentWeaponAmmo = 10;
        public int _maxAmmo = 200;
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
        public int _maxGrenades = 15;
        
        public GameObject EMPGrenade;
        public GameObject EXPLGrenade;

        private GameObject Instante;
        public Transform ThrowPoint;
        public GameObject CameraDirection;
        public TMP_Text GrenadeText;
        public Image GrenadeBar;
        public GameObject AllScore;

        private void Start()
        {
            _cameraspeed = 10f;
            CameraTargetPosition.position = IdleCameraPoint.position;
            _imageX.enabled = true;
            _image0.enabled = false;
            isRifle = true;
            isGrenade = false;
            GrenadeText.text = _totalEMPGrenades.ToString();
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
            ArmedOnOff();
        }

        private void FixedUpdate()
        {   //������������
            CameraSightOn();
            CameraSightOff();
            Camerashooting();

            if (!Animator.GetBool("isSight") && Animator.GetBool("isArmed"))
            {
                if (CameraTargetPosition.position != IdleCameraPoint.position)
                    //if (CinemachineCameraTarget.transform.position != IdleCameraPoint.position)
                {
                    CinemachineCameraTarget.transform.position = IdleCameraPoint.position;
                    CameraTargetPosition.position = IdleCameraPoint.position;
                }
            }

            if (!Animator.GetBool("isArmed"))
            {
                if (Animator.GetBool("isMove"))
                {
                    //CinemachineCameraTarget.transform.position = GuardMoveCameraPoint.position;
                    CameraTargetPosition.position = GuardMoveCameraPoint.position;
                }
                else
                {
                    //CinemachineCameraTarget.transform.position = GuardIdleCameraPoint.position;
                    CameraTargetPosition.position = GuardIdleCameraPoint.position;
                }

                if (Animator.GetBool("isRun"))
                {
                    //CinemachineCameraTarget.transform.position = GuardRunCameraPoint.position;
                    CameraTargetPosition.position = GuardRunCameraPoint.position;
                }
            }

            if (CinemachineCameraTarget.transform.position != CameraTargetPosition.position)
            {
                //Debug.Log((CinemachineCameraTarget.transform.position - CameraTargetPosition.position).magnitude.ToString());
                if ((CinemachineCameraTarget.transform.position - CameraTargetPosition.position).magnitude > 0.1f)
                {
                    //CinemachineCameraTarget.transform.position = CameraTargetPosition.position;
                    //Debug.Log("������������� ��������� ������" + (CinemachineCameraTarget.transform.position - CameraTargetPosition.position).magnitude.ToString());
                    CinemachineCameraTarget.transform.position = Vector3.Lerp(CinemachineCameraTarget.transform.position, CameraTargetPosition.position, _cameraspeed * Time.deltaTime);
                }
                else
                {
                    CinemachineCameraTarget.transform.position = CameraTargetPosition.position;
                }
                
            }



        }

        public void OnShoot(InputValue value)  // ������ ������ ��������
        {
            //Debug.Log("������ ������ ��������");
            if (Animator.GetBool("isArmed"))
            {

                if (!Animator.GetBool("isDead") && !Animator.GetBool("isRun"))
                {
                    if (_currentWeaponAmmo > 0)
                    {
                        if ((Time.time - LastShoot > _shootSpeed) && (!Animator.GetBool("isReloading")))  //_shootSpeed = 0.5f; && (_currentWeaponAmmo > 0) 
                        {
                            Animator.SetTrigger("Shoot");
                            ShootComponent.Shoot();
                            EGA_DemoLasers.Shoot();

                            Shoot.Play();  // ���� ��������
                                           //PlaySound.Play();

                            LastShoot = Time.time;
                            _currentWeaponAmmo -= 1;
                            _ammoText.text = _currentWeaponAmmo.ToString();
                            Animator.SetBool("isNeedToReload", true);
                        }
                        else
                        {
                            if (!Animator.GetBool("isReloading"))
                            {
                                PlaySound.clip = Empty; //������ �� ������ � ��������
                                PlaySound.Play();
                            }
                        }
                    }
                    else
                    {
                        if (!PlaySound.isPlaying)
                        {
                            PlaySound.PlayOneShot(NeedToReload);
                        }
                    }
                }
            } 
            else
            {
                Animator.SetBool("isArmed", true);
                AllScore.SetActive(true);
                _imageX.enabled = true;
                _image0.enabled = false;
                CinemachineCameraTarget.transform.position = IdleCameraPoint.position;
                CameraTargetPosition.position = IdleCameraPoint.position;

            }
        }

        public void OnShootOff(InputValue value)  // �������� ������ ��������
        {
            //Debug.Log("�������� ������ ��������");
        }

        public void OnReload(InputValue value)
        {
            if (Animator.GetBool("isArmed"))
            {

                if ((_totalAmmo > 0) && Animator.GetBool("isNeedToReload") && !Animator.GetBool("isReloading")
                && Animator.GetBool("isGround") && (!Animator.GetBool("isDead") && !Animator.GetBool("isRun")))
                {

                    //Debug.Log("�����������");
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
                        //���� ���������� ��������
                        //PlaySound.clip = Empty; //������� �����������
                        //PlaySound.Play();
                        NoAmmo = !NoAmmo;
                        if (NoAmmo)
                        {
                            PlaySound.PlayOneShot(NoAmmo1);  //������� �����������
                        }
                        else
                        {
                            PlaySound.PlayOneShot(NoAmmo2);  //������� �����������
                        }

                    }
                }
            }            
        }

        public void OnReloadHold(InputValue value)  // ������ ������!
        {
            /*if (!reloadHold)
            {
                reloadHold = true;
                Invoke("ReloadHoldOff", 0.4f);
            }*/
            ArmedOnOff();



        }


        public void ArmedOnOff()
        {
            if (Animator.GetBool("isArmed"))
            {
                Animator.SetBool("isArmed", false);
                AllScore.SetActive(false);
                if (!Animator.GetBool("isReloading"))
                {
                    CinemachineCameraTarget.transform.position = GuardIdleCameraPoint.position;
                    CameraTargetPosition.position = GuardIdleCameraPoint.position;
                    _imageX.enabled = false;
                    _image0.enabled = true;
                }
            }
            else
            {
                Animator.SetBool("isArmed", true);
                AllScore.SetActive(true);
                CinemachineCameraTarget.transform.position = IdleCameraPoint.position;
                CameraTargetPosition.position = IdleCameraPoint.position;
                _imageX.enabled = true;
                _image0.enabled = false;
            }
        }



        public void OnSelectWeapon(InputValue value)
        {
            Debug.Log("Scroll!");
        }


        public void AddAmmo(int _ammo)
        {
            // ����� ������ �������
            
            if ((_totalAmmo + _ammo) < _maxAmmo)  // ���� ���� �����
            {
                _totalAmmo += _ammo;
                PlaySound.PlayOneShot(GetAmmo);
            }
            else
            {
                if (!PlaySound.isPlaying)
                {
                    PlaySound.PlayOneShot(AmmoFull1);                    
                }
                _totalAmmo = _maxAmmo;
            }
            
            Total_Ammo_Text.text = _totalAmmo.ToString();
        }
        public void AddAmmoFull(int i)  //1 - ������ �������, 2 - ������ ������
        {
            // ����� ������ �������  � ������ ������
            if (!PlaySound.isPlaying)
            {
                if (i == 1)
                {
                    PlaySound.PlayOneShot(AmmoFull1);                
                }
                else
                {
                    PlaySound.PlayOneShot(AmmoFull2);
                }                
            }            
        }

        //  -----------�������-------------
        public void AddEMPGrenades(int _grenades)
        {
            // ����� ������ �������
            if ((_totalEMPGrenades + _grenades) < _maxGrenades)
            {
                PlaySound.PlayOneShot(GetGrenades);
                _totalEMPGrenades += _grenades;                
            }
            else
            {
                if (!PlaySound.isPlaying)
                {
                    PlaySound.PlayOneShot(AmmoFull1);                    
                }
                _totalEMPGrenades = _maxGrenades;
            }
            GrenadeText.text = _totalEMPGrenades.ToString();



        }
        /*public void AddEMPGrenadesFull()
        {
            // ����� ������ �������  � ������ ������
            if (!PlaySound.isPlaying)
            {
                PlaySound.PlayOneShot(AmmoFull2);
            }
        }*/

        /*public void AddEXPLGrenades(int _grenades)
        {
            // ����� ������ �������
            PlaySound.PlayOneShot(GetGrenades);
            _totalEXPLGrenades += _grenades;
        }*/

        public void OnGrenade(InputValue value)
        {
            if (_totalEMPGrenades > 0)
            {            
                if (!_isThrowing)
                {
                    //������ ������� ������� ������ ������� �������
                    _forceTimer = Time.time;
                    _isThrowing = true;
                }                
            }
            else
            {                
                 NoGrenade = !NoGrenade;
                if (NoGrenade)
                {
                    PlaySound.PlayOneShot(NoGrenades1);  // ������� �����������
                }
                else
                {
                    PlaySound.PlayOneShot(NoGrenades2);  // ������� �����������
                }                   
            }
        }

        public void OnGrenadeOff(InputValue value)
        {
            if (_totalEMPGrenades > 0)
            {
                if (_isThrowing)
                {

                    if (Animator.GetBool("isDead"))
                    {
                        _forceTimer = 0.1f;
                        Debug.Log("������ �� ������");
                    }
                    else
                    {
                        //��������� ������� ������� ������ ������� �������
                        _forceTimer = Time.time - _forceTimer;
                        Debug.Log("����� ������� ������" + _forceTimer.ToString());
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
            if (Animator.GetBool("isArmed") && !Animator.GetBool("isSight") && Animator.GetBool("isGround") && !Animator.GetBool("isDead") && !Animator.GetBool("isRun")) // !Animator.GetBool("isMove") && 
            {
                // ����������� ������� � ������ - ��������...
                //isRifle = false;  // ������ ��������
                //isGrenade = true;  // ������� �������
                if (_totalEMPGrenades > 0 && !Animator.GetBool("isThrowing"))   // ���� ������� ����
                {
                    // ������ �������
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
                    // ���� �������
                    PlaySound.clip = Empty; // �������
                    PlaySound.Play();
                    
                }
            }            
            _isThrowing = false;
        }



        public void EndReload()
        {
            int _needToReload = _maxWeaponAmmo - _currentWeaponAmmo; // ������� ���� ��� �����������
            //���������� ������ ���������� ��������
            
            if (_totalAmmo > _needToReload)   // �������� ������ ��� �����
            {
                _totalAmmo = _totalAmmo - _needToReload;
                _currentWeaponAmmo = _maxWeaponAmmo;
            }
            else  // ���� �������� ������ ��� ����� �� ������ 0 - ��������� �����������
            {                
                _currentWeaponAmmo += _totalAmmo;
                _totalAmmo = 0; // _totalAmmo - (_maxWeaponAmmo - _currentWeaponAmmo);
            }
            
            
            _ammoText.text = _currentWeaponAmmo.ToString();
            Total_Ammo_Text.text = _totalAmmo.ToString();
            //-----
            Animator.SetBool("isReloading", false);
            Animator.SetBool("isNeedToReload", false);

            // ���� ���� ������ ������
            if (!Animator.GetBool("isArmed"))
            {
                CinemachineCameraTarget.transform.position = GuardIdleCameraPoint.position;
                CameraTargetPosition.position = GuardIdleCameraPoint.position;
                _imageX.enabled = false;
                _image0.enabled = true;
            }
        }

        public void OnSight(InputValue value)
        {
            if (Animator.GetBool("isArmed") && !Animator.GetBool("isReloading") && Animator.GetBool("isGround") && (!Animator.GetBool("isDead") && !Animator.GetBool("isRun")))
            {
                Animator.SetBool("isSight", true);
                FirstPersonController.isSight = true;
                Ui_Control.Scope_On();

                // ?????????????????????
                
                CinemachineCameraTarget.transform.position = SightPoint.position;
                CameraTargetPosition.position = SightPoint.position;
            }                        
        }

        public void Sighting()
        {
            CinemachineCameraTarget.transform.position = SightPoint.position;
            CameraTargetPosition.position = SightPoint.position;
        }

        public void Shooting()
        {
            CinemachineCameraTarget.transform.position = SightPointShoot.position;
            CameraTargetPosition.position = SightPointShoot.position;

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
            //Debug.Log("��������� ������������");
            //CinemachineCameraTarget.transform.localPosition = CameraPoint.localPosition;

            //????????????????????????

            //CinemachineCameraTarget.transform.position = IdleCameraPoint.position;
            CameraTargetPosition.position = IdleCameraPoint.position;
        }
        public void OnLight(InputValue value)
        {
            if (!isLight)
            {
                isLight = true;
                WeaponLight.enabled = true;
            }
            else
            {
                isLight = false;
                WeaponLight.enabled = false;
            }
        }
    }
 
    
}
