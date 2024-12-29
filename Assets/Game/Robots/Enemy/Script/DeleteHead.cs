using UnityEngine;

public class DeleteHead : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Delete", 30f);
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
}
