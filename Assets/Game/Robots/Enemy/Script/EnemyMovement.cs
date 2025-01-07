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
    
    
    //public Transform SearchPoint;
    public NavMeshAgent NavMeshAgent;
    public EGA_EnemyLasers EGA_EnemyLasers;
    public GameObject Prefab;
    public AudioSource ShootSound;
    public float _moveTime;      // время после выстрела до начала движения
    public float _searchTime;    // время до смены направления
    public float _shootTime;     // время прицеливания
    public float _enemyChange;   // время до рандомного изменения характеристик игрока
    

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
    public float _enemySpeed = 2f;  // Cкорость врага
    public float _enemyRotationSpeed = 2.0f; // Скорость поворота врага
    public float _stopDistans; // минимальное расстояние до игрока
    public bool _isMove;
    public bool _isMoveToTheSide;
    public bool _isRotate;
    public bool _isRotate_L;
    public bool _isRotate_R;
    public bool _isShoot;
    public bool _isReloading;
    public bool _isDamage;
    public bool _isDead;
    public bool _isEMPShocking;
    public float _empShokingTimer;
    public Vector3 _rotateDirection;
    public Quaternion targetRotation;
    public Vector3 MoveToTheSidePoint;
    public GameObject Point;
    public float _timerMoveToTheSide;
    
    //private GameObject Instance;

    [Header("Search")]
    public float _timer;
    public AudioSource Scan;
    public Vector3 _searchPoint;
    public Transform SearchPoint;
    //public Transform SearchDirection;
    public Vector3 _direction;
    public Quaternion Direction;
    public Plane PlaneDirection;
    public Plane goalLine1;

    [Header("Fire")]
    public Transform FirePoint;
    public Transform RiflePoint;
    public float _fireDistans;
    public float _distance;  // расстояние до игрока
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
    {   // О игроке узнаем из системы спавна
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
        
        //_enemyChange = 15f;  // время до смены характеристик врага        
        //_fireDistans = 30f;  //максимальная дистанция стрельбы
        _ammo = Random.Range(1, _maxAmmo);
        EnemyChange();
        NavMeshAgent.speed = _enemySpeed;
        EnemyAnimator.SetFloat("y", 1f);
        EnemyAnimator.SetFloat("x", 0f);
        _empShokingTimer = -1f;
        _isEMPShocking = false;
    }

    public void EnemyChange()
    {
        _moveTime = Random.Range(0.5f, 1f);    // время через которое враг начинает движение
        _searchTime = Random.Range(1f, 2f);   // время через которое враг обновляет позицию игрока и начинает визуальный поиск
        _shootTime = Random.Range(0.5f, 1f);    // время через которое враг стреляет по игроку
        _enemySpeed = Random.Range(2f, 4f);
        _enemyRotationSpeed = Random.Range(2f, 3f);
        _stopDistans = Random.Range(4f, 6f);
        //NavMeshAgent.speed = _enemySpeed;
        NavMeshAgent.stoppingDistance = _stopDistans;
        // Разброс стрельбы должен быть от 0,5 до 1,5
        //_bulletSpread = 2f / ((_shootTime + 1f) * (_shootTime + 1f));  //разлёт пуль
        _bulletSpread = _maxBulletSpread.Evaluate(_shootTime / 3f);
        _enemyChange = Random.Range(10f, 15f);  // время до смены характеристик врага        
        _fireDistans = Random.Range(25f, 35f);  //максимальная дистанция стрельбы
    }

    // Update is called once per frame
    void Update()
    {
        _searchPoint = SearchPoint.position;
        Debug.DrawRay(_searchPoint + Enemy.transform.right * 0.5f, Enemy.transform.right * 5, Color.green);
        Debug.DrawRay(_searchPoint + Enemy.transform.right * -0.5f, Enemy.transform.right * -5, Color.green);
        Debug.DrawRay(_searchPoint, Enemy.transform.forward * _fireDistans, Color.red);

        _timer = _timer + Time.deltaTime;
        //_searchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
        if (_isReloading)
        {
            Stop();
            _timer = 0;
        }

        if (!_isDead && !_isDamage && !_isEMPShocking)
        {
            //_timer = _timer + Time.deltaTime;
            if ((_timer > _searchTime) && !_isMoveToTheSide) //&& _isMove 
            {
                Debug.Log("Активирован поиск!");
                Scan.Play();
                try
                    { NavMeshAgent.SetDestination(Player.transform.position); }
                catch { }
                //_isShoot = true;
                Search();
                _timer = 0f;
            }

            if (_isShoot)
            {
                //_searchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
                
                Debug.DrawRay(_searchPoint, _direction * _fireDistans, Color.red);
            }

            if (_isRotate)
            {
                _isMove = false;
                _isMoveToTheSide = false;
                NavMeshAgent.speed = 0f;
                //NavMeshAgent.isStopped = true;

                Rotate();
                //Enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }


            //SearchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
            //Debug.DrawRay(SearchPoint, Enemy.transform.forward * 5, Color.green);
            //Debug.DrawRay(SearchPoint, _direction * 5, Color.yellow);
            //NavMeshAgent.SetDestination(Player.position);
            //Debug.DrawRay(Enemy.transform.position, Enemy.transform.forward * 30, Color.yellow);
            //Debug.DrawRay(Enemy.transform.position, direction * 30, Color.green);

            if (_isShoot | _isMove | _isRotate | _isReloading | _isMoveToTheSide)                
            {
                if (_isDamage | _isEMPShocking)
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

        if (_isMoveToTheSide)  // выполняется маневр уклонения вбок
        {
            MoveToTheSide();
        }

        if (_isEMPShocking)
        {
            _empShokingTimer += Time.deltaTime;
            if (_empShokingTimer > 2f)
            {
                _isEMPShocking = false;
                _empShokingTimer = -1;
                if (!_isDead )
                {
                    //Move();
                    Stop();
                    _timer = 0f;
                }                
            }
        }
    }

    public void Search()
    {
        Debug.Log("Ищу игрока!");
        //бросаем поисковый луч
        _direction = Player.transform.position - Enemy.transform.position;
        Debug.Log("Ищу игрока по вектору! " + _direction.ToString());
        _distance = _direction.magnitude;
        Debug.Log("Расстояние до игрока!" + _distance.ToString());
        //Debug.DrawRay(Enemy.transform.position, Enemy.transform.forward * 30, Color.yellow);
        //Debug.DrawRay(Enemy.transform.position, direction * 30, Color.green);
        //_searchPoint = Enemy.transform.position + new Vector3(0f, 1.6f, 0f);
        if (Physics.Raycast(_searchPoint, _direction, out var hitInfo))
        {
            //Debug.Log("Hit! Object = " + hitInfo.collider.name);
            //Debug.Log(_direction.ToString());
            if (hitInfo.collider.gameObject.name == "Player")  //if (hitInfo.collider.name == "Player")
            {
                Debug.Log("Вижу игрока!");


                if (!_isDamage && !_isMoveToTheSide && !_isDead && !_isEMPShocking)
                {
                    if (_distance < _fireDistans) // && _isMove
                    {
                        Stop();
                        //Rotate();
                        //Direction = Enemy.transform.rotation;
                        //Direction = _direction.rotation;

                        // ==Одно из двух!==
                        //Direction.eulerAngles = SearchPoint;  

                        _isRotate = true;

                        if (_ammo > 0)
                        {
                            if (!_isShoot)
                            {
                                _isShoot = true;
                                Invoke("Shoot2", _shootTime);
                            }

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
                // если не видит игрока
                Debug.Log("Не вижу игрока! - " + hitInfo.collider.gameObject.name.ToString());
                
                if (_distance <= _stopDistans)  // если игрок слишком близко
                {
                    _isRotate = true;
                }
                else
                { 
                    Move();
                }
                
                _timer = 0f;
            }
        }
        else
        {
            Debug.Log("Ошибка - луч не брошен!");
            NavMeshAgent.isStopped = false;
            _isRotate = true;
            _timer = _searchTime/2f;
            _isShoot = false;
        }
    }

    public void Reloading()
    {

        //Stop();
        //_isMove = false;
        _isReloading = true;
        _isShoot = false;
        SoundController.Reload();

        //EnemyAnimator.SetBool("isMove", false);
        EnemyAnimator.SetTrigger("Reload");
        EnemyAnimator.SetBool("isReloading", true);
        //_ammo = _maxAmmo;
        //Invoke("Move", _moveTime); // _moveTime);  
        Invoke("EndReloading", 2f); // _moveTime);  
    }

    public void EndReloading()
    {
        if (!_isDamage && !_isEMPShocking)
        {
            _ammo = _maxAmmo;
            _timer = _searchTime / 2f;
            _isReloading = false;
            Stop();
            //Invoke("Move", _moveTime); // _moveTime);  
        }

    }

    public void Stop()
    {
        _isMove = false;
        _isMoveToTheSide = false;
        //_isRotate= false;
        NavMeshAgent.speed = 0f;
        //NavMeshAgent.isStopped = true;
        EnemyAnimator.SetFloat("y", 0f);
        EnemyAnimator.SetFloat("x", 0f);
        EnemyAnimator.SetBool("isMove", false);
        
        //Debug.Log("Остановился для стрельбы");
    }

    public void Rotate()
    {
        //Поворот в сторону игрока
        EnemyAnimator.SetFloat("y", 0f);
        //Включить поворот 
        //EnemyAnimator.SetFloat("x", -1);
        // Вычисляем направление на игрока
        _rotateDirection = Player.transform.position - Enemy.transform.position;
        float Angle = Vector3.Angle(_rotateDirection, Enemy.transform.forward);
        _rotateDirection.y = 0; // Если хотите игнорировать вертикальную ось
        // Вычисляем поворот к цели
        targetRotation = Quaternion.LookRotation(_rotateDirection);
        //        Debug.Log(_rotateDirection.normalized.ToString());  
        // Проверяем, есть ли ненулевое направление

        PlaneDirection = new Plane(transform.right, transform.position);
        
        if (Quaternion.Angle(targetRotation, transform.rotation) > 0.01f)
        {
            if (Quaternion.Angle(targetRotation, transform.rotation) > 5f)  // включить анимацию поворота
            {
                if (PlaneDirection.GetSide(Player.transform.position))  // игрок находится справа или слева от плоскости?
                {
                    EnemyAnimator.SetFloat("x", 1f);
                    //Debug.Log("Поворот направо");
                }
                else
                {
                    EnemyAnimator.SetFloat("x", -1f);
                    //Debug.Log("Поворот налево");
                }
            }
            else
            {
                EnemyAnimator.SetFloat("x", 0f);  // если угол меньше 5 гр. - анимация не нужна
                //_isShoot = true;

                
            }


            //Debug.Log("Угол поворота > 0f:  " + Angle.ToString());
            //Debug.Log("Игрок находится на векторе:  " + _rotateDirection.ToString());

            //Quaternion RotateVector = targetRotation - transform.rotation;
            //Debug.Log("поворачиваю в сторону игрока на вектор:  " + targetRotation.ToString());

            // Плавно поворачиваем врага в сторону игрока
            Enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _enemyRotationSpeed);
            
            //Debug.Log("Угол до игрока:  " + Quaternion.Angle(targetRotation, transform.rotation).ToString());
        }
        //else
        //{
        //    _isRotate = false;
        //    _isMove = true;
        //}
        
    }

    public bool CheckDirectionToTheSide() // направо=1, налево =-1
    {
        float LeftDistance = 0f;
        float RightDistance = 0f;
        try
        { 
            Physics.Raycast(Enemy.transform.position + Enemy.transform.right * 0.25f, Enemy.transform.right + new Vector3(0f, 0.5f, 0f), out var hitInfoRight);
            Debug.Log("Коллайдер спарава " + hitInfoRight.transform.name.ToString());
            RightDistance = hitInfoRight.distance;

        }
        catch 
        {
            Debug.Log("Коллайдер спарава не найден");
            RightDistance = 10f;
        }
        try
        {
            Physics.Raycast(Enemy.transform.position + Enemy.transform.right * -0.25f, Enemy.transform.right * -1 + new Vector3(0f, 0.5f, 0f), out var hitInfoLeft);
            Debug.Log("Коллайдер слева " + hitInfoLeft.transform.name.ToString());
            LeftDistance = hitInfoLeft.distance;
        }
        catch 
        {
            Debug.Log("Коллайдер слева не найден");
            LeftDistance = 10f;
        }

        if (RightDistance > 9f && LeftDistance > 9f)
        {
            float _random = Random.Range(-1f, 1f);
            Debug.Log("Рандом равен: " + _random.ToString());
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
                Debug.Log("Иду направо в точку: " + MoveToTheSidePoint.ToString() + " " + RightDistance.ToString());
                //Instance = Instantiate(Point, MoveToTheSidePoint, Point.transform.rotation);
                StepsToTheSide(1, 2);
                return true;
            }
            else
            {
                Debug.Log("Направо = " + RightDistance.ToString());

            }
        }
        else
        {
            if (LeftDistance > 2f)
            {
                MoveToTheSidePoint = Enemy.transform.position + Enemy.transform.right * -2;                
                Debug.Log("Иду налево в точку: " + MoveToTheSidePoint.ToString() + " " + LeftDistance.ToString());
                //Instance = Instantiate(Point, MoveToTheSidePoint, Point.transform.rotation);
                StepsToTheSide(-1, 2);
                return true;
            }
            else
            {
                Debug.Log("Налево = " + LeftDistance.ToString());

            }
        }

        return false;
    }

    public void StepsToTheSide(int Direction, int i) // Direction направо=1, налево =-1 // i - расстояние бокового отхода
    {
        // включение маневра-уклонения
        if (Direction > 0)
        {
            Debug.Log("Отход вправо");            
        }
        else
        {
            Debug.Log("Отход влево");           
        }
        // Отключить навигацию!!!
        NavMeshAgent.speed = 0f;
        NavMeshAgent.isStopped = true;

        _isMove = true;
        _isMoveToTheSide = true;
        _isRotate = false;
        _isReloading = false;
        _isShoot = false;

        EnemyAnimator.SetBool("isReloading", false);
        EnemyAnimator.SetBool("isMove", true);
        EnemyAnimator.SetFloat("y", 0f); // движение вперед отключено
        EnemyAnimator.SetFloat("x", Direction); // движение вбок включено        

        //nvoke("Move", _moveTime);
    }

    public void MoveToTheSide()  // выполнение маневра-уклонения
    {   // если точка назначения не достигнута идем вбок
        _timerMoveToTheSide += Time.deltaTime;

        if ((Enemy.transform.position - MoveToTheSidePoint).magnitude > 0.5f && _timerMoveToTheSide < 2f)
        {
            Debug.Log("Расстояние до цели " + (Enemy.transform.position - MoveToTheSidePoint).magnitude.ToString());
            Enemy.transform.position = Vector3.Lerp(Enemy.transform.position, MoveToTheSidePoint, 1.5f * Time.deltaTime);
        }
        else
        {
            MoveToTheSideOff();
        }
        /*else
        {
            // если точка назначения достигнута стоп!
            Invoke("MoveToTheSideOff", 2f);

        }*/
        // если точка назначения достигнута стоп!
        //MoveToTheSideOff();

    }

    public void MoveToTheSideOff()
    {
        // если точка назначения достигнута стоп!
        //_timer = _searchTime - 0.5f;
        _timer = _searchTime/2f;
        //_isMove = true;
        _isMoveToTheSide = false;
        EnemyAnimator.SetFloat("x", 0f); // Поворот на месте отключен
        NavMeshAgent.isStopped = false;
        Stop();
        //NavMeshAgent.isStopped = false;
        //Move();
    }

    

    public void Move()
    {
        _isMove = true;
        _isRotate = false;
        _isMoveToTheSide = false;
        _isReloading = false;
        _isShoot = false;
        _isDamage = false;

        EnemyAnimator.SetBool("isReloading", false);
        
        EnemyAnimator.SetFloat("x", 0f); // Поворот на месте отключен
        if (_distance > _stopDistans)   
        {
            EnemyAnimator.SetBool("isMove", true);
            EnemyAnimator.SetFloat("y", 1f);  // включен шаг, если надо идти
            NavMeshAgent.SetDestination(Player.transform.position);
            NavMeshAgent.isStopped = false;
            NavMeshAgent.speed = _enemySpeed;            
        }
        else
        {
            EnemyAnimator.SetBool("isMove", false);
            EnemyAnimator.SetFloat("y", 0f);  // выключен шаг
            NavMeshAgent.SetDestination(Player.transform.position);
            NavMeshAgent.isStopped = false;
            NavMeshAgent.speed = 0f;
            _isRotate = true;
            _isMove = false;
        }

        _timer = 0f;        

    }

    public void Damage()
    {
        if (!_isDead)
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
    }

    public void DamageOff()
    {
        if (!_isDead)
        {
            _isDamage = false;
            if (CheckDirectionToTheSide())
            {
                _timerMoveToTheSide = 0f;
            }
            else
            {
                Debug.Log("Путь для маневра уклонения не обнаружен!");
                //Invoke("Move", _moveTime);
                _timer = 0f;
                Stop();

            }
        }               
    }

    public void EMPDamage()       
    {
        if (!_isDead)
        {
            Stop();

            //NavMeshAgent.speed = 0f;
            //NavMeshAgent.isStopped = true;
            _isDead = false;
            _isMove = false;
            
            _isDamage = true;
            _isEMPShocking = true;
            _empShokingTimer = 0;
            //_isMove = false;
            _isMoveToTheSide = false;
            _isRotate = false;
            _isShoot = false;
            _isReloading = false;

            Invoke("EMPDamageOff", 1f);
        }
    }

    public void EMPDamageOff()
    {
        if (!_isDead)
        {
            _isDamage = false;
            Stop();
            _timer = 0f;
            //Invoke("Move", _moveTime);
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
            Debug.Log("Выстрел мёртвой руки!");

            if (Physics.Raycast(FirePoint.position, _rifleDirection, out var hitInfo))
            {
                //Debug.DrawRay(SearchPoint, _direction * 15, Color.red);
                if (hitInfo.collider.gameObject.name == "Player")
                {
                    // попал по игроку
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
        _isRotate = false;
        if (!_isDamage && !_isMoveToTheSide && !_isDead && !_isEMPShocking)
        {
            if (_ammo > 0)
            {                
                _rifleDirection = FirePoint.position - RiflePoint.position;
                _shootDerection = Quaternion.LookRotation(_rifleDirection);

                _ammo -= 1;
                //            EGA_EnemyLasers.ShootEnemy(Prefab, SearchPoint, _shootDerection);
                EGA_EnemyLasers.ShootEnemy(Prefab, FirePoint.position, _shootDerection);
                ShootSound.Play();
                //Debug.Log("Выстрел мёртвой руки!");

                if (Physics.Raycast(FirePoint.position, _rifleDirection, out var hitInfo))
                {
                    //Debug.DrawRay(SearchPoint, _direction * 15, Color.red);
                    if (hitInfo.collider.gameObject.name == "Player")
                    {
                        // попал по игроку
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
                if (!_isDead && !_isDamage && !_isEMPShocking)
                {
                    if (_ammo <= 0)
                    {
                        Stop();
                        Reloading();
                        _isShoot = false;
                    }
                    else
                    {
                        //Invoke("Move", _moveTime);
                        Stop();
                        _isShoot = false;
                        _timer = _searchTime / 2f;
                        
                        
                    }
                }
            }
        }
        // Ускоренный повторный выстрел
        _timer = _searchTime/2f;
        Stop();
        //_timer = 0f;
    }

    /*public void Shoot()
    {
        if (_ammo > 0)
        {
            _isRotate = false;
            //_shoot = true;
            //Debug.Log("Выстрел!");
            //_ammo -= 1;
            _targetDirection = Player.transform.position - Enemy.transform.position;

            // задаём отклонение вызова
            // и кидаем луч повторно
            _spreadVector.y = Random.Range((_bulletSpread * -1), _bulletSpread) / 3f;  // отклонение по вертикали меньше в 3 раза
            _spreadVector.z = Random.Range((_bulletSpread * -1), _bulletSpread);  // отклонение по горизонтали

            _targetDirection.y += _spreadVector.y;  // отклонение по вертикали
            _targetDirection.z += _spreadVector.z;  // отклонение по горизонтали               

            // проверка не сбит ли прицел :)
            if (_isDead | _isDamage)
            {
                // стрельба по направлению ствола оружия
                _targetDirection = FirePoint.forward;
            }

            // преобразование Vector 3 в Quaternion!!!
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
                    // попал по игроку
                    //                Debug.Log("Попал по игроку!");
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
                    // промазал по игроку
                    //                Debug.Log("Промазал по игроку!");
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
