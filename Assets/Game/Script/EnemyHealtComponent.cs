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
        public EnemyMovement EnemyMovement;
        public Collider EnemyCollider;


        void Start()
        {
            SpawnSystem = GameObject.Find("EnemySpawnSystem");
            EnemySpawnSystem = SpawnSystem.GetComponent<EnemySpawnSystem>();
            EnemyAnimator = GetComponentInChildren<Animator>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyCollider = GetComponent<Collider>();
        }


            public void TakeDamage(int damage)
        {
            //��������� ���� �����
            Invoke("HitSoundPlay", 0.3f);
            //HitSound.Play();

            Health = Health - damage;
            Debug.Log("Take Damag! Health = " + Health);
            if (Health <= 0)
            {
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
                DeadSound.Play();
                EnemyAnimator.SetBool("isDead", true);
                EnemySpawnSystem.Spawn();
                EnemyCollider.enabled = false;

                //����������� ���� ����� 30 ���
                Invoke("Dead", 30f);

                //_ExplodeTarget.StartExplosion();
                //Destroy(gameObject);
            }
        }

        public void Dead()
        {   
            Destroy(gameObject);
        }



        public void HitSoundPlay()
        {
            DamageSound.Play();
            HitSound.Play();
        }
    }

}
