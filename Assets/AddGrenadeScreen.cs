using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddGrenadeScreen : MonoBehaviour
{
    [SerializeField] private Image AddGrenadeImage;
    [SerializeField] private TMP_Text AddGrenadeText;
    [SerializeField] private Animator Animator;

    public void AddGrenadeShow(int kol)
    {
        AddGrenadeText.text = "+" + kol.ToString();
        AddGrenadeImage.color = Color.white;
        Animator.SetTrigger("AddGrenade");
        Invoke("AddGrenadeHide", 2f);    }

    public void AddGrenadeHide()
    {
        AddGrenadeImage.color = new Color(1, 1, 1, 0);
        AddGrenadeText.text = "";
    }

/*    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
