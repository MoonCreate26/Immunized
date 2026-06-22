using UnityEngine;

public class UISlideControl : MonoBehaviour
{
    public Animator slideUI;

    public void FlipCurrentBool()
    {
        slideUI.SetBool("Open", !slideUI.GetBool("Open"));
    }

    public void SetCurrentBool(bool Open)
    {
        slideUI.SetBool("Open", Open);
    }
}
