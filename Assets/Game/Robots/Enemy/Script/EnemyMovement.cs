using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.Rendering;


public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator EnemyAnimator;
    public SoundController SoundController;
    public GameObject Player;
    
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
    

    private GameObject Instance;
    public EGA_Laser LaserScript;

    [Header("StateMashine")]
    public Transform Enemy;
    public enum StateMashine
    {
        Stop,
        Move,        
        MovetoSide,
        Search,
        Rotate,
        Shoot,
        Reload,
        Damage,
        Dead
    };


    [Header("Move")]
    public float _enemySpeed = 2f;  // C������� �����
    public float _enemyRotationSpeed = 2.0f; // �������� �������� �����
    public float _stopDistans; // ����������� ���������� �� ������
    public bool _isMove;
    public bool _isMoveToTheSide;
    public bool _isRotate;
    public bool _isRotate_L;
    public bool _isRotate_R;
    public bool _isShoot;
    public bool _isReloading;
    public bool _isDamage;
    public bool _isDead;
    public Vector3 _rotateDirection;
    public Quaternion targetRotation;
    public Vector3 MoveToTheSidePoint;
    public GameObject Point;
    public float _timerMoveToTheSide;
    
    //private GameObject Instance;

    [Header("Search")]
    public float _timer;
    //public Transform SearchPoint;
    //public Transform SearchDirection;
    public Vector3 _direction;
    public Quaternion Direction;
    public Plane PlaneDirection;
    public Plane goalLine1;

    [Header("Fire")]
    public Transform FirePoint;
    public Transform RiflePoint;
    public float _fireDistans;
    public float _distance;  // ���������� �� ������
    public AnimationCurve _maxBulletSpread;
    public float _bulletSpread;
    public Vector3 _spreadVector;
    public Vector3 _rifleDirection; 
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
        _isMoveToTheSide = false;
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
        _stopDistans = Random.Range(3f, 5f);
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

        Debug.DrawRay(SearchPoint + Enemy.transform.right * 0.5f, Enemy.transform.right * 5, Color.green);
        Debug.DrawRay(SearchPoint + Enemy.transform.right * -0.5f, Enemy.transform.right * -5, Color.green);

        _timer = _timer + Time.deltaTime;

        if (_isReloading)
        {
            Stop();
        }

        if (!_isDead && !_isDamage)
        {
            //_timer = _timer + Time.deltaTime;
            if ((_timer > _searchTime) && _isMove && !_isMoveToTheSide)
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

            if (_isShoot | _isMove | _isRotate | _isReloading | _isMoveToTheSide)                
            {
                if (_isDamage)
                {
                    _isMove = false;
                    _isMoveToTheSide = false;
                    _isRotate = false;
                    _isShoot = false;
                    _isReloading = false;
                    Stop();
                }
            }
        }

        if (_isMoveToTheSide)  // ����������� ������ ��������� ����
        {
            MoveToTheSide();
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

                if (!_isDamage && !_isMoveToTheSide && !_isDead)
                {
                    if (_distance < _fireDistans)
                    {
                        Stop();
                        //Rotate();
                        //Direction = Enemy.transform.rotation;
                        //Direction = _direction.rotation;

                        // ==���� �� ����!==
                        //Direction.eulerAngles = SearchPoint;  

                        _isRotate = true;
                        _isShoot = true;

                        if (_ammo > 0)
                        {
                            Invoke("Shoot2", _shootTime);
                        }
                        else
                        {
                            Stop();
                            Reloading();
                        }
                    }
                }
                    
            }
            else
            {
                // ���� �� ����� ������
                //Debug.Log("�� ���� ������!");
                if (_distance < _stopDistans)  // ���� ����� ������� ������
                {
                    _isRotate = true;
                }
                else
                { Move();}
                
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
        Invoke("EndReloading", 3f); // _moveTime);  
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
        _isMove = false;
        NavMeshAgent.speed = 0f;
        EnemyAnimator.SetFloat("y", 0f);
        EnemyAnimator.SetFloat("x", 0f);
        EnemyAnimator.SetBool("isMove", false);
        
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
                    //Debug.Log("������� �������");
                }
                else
                {
                    EnemyAnimator.SetFloat("x", -1f);
                    //Debug.Log("������� ������");
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
            
            //Debug.Log("���� �� ������:  " + Quaternion.Angle(targetRotation, transform.rotation).ToString());
        }
        
    }

    public bool CheckDirectionToTheSide() // �������=1, ������ =-1
    {
        float LeftDistance = 0f;
        float RightDistance = 0f;
        try
        { 
            Physics.Raycast(SearchPoint + Enemy.transform.right * 0.5f, Enemy.transform.right + new Vector3(0f, 0.5f, 0f), out var hitInfoRight);
            Debug.Log("��������� ������� " + hitInfoRight.transform.name.ToString());
            RightDistance = hitInfoRight.distance;

        }
        catch 
        {
            Debug.Log("��������� ������� �� ������");
            RightDistance = 10f;
        }
        try
        {
            Physics.Raycast(SearchPoint + Enemy.transform.right * -0.5f, Enemy.transform.right * -1 + new Vector3(0f, 0.5f, 0f), out var hitInfoLeft);
            Debug.Log("��������� ����� " + hitInfoLeft.transform.name.ToString());
            LeftDistance = hitInfoLeft.distance;
        }
        catch 
        {
            Debug.Log("��������� ����� �� ������");
            LeftDistance = 10f;
        }

        if (RightDistance > 9f && LeftDistance > 9f)
        {
            float _random = Random.Range(-1f, 1f);
            Debug.Log("������ �����: " + _random.ToString());
            if (_random > 0f)
            {
                LeftDistance = 0f;
            }
            else
            {
                RightDistance = 0f;
            }
        }

        if (RightDistance >= LeftDistance)
        {
            if (RightDistance > 2f)
            {
                MoveToTheSidePoint = Enemy.transform.position + Enemy.transform.right * 2;
                Debug.Log("��� ������� � �����: " + MoveToTheSidePoint.ToString() + " " + RightDistance.ToString());
                //Instance = Instantiate(Point, MoveToTheSidePoint, Point.transform.rotation);
                StepsToTheSide(1, 2);
                return true;
            }
            else
            {
                Debug.Log("������� = " + RightDistance.ToString());

            }
        }
        else
        {
            if (LeftDistance > 2f)
            {
                MoveToTheSidePoint = Enemy.transform.position + Enemy.transform.right * -2;
                Debug.Log("��� ������ � �����: " + MoveToTheSidePoint.ToString() + " " + LeftDistance.ToString());
                //Instance = Instantiate(Point, MoveToTheSidePoint, Point.transform.rotation);
                StepsToTheSide(-1, 2);
                return true;
            }
            else
            {
                Debug.Log("������ = " + LeftDistance.ToString());

            }
        }

        return false;
    }

    public void StepsToTheSide(int Direction, int i) // Direction �������=1, ������ =-1 // i - ���������� �������� ������
    {
        // ��������� �������-���������
        if (Direction > 0)
        {
            Debug.Log("����� ������");            
        }
        else
        {
            Debug.Log("����� �����");           
        }
        // ��������� ���������!!!
        NavMeshAgent.speed = 0f;

        _isMove = true;
        _isMoveToTheSide = true;
        _isRotate = false;
        _isReloading = false;
        _isShoot = false;

        EnemyAnimator.SetBool("isReloading", false);
        EnemyAnimator.SetBool("isMove", true);
        EnemyAnimator.SetFloat("y", 0f); // �������� ������ ���������
        EnemyAnimator.SetFloat("x", Direction); // �������� ���� ��������        

        //nvoke("Move", _moveTime);
    }

    public void MoveToTheSide()  // ���������� �������-���������
    {   // ���� ����� ���������� �� ���������� ���� ����
        _timerMoveToTheSide += Time.deltaTime;

        if ((Enemy.transform.position - MoveToTheSidePoint).magnitude > 0.5f && _timerMoveToTheSide < 2f)
        {
            Debug.Log("���������� �� ���� " + (Enemy.transform.position - MoveToTheSidePoint).magnitude.ToString());
            Enemy.transform.position = Vector3.Lerp(Enemy.transform.position, MoveToTheSidePoint, 1.5f * Time.deltaTime);
        }
        else
        {
            MoveToTheSideOff();
        }
        /*else
        {
            // ���� ����� ���������� ���������� ����!
            Invoke("MoveToTheSideOff", 2f);

        }*/
        // ���� ����� ���������� ���������� ����!
        //MoveToTheSideOff();

    }

    public void MoveToTheSideOff()
    {
        // ���� ����� ���������� ���������� ����!
        _timer = _searchTime + 1;
        _isMove = true;
        _isMoveToTheSide = false;
        EnemyAnimator.SetFloat("x", 0f); // ������� �� ����� ��������
        //Move();
    }

    

    public void Move()
    {
        _isMove = true;
        _isMoveToTheSide = false;
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
        _isMoveToTheSide = false;
        _isRotate = false;
        _isShoot = false;
        _isReloading = false;
        
        Invoke("DamageOff", 1f);
    }

    public void DamageOff()
    {
        _isDamage = false;        
        if (CheckDirectionToTheSide())
        {
            _timerMoveToTheSide = 0f;
        }
        else
        {
            Debug.Log("���� ��� ������� ��������� �� ���������!");
            Invoke("Move", _moveTime); 
        }
        
    }

    public void DeadShoot()
    {
        if (_ammo > 0)
        {
            _isRotate = false;
            _rifleDirection = FirePoint.position - RiflePoint.position;
            _shootDerection = Quaternion.LookRotation(_rifleDirection);

            _ammo -= 1;
//            EGA_EnemyLasers.ShootEnemy(Prefab, SearchPoint, _shootDerection);
            EGA_EnemyLasers.ShootEnemy(Prefab, FirePoint.position, _shootDerection);
            ShootSound.Play();
            Debug.Log("������� ������ ����!");

            if (Physics.Raycast(FirePoint.position, _rifleDirection, out var hitInfo))
            {
                //Debug.DrawRay(SearchPoint, _direction * 15, Color.red);
                if (hitInfo.collider.gameObject.name == "Player")
                {
                    // ����� �� ������
                    if (hitInfo.collider.TryGetComponent(out PlayerHealthComponentNew healthComponent))
                    {
                        try
                        {
                            healthComponent.TakeDamage(10);
                        }
                        catch { }
                    }
                }
            }
        }
    }

    public void Shoot2()
    {
        if (!_isDamage && !_isMoveToTheSide && !_isDead)
        {
            if (_ammo > 0)
            {
                _isRotate = false;
                _rifleDirection = FirePoint.position - RiflePoint.position;
                _shootDerection = Quaternion.LookRotation(_rifleDirection);

                _ammo -= 1;
                //            EGA_EnemyLasers.ShootEnemy(Prefab, SearchPoint, _shootDerection);
                EGA_EnemyLasers.ShootEnemy(Prefab, FirePoint.position, _shootDerection);
                ShootSound.Play();
                //Debug.Log("������� ������ ����!");

                if (Physics.Raycast(FirePoint.position, _rifleDirection, out var hitInfo))
                {
                    //Debug.DrawRay(SearchPoint, _direction * 15, Color.red);
                    if (hitInfo.collider.gameObject.name == "Player")
                    {
                        // ����� �� ������
                        if (hitInfo.collider.TryGetComponent(out PlayerHealthComponentNew healthComponent))
                        {
                            try
                            {
                                healthComponent.TakeDamage(10);
                            }
                            catch { }
                        }
                    }
                }
                if (!_isDead)
                {
                    if (_ammo <= 0)
                    {
                        Stop();
                        Reloading();
                    }
                    else
                    {
                        Invoke("Move", _moveTime);
                    }
                }
            }
        }
    }

    /*public void Shoot()
    {
        if (_ammo > 0)
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
                else
                {
                    Invoke("Move", _moveTime);
                }
            }

        }

    }*/

    //public void DestroyLaser()
    // {
    //    LaserScript.DisablePrepare();        
    // }

}
