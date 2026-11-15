using UnityEngine;
using UnityEngine.AI;
using Gameplay.Character;

public class AIBrain : MonoBehaviour {
    private CharacterMovement _characterMovement;
    private CharacterHabilities _characterHabilities;
    float timeInState;
    Vector3 initialPosition;

    float detectionRadius = 2f; 
    float chaseRadius = 5f;
    float distanceToPlayer = 0.5f;
    float attackPrepare = 0.25f;
    float attackDuration = 2f;

    bool hasAttacked = false;
    bool hasLooked = false;

    NavMeshPath path;
    [SerializeField] Transform target;

    enum AIState {
        Idle, Patrol, Chase, Return, Attack
    }
    private AIState _currentState = AIState.Idle;

    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        if (_characterMovement == null)
            Debug.LogError("[UserInputMovement] CharacterMovement is null! Add a CharacterMovement component to the GameObject.");

        _characterHabilities = GetComponent<CharacterHabilities>();
        if (_characterHabilities == null)
            Debug.LogError("[UserInputMovement] CharacterHabilities is null! Add a CharacterHabilities component to the GameObject.");
    }

    void Start()
    {
        path = new NavMeshPath();
        initialPosition = transform.position;
    }
    void OnDrawGizmos() 
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(initialPosition, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(initialPosition, chaseRadius);


        }
        else 
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
    void Update()
    {
        timeInState += Time.deltaTime;

        float distanceToSpawn = Vector3.Distance(initialPosition, target.position);

        switch (_currentState)
        {
            case AIState.Idle:
                _characterMovement.Move(Vector3.zero);
                if (distanceToSpawn <= detectionRadius)
                {
                    ChangeState(AIState.Chase);
                    _characterMovement.TriggerJump();

                }
                break;
            case AIState.Return:
                if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
                {
                    MoveTo(initialPosition, 0f);
                }
                else
                {
                    ChangeState(AIState.Idle);
                }
                break;
            case AIState.Patrol:
                break;

            case AIState.Chase:
                if (distanceToSpawn > chaseRadius)
                {
                    ChangeState(AIState.Return);

                }
                else
                {
                    MoveTo(target.position, distanceToPlayer);
                    if (Vector3.Distance(transform.position, target.position) <= distanceToPlayer)
                    {
                        ChangeState(AIState.Attack);

                    }
                }
                break;
            case AIState.Attack:


                _characterMovement.Move(Vector3.zero);
                if (!hasLooked) 
                {
                    _characterMovement.LookAt(target.position);
                    hasLooked = true;
                }

                if (timeInState >= attackPrepare)
                {
                    if (!hasAttacked)
                    {

                        _characterHabilities.Attack();
                        hasAttacked = true;
                    }
                }
                if (timeInState > attackDuration) 
                {
                    hasAttacked = false;
                    hasLooked = false;


                    if (distanceToSpawn > chaseRadius)
                    {
                        ChangeState(AIState.Return);

                    }
                    else 
                    {

                        ChangeState(AIState.Chase);


                    }
                }
                break;


        }
    }

    void ChangeState(AIState newState)
    {
        Debug.Log("changing state from "+_currentState+" to "+newState);

        _currentState = newState;
        timeInState = 0;
    }

    void MoveTo(Vector3 targetPosition, float distanceToTarget)
    {
        CalculatePath(targetPosition);
        if (path.corners.Length < 2)
        {
            _characterMovement.Move(Vector3.zero);
            return;
        }

        Vector3 directionToNextCorner = (path.corners[1] - path.corners[0]);
        Vector2 simulatedInput = new Vector2(directionToNextCorner.x, directionToNextCorner.z).normalized;
        Vector3 lookVector = new Vector3(simulatedInput.x, 0, simulatedInput.y);

        if (directionToNextCorner.magnitude > distanceToTarget)
        {
            _characterMovement.Move(simulatedInput);
            _characterMovement.LookAt(lookVector + transform.position);
        }
        /*
        else
        {
            _characterMovement.Move(Vector3.zero);
        }
        */
    }

    void CalculatePath(Vector3 targetPosition)
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
    }
}