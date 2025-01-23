using UnityEngine;

public class DoorsTrigger : MonoBehaviour
{
   //public GameObject Doors;
    public DoorSystem DoorSystem;
    /*public Animator DoorsAnimator;
    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;
    public bool _isOpen;
    public bool _isCloseTimer;  // есть ли таймер закрытия
    public float _timer = -1f;
    public float _timeClose = 10f;  // вемя закрытия по таймеру*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_timer = -1f;
        //_timeClose = 10f;
        /*if (_isOpen)
        {
            Invoke("ToCloseDoors", 2f);

        }*/
    }

    /*public void ToCloseDoors()
    {
        DoorsAnimator.SetTrigger("ToClose");
        _isOpen = false;
        DoorsClose.Play();
        _timer = -1f;

    }*/


    // Update is called once per frame
    /*void Update()
    {
        // таймер закрытия
        if (_isOpen && _timer >=0f)
        {
            _timer += Time.deltaTime;
        }
        if (_timer > _timeClose)
        {
            _timer = -1f;
            _isOpen = false;
            DoorsAnimator.SetBool("isOpen", false);
            DoorsAnimator.SetTrigger("ToClose");
            DoorsClose.Play();
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // если к двери подошел игрок
            DoorSystem.PlayerInTrigger();
            Debug.Log("к двери подошел игрок");
            /*_isOpen = true;
            DoorsOpen.Play();
            DoorsAnimator.SetBool("isOpen", true);
            DoorsAnimator.SetTrigger("ToOpen");

            if (_isCloseTimer)
            {
                _timer = 0f;
            }*/
        } 
        
        if (other.gameObject.tag == "Enemy")
        {
            // если к двери подошел враг
            DoorSystem.EnemyInTrigger();
            Debug.Log("к двери подошел враг");

        }
    }
}
