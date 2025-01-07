//using DestroyIt;
using StarterAssets;
//using System.IO;
//using UnityEditor.PackageManager;
using UnityEngine;
using System.Collections;

public class ExlosionDamage : MonoBehaviour
{
    public GameObject Grenade;
    //public GameObject Trigger;
    public AudioSource Lightning;
    //public float _time = 4f;  // время до взрыва
    public int _maxDistance = 5;  // максимальная дистанция урона
    public AnimationCurve InensivityDamage; // кривая урона
    public int _maxDamage = 30;  //  Максимальный урон    
    public int _maxCount = 5;  // количество ударов
    public int _count = 0;  // счетчик ударов
    public float _timer = -1f;  // таймер
    public bool _takeDamage = false;
    public float _power = 3f; // сила взрыва
    public Collider[] colliders;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_timer = 0f;


    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= 0f)
        {            
            _timer += Time.deltaTime;
        }
        if (_timer > 0.5f && _count <= 3)
        {
            _count += 1;
            _timer = 0f;
            _takeDamage = true;
            GrenadeExplosion();
        }
        else
        {
            if (_count == 3)
            { 
                _timer = -1f; // остановка таймера
                _takeDamage = false;
            }
        }
    }

    public void GrenadeActivation()
    {
        _timer = 0f;  // активация гранаты
        _takeDamage = true;
        _count = 1;
        GrenadeExplosion();

    }

    public void GrenadeExplosion()
    {
        Debug.Log(Grenade.GetComponent<Collider>().ToString());
        //Debug.Log(Trigger.GetComponent<Collider>().ToString());
        Debug.Log("--------------");



        colliders = Physics.OverlapSphere(Grenade.transform.position, _maxDistance);


        foreach (Collider hit in colliders)
        {
            if (_count == 1)
            {

                Debug.Log(hit.ToString());

                if (hit != Grenade.GetComponent<Collider>() & hit) // | Trigger.GetComponent<Collider>())
                {
                    try
                    {

                        Rigidbody rb = hit.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Debug.Log("Обнаружен коллайдер с ригидбоди: " + hit.ToString());
                            //Debug.Log("Обнаружен ригидбоди - подбрасывание!");
                            rb.AddExplosionForce(_power, Grenade.transform.position, _maxDistance / 2f, 0.2f, ForceMode.Impulse);
                        }
                    }
                    catch { }
                }
                else
                {
                    
                }
            }
            if (hit.gameObject.tag == "Player") // урон по себе!
            {
                Debug.Log("Обнаружен игрок!");
                // вычислить расстояние до игрока
                float _distance = (hit.transform.position - Grenade.transform.position).magnitude;
                Debug.Log("Расстояние до игрока = "+ _distance.ToString());

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance / _maxDistance) * _maxDamage); // рассчет урона;

                try
                {
                    Debug.Log("Электрический урон по себе = " + _damage.ToString() + "!");
                    hit.GetComponent<PlayerHealthComponentNew>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }
            }


            if (hit.gameObject.tag == "Enemy") // урон по врагу!
            {

                Debug.Log("Обнаружен враг!");
                // вычислить расстояние до врага
                float _distance = (hit.transform.position - Grenade.transform.position).magnitude;
                Debug.Log("Расстояние до врага = " + _distance.ToString());
                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance / _maxDistance) * _maxDamage); // рассчет урона;


                try
                {
                    Debug.Log("Электрический урон по врагу = " + _damage.ToString() + "!");
                    hit.GetComponent<EnemyHealthComponent>().TakeEMPDamage(_damage);
                    Lightning.Play();
                }
                catch { }
            }
        }
        _takeDamage = false;
    }

    /*public void OnTriggerStay(Collider other)
    {
        if (_takeDamage)
        {
            //Debug.Log("Обнаружен коллайдер: " + other.ToString());
            if (other.GetComponent<Rigidbody>() != null)  //(other.attachedRigidbody)
            {
                //Debug.Log("Обнаружен ригидбоди - подбрасывание!");
                try
                {   
                    //other.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Acceleration);
                    other.GetComponent<Rigidbody>().AddExplosionForce(_power, Grenade.transform.position, _maxDistance, 3.0f);
                }
                catch { }
                
            }

            //if (other.TryGetComponent(out EnemyHealthComponent enemyHealthComponent))
            if (other.gameObject.tag == "Player") // урон по себе!
            {
                Debug.Log("Обнаружен игрок!");
                // вычислить расстояние до врага
                float _distance = (other.transform.position - Grenade.transform.position).magnitude;

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance)); // рассчет урона;
                
                try
                {
                    Debug.Log("Электрический урон по себе = " + _damage.ToString() + "!");
                    other.GetComponent<PlayerHealthComponentNew>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }
            }


            if (other.gameObject.tag == "Enemy") // урон по врагу!
            {

                Debug.Log("Обнаружен враг!");
                // вычислить расстояние до врага
                float _distance = (other.transform.position - Grenade.transform.position).magnitude;

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance)); // рассчет урона;
                

                try
                {
                    Debug.Log("Электрический урон по врагу = " + _damage.ToString() + "!");
                    other.GetComponent<EnemyHealthComponent>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }


            }
            _takeDamage = false;
        }
    }*/

}
