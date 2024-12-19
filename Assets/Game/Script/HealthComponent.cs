using Unity.VisualScripting;
using UnityEngine;

namespace StarterAssets
{

    public class HealthComponent : MonoBehaviour
    {
        public Destroy2 TargetSystem;
        public int Health = 100;
        public AudioSource HitSound;
        public AudioSource DestroySound;
        public ExplodeTarget _ExplodeTarget;

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
                DestroySound.Play();
                //����� ������ ���������� � ��������� 0,2�
                TargetSystem.DestroyTarget2();

                Invoke("DestroyTarget", 0.1f);

                //_ExplodeTarget.StartExplosion();
                //Destroy(gameObject);
            }
        }

        public void DestroyTarget()
        {
            _ExplodeTarget.StartExplosion();

        }

        public void HitSoundPlay()
        {
            HitSound.Play();
        }
    }

}
