using UnityEngine;

public class Turret_Controller : MonoBehaviour
{
    public float rotationSpeed = 30f; // Скорость вращения
    public Transform Turret_Base_Up;
    public float _roration_angle;    
    public Transform Turret_Gun_Arm;
    public float _tilt_angle;
    public float _angle_up = 45;
    public float _angle_down = -45;
    public Transform Turret_Gun_Burrel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    void Update()
    {
        // Вращаем объект вокруг вертикальной оси (оси Y)
        //transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        Turret_Base_Up.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        Turret_Gun_Arm.Rotate(Vector3.left, rotationSpeed / 10f * Time.deltaTime);
        Turret_Gun_Burrel.Rotate(Vector3.up, rotationSpeed * 10f * Time.deltaTime);
    }
}
