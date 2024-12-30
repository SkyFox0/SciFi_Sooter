using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;


public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator EnemyAnimator;
    public SoundController SoundController;
    public GameObject Player;
    public Transform Enemy;
    public Vector3 SearchPoint;
    //public Transform SearchPoint;
    public NavMeshAgent NavMeshAgent;
    public EGA_EnemyLasers EGA_EnemyLasers;
    public GameObject Prefab;
    public AudioSource ShootSound;
    public float _moveTime;      // ����� ����� �������� �� ������ ��������
    public float _searchTime;    // ����� �� ����� �����������
    public float _shootTime;     // ����� ������������
    public float _enemyChange;   // ����� �� ���������� ��������� ������������� ������
    public Transform FirePoint;

    private GameObject Instance;
    public EGA_Laser LaserScript;


    [Header("Move")]
    public float _enemySpeed = 2f;  // C������� �����
    public float _enemyRotationSpeed = 2.0f; // �������� �������� �����
    public float _stopDistans; // ����������� ���������� �� ������
    public bool _isMove;
    public bool _isRotate;
    public bool _isRotate_L;
    public bool _isRotate_R;
    public bool _isShoot;
    public bool _isReloading;
    public bool _isDamage;
    public bool _isDead;
    public Vector3 _rotateDirection;
    public Quaternion targetRotation;


    [Header("Search")]
    public float _timer;
    //public Transform SearchPoint;
    //public Transform SearchDirection;
    public Vector3 _direction;
    public Quaternion Direction;
    public Plane PlaneDirection;
    public Plane goalLine1;

    [Header("Fire")]
    public float _fireDistans;
    public float _distance;
    public AnimationCurve _maxBulletSpread;
    public float _bulletSpread;
    public Vector3 _spreadVector;

    public Vector3 _targetDirection;
    public Quaternion _shootDerection;

    [Header("Weapon")]
    public int _ammo = 3;
    public int _maxAmmo = 5;

    void Start()
    {   // � ������ ������ �� ������� ������
        //Player = GameObject.FindGameObjectWithTag("Player").transform;
        //NavMeshAgent.speed = _enemySpeed;
        //NavMeshAgent.stoppingDistance = _stopDistans;
        _isMove = true;
        _isDead = false;
        _isReloading = false;
        _timer = 0f;

        Player = GameObject.Find("Player");
        EGA_EnemyLasers = GetComponent<EGA_EnemyLasers>();
        EnemyAnimator = GetComponentInChildren<Animator>();
        SoundController = GetComponentInChildren<SoundController>();
        
        //_enemyChange = 15f;  // ����� �� ����� ������������� �����        
        //_fireDistans = 30f;  //������������ ��������� ��������
        _ammo = Random.Range(1, _maxAmmo);
        EnemyChange();
        NavMeshAgent.speed = _enemySpeed;
        EnemyAnimator.SetFloat("y", 1f);
        EnemyAnimator.SetFloat("x", 0f);
    }

    public void EnemyChange()
    {
        _moveTime = Random.Range(0.5f, 1f);    // ����� ����� ������� ���� �������� ��������
        _searchTime = Random.Range(0.5f, 1f);  // ����� ����� ������� ���� ��������� ������� ������ � �������� ���������� �����
        _shootTime = Random.Range(1.5f, 2.5f);    // ����� ����� ������� ���� �������� �� ������
        _enemySpeed = Random.Range(2f, 4f);
        _enemyRotationSpeed = Random.Range(2f, 3f);
        _stopDistans = Random.Range(4f, 8f);
        //NavMeshAgent.speed = _enemySpeed;
        NavMeshAgent.stoppingDistance = _stopDistans;
        // ������� �������� ������ ���� �� 0,5 �� 1,5
        //_bulletSpread = 2f / ((_shootTime + 1f) * (_shootTime + 1f));  //����� ����
        _bulletSpread = _maxBulletSpread.Evaluate(_shootTime / 3f);
        _enemyChange = Random.Range(10f, 15f);  // ����� �� ����� ������������� �����        
        _fireDistans = Random.Range(25f, 35f);  //������������ ��������� ��������
    }

    // Update is called once per frame
    void Update()
    {
        _timer = _timer + Time.deltaTime;
        if (!_isDead && !_isDamage)
        {
            //_timer = _timer + Time.deltaTime;
            if ((_timer > _searchTime) && _isMove)
            {
                NavMeshAgent.SetDestination(Player.transform.position);
                Search();
            }
            
            if (_isShoot)
            {
                SearchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
                //Debug.DrawRay(SearchPoint, _direction * 10, Color.red);
                Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
            }

            if (_isRotate)
            {
                Rotate();
                //Enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }


            SearchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
            Debug.DrawRay(SearchPoint, Enemy.transform.forward * 5, Color.green);
            Debug.DrawRay(SearchPoint, _direction * 5, Color.yellow);
            //NavMeshAgent.SetDestination(Player.position);
            //Debug.DrawRay(Enemy.transform.position, Enemy.transform.forward * 30, Color.yellow);
            //Debug.DrawRay(Enemy.transform.position, direction * 30, Color.green);

            if (_isShoot | _isMove | _isRotate | _isReloading )                
            {
                if (_isDamage)
                {
                    _isMove = false;
                    _isRotate = false;
                    _isShoot = false;
                    _isReloading = false;
                    Stop();
                }
            }
        }

    }

    public void Search()
    {
        //������� ��������� ���
        _direction = Player.transform.position - Enemy.transform.position;
        _distance = _direction.magnitude;
        //Debug.DrawRay(Enemy.transform.position, Enemy.transform.forward * 30, Color.yellow);
        //Debug.DrawRay(Enemy.transform.position, direction * 30, Color.green);
        if (Physics.Raycast(SearchPoint, _direction, out var hitInfo))
        {
            //Debug.Log("Hit! Object = " + hitInfo.collider.name);
            //Debug.Log(_direction.ToString());
            if (hitInfo.collider.gameObject.name == "Player")  //if (hitInfo.collider.name == "Player")
            {
                //Debug.Log("���� ������!");

                if (_distance < _fireDistans)
                {
                    Stop();

                    //Rotate();
                    //Direction = Enemy.transform.rotation;
                    //Direction = _direction.rotation;

                    // ==���� �� ����!==
                    //Direction.eulerAngles = SearchPoint;  

                    _isRotate = true;

                    //Invoke("Rotate", _shootSpeed/2f);

                    _isShoot = true;

                    if (_ammo > 0)
                    {
                        Invoke("Shoot", _shootTime);
                    }
                    else
                    {
                        Reloading();
                    }
                }
            }
            else
            {
                _timer = 0f;
            }
        }
    }

    public void Reloading()
    {
        //Stop();
        //_isMove = false;
        _isReloading = true;
        SoundController.Reload();

        //EnemyAnimator.SetBool("isMove", false);
        EnemyAnimator.SetTrigger("Reload");
        EnemyAnimator.SetBool("isReloading", true);
        //_ammo = _maxAmmo;
        //Invoke("Move", _moveTime); // _moveTime);  
        Invoke("EndReloading", 1f); // _moveTime);  
    }

    public void EndReloading()
    {
        if (!_isDamage)
        {
                _ammo = _maxAmmo;
                Invoke("Move", _moveTime); // _moveTime);  
        }

    }

        public void Stop()
    {
        NavMeshAgent.speed = 0f;
        EnemyAnimator.SetFloat("y", 0f);
        EnemyAnimator.SetFloat("x", 0f);
        EnemyAnimator.SetBool("isMove", false);
        _isMove = false;
        //Debug.Log("����������� ��� ��������");
    }

    public void Rotate()
    {
        //������� � ������� ������
        EnemyAnimator.SetFloat("y", 0f);
        //�������� ������� 
        //EnemyAnimator.SetFloat("x", -1);
        // ��������� ����������� �� ������
        _rotateDirection = Player.transform.position - Enemy.transform.position;
        float Angle = Vector3.Angle(_rotateDirection, Enemy.transform.forward);
        _rotateDirection.y = 0; // ���� ������ ������������ ������������ ���
        // ��������� ������� � ����
        targetRotation = Quaternion.LookRotation(_rotateDirection);
        //        Debug.Log(_rotateDirection.normalized.ToString());  
        // ���������, ���� �� ��������� �����������

        PlaneDirection = new Plane(transform.right, transform.position);
        
        if (Quaternion.Angle(targetRotation, transform.rotation) > 0.1f)
        {
            if (Quaternion.Angle(targetRotation, transform.rotation) > 2f)  // �������� �������� ��������
            {
                if (PlaneDirection.GetSide(Player.transform.position))  // ����� ��������� ������ ��� ����� �� ���������?
                {
                    EnemyAnimator.SetFloat("x", 1f);
                    Debug.Log("������� �������");
                }
                else
                {
                    EnemyAnimator.SetFloat("x", -1f);
                    Debug.Log("������� ������");
                }
            }
            else
            {
                EnemyAnimator.SetFloat("x", 0f);
            }


            //Debug.Log("���� �������� > 0f:  " + Angle.ToString());
            //Debug.Log("����� ��������� �� �������:  " + _rotateDirection.ToString());

            //Quaternion RotateVector = targetRotation - transform.rotation;
            //Debug.Log("����������� � ������� ������ �� ������:  " + targetRotation.ToString());

            // ������ ������������ ����� � ������� ������
            Enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _enemyRotationSpeed);
            
            Debug.Log("���� �� ������:  " + Quaternion.Angle(targetRotation, transform.rotation).ToString());
        }
        
    }

    public void Shoot()
    {
        _isRotate = false;
        //_shoot = true;
        //Debug.Log("�������!");
        //_ammo -= 1;
        _targetDirection = Player.transform.position - Enemy.transform.position;

        // ����� ���������� ������
        // � ������ ��� ��������
        _spreadVector.y = Random.Range((_bulletSpread * -1), _bulletSpread) / 3f;  // ���������� �� ��������� ������ � 3 ����
        _spreadVector.z = Random.Range((_bulletSpread * -1), _bulletSpread);  // ���������� �� �����������

        _targetDirection.y += _spreadVector.y;  // ���������� �� ���������
        _targetDirection.z += _spreadVector.z;  // ���������� �� �����������               

        // �������� �� ���� �� ������ :)
        if (_isDead | _isDamage)
        {
            // �������� �� ����������� ������ ������
            _targetDirection = FirePoint.forward;
        }

        // �������������� Vector 3 � Quaternion!!!
        _shootDerection = Quaternion.LookRotation(_targetDirection);
        //Direction = _shootDerection;   //Quaternion.LookRotation(FirePoint.forward);        
        EnemyAnimator.SetTrigger("Shoot");
        _ammo -= 1;
        EGA_EnemyLasers.ShootEnemy(Prefab, SearchPoint, _shootDerection);
        ShootSound.Play();
        //EGA_EnemyLasers.ShootEnemy(Prefab, SearchPoint, Direction);

        //Destroy(Instance);
        //Instance = Instantiate(Prefab, SearchPoint, Direction);
        //Instance.transform.parent = transform;




        //if (Physics.Raycast(SearchPoint, _direction, out var hitInfo))
        if (Physics.Raycast(SearchPoint, _targetDirection, out var hitInfo))
        {
            //Debug.DrawRay(SearchPoint, _direction * 15, Color.red);
            if (hitInfo.collider.gameObject.name == "Player")
            {
                // ����� �� ������
//                Debug.Log("����� �� ������!");
                if (hitInfo.collider.TryGetComponent(out PlayerHealthComponentNew healthComponent))
                {
                    try
                    {
                        //healthComponent.HitSound.Play();
                        healthComponent.TakeDamage(10);
                    }
                    catch { }
                }

            }
            else
            {
                // �������� �� ������
//                Debug.Log("�������� �� ������!");
            }
        }
        if (!_isDead)
        {
            if (_ammo <= 0)
            {
                Stop();
                Reloading();
            }
            Invoke("Move", _moveTime);
        }

        

    }

    //public void DestroyLaser()
    // {
    //    LaserScript.DisablePrepare();        
    // }

    public void Move()
    {
        _isMove = true; 
        _isReloading = false;
        _isShoot = false;

        EnemyAnimator.SetBool("isReloading", false);
        EnemyAnimator.SetBool("isMove", true);
        EnemyAnimator.SetFloat("x", 0f); // ������� �� ����� ��������
        if (_distance > _stopDistans)   
        {
            EnemyAnimator.SetFloat("y", 1f);  // ������� ���, ���� ���� ����
            NavMeshAgent.SetDestination(Player.transform.position);
            NavMeshAgent.speed = _enemySpeed;
        }        

        _timer = 0f;        

    }

    public void Damage()
    {
        Stop();

        _isDamage = true;
        _isMove = false;
        _isRotate = false;
        _isShoot = false;
        _isReloading = false;
        
        Invoke("DamageOff", 1f);
    }

    public void DamageOff()
    {
        _isDamage = false;
        Invoke("Move", _moveTime);
    }

}
