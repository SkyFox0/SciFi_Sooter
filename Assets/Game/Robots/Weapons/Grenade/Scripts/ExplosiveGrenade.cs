//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Timeline;

public class ExplosiveGrenade : MonoBehaviour
{
    //public Collider Explosion; // сфера взрыва
    public GameObject Grenade; // корпус гранаты  
    public float _time = 4f;  // врем€ до взрыва
    public float _timer = -1f;  // таймер
    public ParticleSystem EnergyExplosion;
    public AnimationCurve Inensivity;
    public Light RedLight;
    public AudioSource Throw;
   // public AudioSource Down;
    public AudioSource Timer;
    public AudioSource ExplosionSound;
   // public AudioSource Lightning;
    public ExlosionDamage ExlosionDamage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GrenadeActivation();
        Throw.Play();  
        Timer.Play();


    }
    public void GrenadeActivation()
    {
        // запускаем таймер
        _timer = 0f;
    }

    public void StartExplosion()
    {
        ExlosionDamage.GrenadeActivation();  // активаци€ коллайдера;
        EnergyExplosion.Play();  // анимаци€ взрыва
        ExplosionSound.Play();  // звук взрыва
        Invoke("DestroyGrenade", 0.2f);
        

    }

    public void DestroyGrenade()
    {
        Destroy(Grenade); // ”ничтожение оболочки гранаты
        Invoke("DestroyGrenadeFull", 3f);
    }

    public void DestroyGrenadeFull()
    {
        Destroy(gameObject); // ”ничтожение обьекта граната   
    }



    // Update is called once per frame
    void Update()
    {
        if (_timer >= 0f)
        {
            RedLight.intensity = Inensivity.Evaluate(_timer); // мигающий свет
            _timer += Time.deltaTime;
        }
        if (_timer >= _time)  
        {
            _timer = -1f; // остановка таймера и взрыв
            StartExplosion();
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        Down.Play();
    }*/
}
