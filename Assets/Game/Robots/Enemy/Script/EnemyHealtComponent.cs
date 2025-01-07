//using DestroyIt;
//using Unity.VisualScripting;
using DestroyIt;
using UnityEngine;
//using UnityEngine.AI;
//using System.Collections;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.InputSystem;

namespace StarterAssets
{

    public class EnemyHealthComponent : MonoBehaviour
    {
        public GameObject Enemy;
        //public Destroy2 TargetSystem;
        public int Health = 100;
        public bool isDead = false;
        public AudioSource HitSound;
        public AudioSource HitEMP;
       
        public AudioSource DamageSound;
        public AudioSource DeadSound;        
        //public ExplodeTarget _ExplodeTarget;
        public EnemySpawnSystem EnemySpawnSystem;
        public GameObject SpawnSystem;
        public Animator EnemyAnimator;
        //public Animator SFX_Animator;
        //public EnimeAnimatorController EnimeAnimatorController;
        public EnemyMovement EnemyMovement;
        public Collider EnemyCollider;
        public ParticleSystem WhiteSmoke;
        public ParticleSystem BlackSmoke;
        public ParticleSystem BlackSmokeDead;
        public ParticleSystem Sparks_1;
        public ParticleSystem Sparks_2;

        [Header("HeadShot")]
        public ParticleSystem HeadSparks;
        public ParticleSystem HeadDestroy;
        public GameObject Head;
        public GameObject RoboHead;
        public GameObject RoboHeadPosition;
        public Rigidbody RoboHeadRB;

        [Header("AmmoDrop")]
        public GameObject Ammo_Box_Enemy_Drop;
        public Rigidbody AmmoBoxRB;

        public GameObject Player;
        public Ui_Control Ui_Control;



        void Start()
        {
            SpawnSystem = GameObject.Find("EnemySpawnSystem");
            EnemySpawnSystem = SpawnSystem.GetComponent<EnemySpawnSystem>();
            EnemyAnimator = GetComponentInChildren<Animator>();
            //EnimeAnimatorController = GetComponent<EnimeAnimatorController>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyCollider = GetComponent<Collider>();

            Player = GameObject.Find("Player");
            Ui_Control = Player.GetComponentInChildren<Ui_Control>();

        }

        public void HeadShot (int damage)
        {
            if (!isDead)
            {
                HitSound.Play();
                //Debug.Log("ХЕДШОТ4!!!");
                Health = Health - damage;
                if (Health > 19 && Health < 30)
                {
                    if (!WhiteSmoke.isPlaying) { WhiteSmoke.Play(); }
                }

                if (Health > 9 && Health < 20)
                {
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (!BlackSmoke.isPlaying) { BlackSmoke.Play(); }
                }


                //Debug.Log("Take Damag! Health = " + Health);
                if (Health <= 0)
                {
                    
                    isDead = true;
                    DeadSound.Play();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetBool("isMove", false);
                    EnemyAnimator.SetBool("isDead", true);
                    EnemyCollider.enabled = false;

                    //запустить звук разрушения
                    //DeadSound.Play();
                    //вызов метода разрушения с задержкой 0,2с
                    //TargetSystem.DestroyTarget2();

                    //Debug.Log("==\\Смерть врага//==");
                    EnemyMovement.NavMeshAgent.speed = 0f;
                    //EnemyMovement.NavMeshAgent.Stop(); // Устарело!
                    EnemyMovement.NavMeshAgent.isStopped = true;
                    EnemyMovement._isDead = true;
                    EnemyMovement._isMove = false;

                    //запускаем анимацию падения дыма и искры
                    //SFX_Animator.SetBool("isDead", true);
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (BlackSmoke.isPlaying) { BlackSmoke.Stop(); }
                    BlackSmokeDead.Play();
                    Sparks_1.Play();
                    Sparks_2.Play();

                    //Взрыв головы                 
                    HeadDestroy.Play();
                    HeadSparks.Play();
                    //голову выключаем
                    Head.gameObject.SetActive(false);
                    // вторую голову включаем и запускаем в воздух
                    //RoboHead.gameObject.SetActive(true);
                    RoboHead = Instantiate(RoboHead, RoboHeadPosition.transform.position, RoboHeadPosition.transform.rotation);
                    RoboHeadRB = RoboHead.GetComponent<Rigidbody>();
                    //RoboHeadRB.AddForce(HeadDirection * _force);
                    // отстрел башки
                    RoboHeadRB.AddForce(Random.Range(-2f, 0f), Random.Range(3f, 5f), Random.Range(0f, 1f), ForceMode.VelocityChange); // ForceMode.Force // ForceMode.Acceleration // ForceMode.Impulse 
                    RoboHeadRB.AddTorque(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f), ForceMode.VelocityChange);
                    //Debug.Log("ХЕДШОТ5!!!");
                    EnemySpawnSystem.Spawn();

                    Ui_Control.AddFrag();
                    // посмертный выстрел
                    float _t = Random.Range(0.1f, 0.5f);
                    Invoke("DeadShoot", _t);
                    //уничтожение тела через 30 сек
                    Invoke("Dead", 30f);
                    Invoke("DropAmmo", 1f);

                    //_ExplodeTarget.StartExplosion();
                    //Destroy(gameObject);
                }
                else
                {
                    EnemyMovement.Damage();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetTrigger("Damage");  // запуск анимации урона
                                                         //Invoke("DamageSoundPlay", 0.1f);
                    DamageSoundPlay();
                }
            }            
        }

        public void DeadShoot()
        {
            EnemyMovement.DeadShoot();
        }


        public void TakeDamage(int damage)
        {
            if (!isDead)
            {
                //запустить звук урона
                HitSound.Play();

                Health = Health - damage;
                if (Health > 19 && Health < 30)
                {
                    if (!WhiteSmoke.isPlaying) { WhiteSmoke.Play(); }
                }

                if (Health > 9 && Health < 20)
                {
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (!BlackSmoke.isPlaying) { BlackSmoke.Play(); }
                }


                //Debug.Log("Take Damag! Health = " + Health);
                if (Health <= 0)
                {
                    isDead = true;
                    DeadSound.Play();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetBool("isMove", false);
                    EnemyAnimator.SetBool("isDead", true);
                    EnemyCollider.enabled = false;

                    //запустить звук разрушения
                    //DeadSound.Play();
                    //вызов метода разрушения с задержкой 0,2с
                    //TargetSystem.DestroyTarget2();

                    //Debug.Log("==\\Смерть врага//==");
                    EnemyMovement.NavMeshAgent.speed = 0f;
                    //EnemyMovement.NavMeshAgent.Stop(); // Устарело!
                    EnemyMovement.NavMeshAgent.isStopped = true;
                    EnemyMovement._isDead = true;
                    EnemyMovement._isMove = false;

                    //запускаем анимацию падения дыма и искры
                    //SFX_Animator.SetBool("isDead", true);
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (BlackSmoke.isPlaying) { BlackSmoke.Stop(); }
                    BlackSmokeDead.Play();
                    Sparks_1.Play();
                    Sparks_2.Play();

                    // посмертный выстрел
                    float _t = Random.Range(0.1f, 0.5f);
                    Invoke("DeadShoot", _t);
                    EnemySpawnSystem.Spawn();
                    Ui_Control.AddFrag();

                    //уничтожение тела через 30 сек
                    Invoke("Dead", 30f);
                    Invoke("DropAmmo", 1f);

                    //_ExplodeTarget.StartExplosion();
                    //Destroy(gameObject);
                }
                else
                {
                    EnemyMovement.Damage();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetTrigger("Damage");  // запуск анимации урона
                                                         //Invoke("DamageSoundPlay", 0.1f);
                    DamageSoundPlay();
                }
            }                
        }

        public void TakeEMPDamage(int damage)
        {
            if (!isDead)
            {
                //запустить звук урона
                HitEMP.Play();

                Health = Health - damage;
                if (Health > 19 && Health < 30)
                {
                    if (!WhiteSmoke.isPlaying) { WhiteSmoke.Play(); }
                }

                if (Health > 9 && Health < 20)
                {
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (!BlackSmoke.isPlaying) { BlackSmoke.Play(); }
                }

                if (Health <= 0)
                {
                    isDead = true;
                    DeadSound.Play();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetBool("isMove", false);
                    EnemyAnimator.SetBool("isDead", true);
                    EnemyCollider.enabled = false;

                    //запустить звук разрушения
                    //DeadSound.Play();
                    //вызов метода разрушения с задержкой 0,2с
                    //TargetSystem.DestroyTarget2();

                    //Debug.Log("==\\Смерть врага//==");
                    EnemyMovement.NavMeshAgent.speed = 0f;
                    //EnemyMovement.NavMeshAgent.Stop(); // Устарело!
                    EnemyMovement.NavMeshAgent.isStopped = true;
                    EnemyMovement._isDead = true;
                    EnemyMovement._isMove = false;

                    //запускаем анимацию падения дыма и искры
                    //SFX_Animator.SetBool("isDead", true);
                    if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                    if (BlackSmoke.isPlaying) { BlackSmoke.Stop(); }
                    BlackSmokeDead.Play();
                    Sparks_1.Play();
                    Sparks_2.Play();

                    // посмертный выстрел
                    float _t = Random.Range(0.1f, 0.5f);
                    Invoke("DeadShoot", _t);
                    EnemySpawnSystem.Spawn();
                    Ui_Control.AddFrag();

                    //уничтожение тела через 30 сек
                    Invoke("Dead", 30f);
                    Invoke("DropAmmo", 1f);

                    //_ExplodeTarget.StartExplosion();
                    //Destroy(gameObject);
                }
                else
                {
                    /*EnemyMovement.Stop();
                    EnemyMovement._isEMPShocking = true;
                    EnemyMovement._empShokingTimer = 0;
                    EnemyMovement.NavMeshAgent.speed = 0f;
                    EnemyMovement.NavMeshAgent.isStopped = true;
                    EnemyMovement._isDead = false;
                    EnemyMovement._isMove = false;*/


                    EnemyMovement.EMPDamage();
                    EnemyAnimator.SetBool("isReloading", false);
                    EnemyAnimator.SetTrigger("Damage");  // запуск анимации урона
                                                         //Invoke("DamageSoundPlay", 0.1f);
                    DamageSoundPlay();
                }
            }
        }


        public void Dead()
        {   
            Destroy(gameObject);
        }



        public void DamageSoundPlay()
        {
            DamageSound.Play();
            

        }

 
        public void DropAmmo()
        {
            //Vector3 DropPosition = new Vector3(Random.Range(-1, 1), 0.5f, Random.Range(-1, 1));
            Vector3 DropPosition = Enemy.transform.position;// + DropPosition;
            DropPosition.y = 0.5f;
            Ammo_Box_Enemy_Drop = Instantiate(Ammo_Box_Enemy_Drop, DropPosition, Ammo_Box_Enemy_Drop.transform.rotation);
            //Ammo_Box_Enemy_Drop = Instantiate(Ammo_Box_Enemy_Drop, RoboHeadPosition.transform.position, RoboHeadPosition.transform.rotation);

            AmmoBoxRB = Ammo_Box_Enemy_Drop.GetComponent<Rigidbody>();
            //RoboHeadRB.AddForce(HeadDirection * _force);
            // отстрел башки
            AmmoBoxRB.AddForce(Random.Range(-1, 1), 3, Random.Range(-1, 1), ForceMode.VelocityChange); // ForceMode.Force // ForceMode.Acceleration // ForceMode.Impulse 
        }

    }

}
