using UnityEngine;
using Gameplay.Interactions;
using Gameplay.Systems; 
public class MirrorController : MonoBehaviour {
    private Interactable _interactable;
    public LaserController _laserController { get; private set; }
    private Animator _animator;


    void Awake()
    {
        _interactable = GetComponent<Interactable>();
        if (_interactable != null)
        {
            _interactable.OnInteract += Interact;
        }

        _laserController = GetComponent<LaserController>();
        _animator = GetComponent<Animator>();
    }
    private void Interact(CharacterInteractor interactor)
    {
        if (_animator != null)
        {
            _animator.SetTrigger("rotate"); 
        }
    }



}