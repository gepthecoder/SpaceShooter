using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private Animator popUpAnime;

    public void PlayPopUpAnimation(string damage)
    {
        popUpAnime.GetComponent<Text>().text = damage;
        popUpAnime.SetTrigger("damage");
    }

    private void LateUpdate()
    {
        if(popUpAnime != null)
        {
            popUpAnime.transform.parent.LookAt(Camera.main.transform);
            popUpAnime.transform.parent.Rotate(0f, 180f, 0f);
        }
    }
}
