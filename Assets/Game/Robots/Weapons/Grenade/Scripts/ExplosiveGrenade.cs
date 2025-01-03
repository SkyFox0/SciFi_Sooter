//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Timeline;

public class ExplosiveGrenade : MonoBehaviour
{
    //public Collider Explosion; // ����� ������
    public GameObject Grenade; // ������ �������  
    public float _time = 4f;  // ����� �� ������
    public float _timer = -1f;  // ������
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
        // ��������� ������
        _timer = 0f;
    }

    public void StartExplosion()
    {
        ExlosionDamage.GrenadeActivation();  // ��������� ����������;
        EnergyExplosion.Play();  // �������� ������
        ExplosionSound.Play();  // ���� ������
        Invoke("DestroyGrenade", 0.2f);
        

    }

    public void DestroyGrenade()
    {
        Destroy(Grenade); // ����������� �������� �������
        Invoke("DestroyGrenadeFull", 3f);
    }

    public void DestroyGrenadeFull()
    {
        Destroy(gameObject); // ����������� ������� �������   
    }



    // Update is called once per frame
    void Update()
    {
        if (_timer >= 0f)
        {
            RedLight.intensity = Inensivity.Evaluate(_timer); // �������� ����
            _timer += Time.deltaTime;
        }
        if (_timer >= _time)  
        {
            _timer = -1f; // ��������� ������� � �����
            StartExplosion();
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        Down.Play();
    }*/
}
