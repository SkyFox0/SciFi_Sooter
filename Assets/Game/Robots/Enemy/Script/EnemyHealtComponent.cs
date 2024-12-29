using DestroyIt;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace StarterAssets
{

    public class EnemyHealthComponent : MonoBehaviour
    {
        //public Destroy2 TargetSystem;
        public int Health = 100;
        public AudioSource HitSound;
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
        public ParticleSystem Sparks_1;
        public ParticleSystem Sparks_2;

        [Header("HeadShot")]
        public ParticleSystem HeadSparks;
        public ParticleSystem HeadDestroy;
        public GameObject Head;


        void Start()
        {
            SpawnSystem = GameObject.Find("EnemySpawnSystem");
            EnemySpawnSystem = SpawnSystem.GetComponent<EnemySpawnSystem>();
            EnemyAnimator = GetComponentInChildren<Animator>();
            //EnimeAnimatorController = GetComponent<EnimeAnimatorController>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyCollider = GetComponent<Collider>();
        }

        public void HeadShot (int damage)
        {
            HitSound.Play();
            Debug.Log("ХЕДШОТ4!!!");
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
                DeadSound.Play();
                EnemyAnimator.SetBool("isReloading", false);
                EnemyAnimator.SetBool("isMove", false);
                EnemyAnimator.SetBool("isDead", true);
                EnemyCollider.enabled = false;

                //запустить звук разрушения
                //DeadSound.Play();
                //вызов метода разрушения с задержкой 0,2с
                //TargetSystem.DestroyTarget2();

                Debug.Log("==\\Смерть врага//==");
                EnemyMovement.NavMeshAgent.speed = 0f;
                //EnemyMovement.NavMeshAgent.Stop(); // Устарело!
                EnemyMovement.NavMeshAgent.isStopped = true;
                EnemyMovement._isDead = true;
                EnemyMovement._isMove = false;

                //запускаем анимацию падения дыма и искры
                //SFX_Animator.SetBool("isDead", true);
                if (WhiteSmoke.isPlaying) { WhiteSmoke.Stop(); }
                if (!BlackSmoke.isPlaying) { BlackSmoke.Play(); }
                Sparks_1.Play();
                Sparks_2.Play();

                //Взрыв головы
                
                HeadDestroy.Play();
                HeadSparks.Play();

                Head.gameObject.SetActive(false);




                Debug.Log("ХЕДШОТ5!!!");
                EnemySpawnSystem.Spawn();


                //уничтожение тела через 30 сек
                Invoke("Dead", 30f);

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


        public void TakeDamage(int damage)
        {
            //запустить звук урона
            HitSound.Play();
            
            //EnemyAnimator.Stop();
            //EnemyAnimator.enabled = false;
            //EnemyAnimator.enabled = true;


            //HitSound.Play();

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
                DeadSound.Play();
                EnemyAnimator.SetBool("isReloading", false);
                EnemyAnimator.SetBool("isMove", false);
                EnemyAnimator.SetBool("isDead", true);
                EnemyCollider.enabled = false;

                //запустить звук разрушения
                //DeadSound.Play();
                //вызов метода разрушения с задержкой 0,2с
                //TargetSystem.DestroyTarget2();

                Debug.Log("==\\Смерть врага//==");
                EnemyMovement.NavMeshAgent.speed = 0f;
                //EnemyMovement.NavMeshAgent.Stop(); // Устарело!
                EnemyMovement.NavMeshAgent.isStopped = true;
                EnemyMovement._isDead = true;
                EnemyMovement._isMove = false;

                //запускаем анимацию падения дыма и искры
                //SFX_Animator.SetBool("isDead",true);
                Sparks_1.Play();
                Sparks_2.Play();


                EnemySpawnSystem.Spawn();
                

                //уничтожение тела через 30 сек
                Invoke("Dead", 30f);

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

        public void Dead()
        {   
            Destroy(gameObject);
        }



        public void DamageSoundPlay()
        {
            DamageSound.Play();
            

        }

    }

}
