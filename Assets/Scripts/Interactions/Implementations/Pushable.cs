using UnityEngine;
using Gameplay.Interactions;
using Gameplay.Systems;

public class Pushable : MonoBehaviour, IDamageable<DamageData>

{
    private Vector3 originalPosition;
    [SerializeField] private float cellSize = 1f;
    public DamageSource pusher;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        originalPosition = transform.position;
    }
    public void TakeDamage(DamageData info)
    {
        if ( pusher == null)
        {
            Debug.LogError($"{this} [Pusheable] falta pusher");
            return;
        }
        if (info.Source != pusher)
        {
            return;
        }
        Vector3 pushDirection = GetGridDirection(info.AttackDirection);
        Vector3 targetPosition = transform.position + (pushDirection * cellSize);

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayOrigin, pushDirection, out RaycastHit hit, cellSize))
        {
            if (!hit.collider.isTrigger)
            {
                Debug.Log($"{this}[pushable] bloqueado por: {hit.collider.name}");
                return;
            }
        }

        transform.position = targetPosition;
        Debug.Log($"{this}[pushable] empujado hacia {pushDirection}");
        Debug.Log($"{this}[pushable] posición con respecto a la original {transform.position - originalPosition}");

    }

    private Vector3 GetGridDirection(Vector3 attackDir)
    {
        if (Mathf.Abs(attackDir.x) > Mathf.Abs(attackDir.z))
        {
            return new Vector3(Mathf.Sign(attackDir.x), 0, 0);
        }
        else
        {
            return new Vector3(0, 0, Mathf.Sign(attackDir.z));
        }
    }
}
