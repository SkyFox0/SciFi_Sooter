using DestroyIt;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class PlayerHealthComponentNew : MonoBehaviour
    {
        //public Destroy2 TargetSystem;
        public float HealthOldFloat = 100f;
        public int HealthOld = 100;
        public int HealthNew = 100;
        public int _maxHealth = 200;
        public TextMesh Live_Score_Text;
        //public AudioSource HitSound;
        //public AudioSource Dead;
        public Animator Animator;
        public SoundController SoumdController;
        public DeadController DeadController;
        public AudioSource Healing;
        private int Sound = 1;
        public AudioSource HealthFull1;
        public AudioSource HealthFull2;
        public AudioSource HealthFull3;
        public DamageEffect DamageEffect;

        void Start()
        {   
           Live_Score_Text.text = HealthOld.ToString();
            DamageEffect.SetHealth(HealthOld);
        }

        void Update()
        {
            if (HealthOld < HealthNew)  // лечение
            {
                HealthOldFloat = math.lerp(HealthOldFloat, HealthNew, 1f * Time.deltaTime);
                HealthOld = (int)Mathf.Round(HealthOldFloat);
                Live_Score_Text.text = HealthOld.ToString();
            }

            if (HealthOld > HealthNew)  // наносим урон
            {
                HealthOldFloat = math.lerp(HealthOldFloat, HealthNew, 10f * Time.deltaTime);
                HealthOld = (int)Mathf.Round(HealthOldFloat);
                Live_Score_Text.text = HealthOld.ToString();
            }
        }
        


        public void TakeDamage(int damage)
        {
            //запустить звук урона
            //Invoke("HitSoundPlay", 0.3f);
            //HitSound.Play();
            if (HealthOld > 0f)
            {
                HealthNew = HealthNew - damage;  // новый уровень здоровья

                if (HealthNew > 0f)
                {

                    //Live_Score_Text.text = HealthOld.ToString();
                    SoumdController.Damage();

                    DamageEffect.SetHealth(HealthNew);  // передаём новый уровень здоровья
                    // запускаем корректировку изменения здоровья


                }
                else
                {
                    StartDead();
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
            HealthNew = HealthNew + _health;

            if (HealthNew > _maxHealth)
            {
                HealthNew = _maxHealth;
            }
            //Live_Score_Text.text = HealthOld.ToString();
            DamageEffect.SetHealth(HealthNew);
        }

        public void HealthFull()
        {
            if (!HealthFull1.isPlaying && !HealthFull2.isPlaying && !HealthFull3.isPlaying)
            {
                if (Sound == 1)
                {
                    HealthFull1.Play();
                    Sound = 2;
                }
                else
                {
                    if (Sound == 2)
                    {
                        HealthFull2.Play();
                        Sound = 3;
                    }
                    else
                    {
                        if (Sound == 3)
                        {
                            HealthFull3.Play();
                            Sound = 1;
                        }
                    }
                }
            }
        }
    }
}
