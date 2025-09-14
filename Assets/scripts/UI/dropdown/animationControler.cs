using UnityEngine;

public class animationControler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool _isPanalOpen;
    public void TogglePanal()
    {
        _isPanalOpen = !_isPanalOpen;
        animator.SetBool("open", _isPanalOpen);
    }
}
