using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        //public EnimeAnimatorController EnimeAnimatorController;
        public EnemyMovement EnemyMovement;
        public Collider EnemyCollider;


        void Start()
        {
            SpawnSystem = GameObject.Find("EnemySpawnSystem");
            EnemySpawnSystem = SpawnSystem.GetComponent<EnemySpawnSystem>();
            EnemyAnimator = GetComponentInChildren<Animator>();
            //EnimeAnimatorController = GetComponent<EnimeAnimatorController>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyCollider = GetComponent<Collider>();
        }


            public void TakeDamage(int damage)
        {
            //��������� ���� �����
            HitSound.Play();
            
            //EnemyAnimator.Stop();
            //EnemyAnimator.enabled = false;
            //EnemyAnimator.enabled = true;


            //HitSound.Play();

            Health = Health - damage;
            Debug.Log("Take Damag! Health = " + Health);
            if (Health <= 0)
            {
                DeadSound.Play();
                EnemyAnimator.SetBool("isReloading", false);
                EnemyAnimator.SetBool("isMove", false);
                EnemyAnimator.SetBool("isDead", true);
                EnemyCollider.enabled = false;

                //��������� ���� ����������
                //DeadSound.Play();
                //����� ������ ���������� � ��������� 0,2�
                //TargetSystem.DestroyTarget2();

                Debug.Log("==\\������ �����//==");
                EnemyMovement.NavMeshAgent.speed = 0f;
                //EnemyMovement.NavMeshAgent.Stop(); // ��������!
                EnemyMovement.NavMeshAgent.isStopped = true;
                EnemyMovement._isDead = true;
                EnemyMovement._isMove = false;               
                
                EnemySpawnSystem.Spawn();
                

                //����������� ���� ����� 30 ���
                Invoke("Dead", 30f);

                //_ExplodeTarget.StartExplosion();
                //Destroy(gameObject);
            }
            else
            {
                EnemyMovement.Damage();
                EnemyAnimator.SetBool("isReloading", false);
                EnemyAnimator.SetTrigger("Damage");  // ������ �������� �����
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
