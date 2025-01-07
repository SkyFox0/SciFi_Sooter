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
    //public float _time = 4f;  // ����� �� ������
    public int _maxDistance = 5;  // ������������ ��������� �����
    public AnimationCurve InensivityDamage; // ������ �����
    public int _maxDamage = 30;  //  ������������ ����    
    public int _maxCount = 5;  // ���������� ������
    public int _count = 0;  // ������� ������
    public float _timer = -1f;  // ������
    public bool _takeDamage = false;
    public float _power = 3f; // ���� ������
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
                _timer = -1f; // ��������� �������
                _takeDamage = false;
            }
        }
    }

    public void GrenadeActivation()
    {
        _timer = 0f;  // ��������� �������
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
                            Debug.Log("��������� ��������� � ���������: " + hit.ToString());
                            //Debug.Log("��������� ��������� - �������������!");
                            rb.AddExplosionForce(_power, Grenade.transform.position, _maxDistance / 2f, 0.2f, ForceMode.Impulse);
                        }
                    }
                    catch { }
                }
                else
                {
                    
                }
            }
            if (hit.gameObject.tag == "Player") // ���� �� ����!
            {
                Debug.Log("��������� �����!");
                // ��������� ���������� �� ������
                float _distance = (hit.transform.position - Grenade.transform.position).magnitude;
                Debug.Log("���������� �� ������ = "+ _distance.ToString());

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance / _maxDistance) * _maxDamage); // ������� �����;

                try
                {
                    Debug.Log("������������� ���� �� ���� = " + _damage.ToString() + "!");
                    hit.GetComponent<PlayerHealthComponentNew>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }
            }


            if (hit.gameObject.tag == "Enemy") // ���� �� �����!
            {

                Debug.Log("��������� ����!");
                // ��������� ���������� �� �����
                float _distance = (hit.transform.position - Grenade.transform.position).magnitude;
                Debug.Log("���������� �� ����� = " + _distance.ToString());
                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance / _maxDistance) * _maxDamage); // ������� �����;


                try
                {
                    Debug.Log("������������� ���� �� ����� = " + _damage.ToString() + "!");
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
            //Debug.Log("��������� ���������: " + other.ToString());
            if (other.GetComponent<Rigidbody>() != null)  //(other.attachedRigidbody)
            {
                //Debug.Log("��������� ��������� - �������������!");
                try
                {   
                    //other.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Acceleration);
                    other.GetComponent<Rigidbody>().AddExplosionForce(_power, Grenade.transform.position, _maxDistance, 3.0f);
                }
                catch { }
                
            }

            //if (other.TryGetComponent(out EnemyHealthComponent enemyHealthComponent))
            if (other.gameObject.tag == "Player") // ���� �� ����!
            {
                Debug.Log("��������� �����!");
                // ��������� ���������� �� �����
                float _distance = (other.transform.position - Grenade.transform.position).magnitude;

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance)); // ������� �����;
                
                try
                {
                    Debug.Log("������������� ���� �� ���� = " + _damage.ToString() + "!");
                    other.GetComponent<PlayerHealthComponentNew>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }
            }


            if (other.gameObject.tag == "Enemy") // ���� �� �����!
            {

                Debug.Log("��������� ����!");
                // ��������� ���������� �� �����
                float _distance = (other.transform.position - Grenade.transform.position).magnitude;

                int _damage = (int)Mathf.Round(InensivityDamage.Evaluate(_distance)); // ������� �����;
                

                try
                {
                    Debug.Log("������������� ���� �� ����� = " + _damage.ToString() + "!");
                    other.GetComponent<EnemyHealthComponent>().TakeDamage(_damage);
                    Lightning.Play();
                }
                catch { }


            }
            _takeDamage = false;
        }
    }*/

}
