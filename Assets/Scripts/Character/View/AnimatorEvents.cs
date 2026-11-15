using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    CharacterHabilities _characterHabilities;

    void Awake() 
    {
        _characterHabilities = GetComponentInParent<CharacterHabilities>();
    }
    public void CallHurtDamageables() 
    {
        _characterHabilities.HurtDamageables();
    }
}
