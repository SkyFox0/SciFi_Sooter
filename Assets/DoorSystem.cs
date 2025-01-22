using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Animator DoorsAnimator;
    public bool _doorIsOpen; // открыта ли дверь прямо сейчас
    public bool _doorIsLocked; // заблокирована ли дверь (можно ли её открыть)
    public bool _doorTrigger; // есть ли у двери триггер

    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;

    public bool _isCloseTimer;  // есть ли таймер закрытия
    public float _timer = -1f;
    private float _saveTimer;
    public float _timeClose = 10f;  // вемя закрытия по таймеру


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_doorIsOpen = false;
        //_doorIsLocked = false;

        //_timer = -1f;
        //_timeClose = 10f;
        if (_doorIsOpen)
        {
            Invoke("ToCloseDoors", 2f);
        }
    }

    public void ToCloseDoors()
    {
        DoorsAnimator.SetTrigger("ToClose");
        _doorIsOpen = false;
        DoorsClose.Play();
        _timer = -1f;
    }

    void Update()
    {
        // таймер закрытия дверей

        if (_isCloseTimer && _doorIsOpen && _timer >= 0f)
        {
            _timer += Time.deltaTime;
        }
        if (_timer > _timeClose)
        {
            CloseDoor();
        }
    }


    public void PlayerInTrigger()
    {
        if (_doorTrigger && !_doorIsOpen && !_doorIsLocked)
        {
            OpenDoor();
        }
    }

    public void EnemyInTrigger()
    {
        //----------------
    }

    public void OpenDoor()
    {
        _doorIsOpen = true;
        DoorsOpen.Play();
        DoorsAnimator.SetBool("isOpen", true);
        DoorsAnimator.SetTrigger("ToOpen");

        if (_isCloseTimer && !_doorIsLocked)
        {
            // запуск таймера автоматического закрытия двери
            _timer = 0f;
        }
    }

    public void CloseDoor()
    {
        _timer = -1f;
        _doorIsOpen = false;
        DoorsAnimator.SetBool("isOpen", false);
        DoorsAnimator.SetTrigger("ToClose");
        DoorsClose.Play();
    }

    public void CloseAndSetLocked()  // функция закрывает и блокирует дверь
    {
        if (_doorIsOpen)
        {
            // включить автозакрытие
            CloseDoor();
        }        
        _doorIsLocked = true;
        _timer = -1f;
    }

    public void SetLocked()  // функция просто блокирует дверь
    {
        if (_timer >= 0f)
        {
            // сохранить таймер закрытия
            _saveTimer = _timer;
            _timer = -1f;
        }
        else
        {
            _saveTimer = -1f;
        }
        _doorIsLocked = true;        
    }

    public void SetUnlocked()  // функция разблокирует дверь
    {
        if (_doorIsLocked)
        {

        }
        if (_saveTimer >= 0f)
        {
            // сохранить таймер закрытия
            _timer = _saveTimer;
            _saveTimer = -1f;
        }
        _doorIsLocked = false;        
    }
}

