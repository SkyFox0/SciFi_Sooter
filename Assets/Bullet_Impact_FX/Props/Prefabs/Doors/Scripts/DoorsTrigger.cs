using UnityEngine;

public class DoorsTrigger : MonoBehaviour
{
    public GameObject Doors;
    public Animator DoorsAnimator;
    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;
    public bool _open;
    public bool _isCloseTimer;
    public float _timer = -1f;
    public float _timeClose = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_timer = -1f;
        //_timeClose = 10f;
        if (_open)
        {
            Invoke("ToCloseDoors", 2f);

        }
    }

    public void ToCloseDoors()
    {
        DoorsAnimator.SetTrigger("ToClose");
        _open = false;
        DoorsClose.Play();
        _timer = -1f;

    }


    // Update is called once per frame
    void Update()
    {
        if (_open && _timer >=0f)
        {
            _timer += Time.deltaTime;
        }
        if (_timer > _timeClose)
        {
            _timer = -1f;
            _open = false;
            DoorsAnimator.SetBool("Open", false);
            DoorsClose.Play();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player") && !_open)
        {
            _open = true;
            DoorsOpen.Play();
            DoorsAnimator.SetBool("Open", true);

            if (_isCloseTimer)
            {
                _timer = 0f;
            }
        }        
    }
}
