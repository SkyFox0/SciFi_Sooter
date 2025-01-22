using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Animator DoorsAnimator;
    public bool _doorIsOpen; // ������� �� ����� ����� ������
    public bool _doorIsLocked; // ������������� �� ����� (����� �� � �������)
    public bool _doorTrigger; // ���� �� � ����� �������

    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;

    public bool _isCloseTimer;  // ���� �� ������ ��������
    public float _timer = -1f;
    private float _saveTimer;
    public float _timeClose = 10f;  // ���� �������� �� �������


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
        // ������ �������� ������

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
            // ������ ������� ��������������� �������� �����
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

    public void CloseAndSetLocked()  // ������� ��������� � ��������� �����
    {
        if (_doorIsOpen)
        {
            // �������� ������������
            CloseDoor();
        }        
        _doorIsLocked = true;
        _timer = -1f;
    }

    public void SetLocked()  // ������� ������ ��������� �����
    {
        if (_timer >= 0f)
        {
            // ��������� ������ ��������
            _saveTimer = _timer;
            _timer = -1f;
        }
        else
        {
            _saveTimer = -1f;
        }
        _doorIsLocked = true;        
    }

    public void SetUnlocked()  // ������� ������������ �����
    {
        if (_doorIsLocked)
        {

        }
        if (_saveTimer >= 0f)
        {
            // ��������� ������ ��������
            _timer = _saveTimer;
            _saveTimer = -1f;
        }
        _doorIsLocked = false;        
    }
}

