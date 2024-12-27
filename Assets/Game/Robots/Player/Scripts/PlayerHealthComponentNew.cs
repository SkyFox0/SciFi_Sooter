using DestroyIt;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class PlayerHealthComponentNew : MonoBehaviour
    {
        //public Destroy2 TargetSystem;
        public int Health = 100;
        public int _maxHealth = 130;
        public TextMesh Live_Score_Text;
        //public AudioSource HitSound;
        //public AudioSource Dead;
        public Animator Animator;
        public SoundController SoumdController;
        public DeadController DeadController;
        public AudioSource Healing;

        void Start()
        {   
           Live_Score_Text.text = Health.ToString();


        }

        void Update()
        {
            //if (Health > 0)
            //{
            //    TakeDamage(5);
            //}

            //TakeDamage(105);
        }
        public void OnDamage(InputValue value)
        {
            TakeDamage(10);

        }


        public void TakeDamage(int damage)
        {
            //запустить звук урона
            //Invoke("HitSoundPlay", 0.3f);
            //HitSound.Play();
            if (Health > 0f)
            {
                Health = Health - damage;
                Live_Score_Text.text = Health.ToString();
                SoumdController.Damage();


                if (Health <= 0)
                {
                    //запустить анимацию и звук гибели            
                    StartDead();

                    //вызов метода разрушения с задержкой 0,2с
                    //TargetSystem.DestroyTarget2();

                    //Invoke("DestroyTarget", 0.1f);

                    //_ExplodeTarget.StartExplosion();
                    //Destroy(gameObject);
                }
            }
        }

        public void StartDead()
        {
            SoumdController.DeadSound();
            Animator.SetBool("isDead", true);
            //_ExplodeTarget.StartExplosion();     
            DeadController.PlayerIsDead();
        }

        public void AddHealth(int _health)
        {
            Health = Health + _health;
            if (Health > _maxHealth)
            {
                Health = _maxHealth;
            }
            Live_Score_Text.text = Health.ToString();
        }


    }
}
