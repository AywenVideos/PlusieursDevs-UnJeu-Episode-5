using UnityEngine;
using UnityEngine.AI;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public abstract class EntitySubject : MonoBehaviour, IEntityConsumable
{

    public string EntityName;
    public enum State { Idle, Walk, GetFood, Scream }
    protected State currentState;
    protected NavMeshAgent agent;
    private float idleTimer;
    [SerializeField] private float minIdleDuration = 3f;
    [SerializeField] private float maxIdleDuration = 10f;
    private Animator animator;

    [SerializeField] private Transform head;
    private float targetHeadRotation;
    [SerializeField]  private float headRotationSpeed = 2f;
    private float headLookTimer;
    [SerializeField] private float headLookInterval = 2.5f;
    [SerializeField] private float grabbingFoodTimeDuration = 2f;
    private float grabbingFoodTime = 0f;

    private bool isGathering = false;
    private Vector3 foodSourcePosition;
    private int foodPoints = 0;
    private bool needsToReturnToLeader = false;

    public bool IsLeader = false;

    private EntityPack entityPack;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float leaderInfluenceDistance = 30f;
    [SerializeField] private int entityConsumableScore;
    private int personalQuota = 10;

    [SerializeField] private Transform depositPoint;
    [SerializeField] private float depositRange = 2f;

    [SerializeField] private float explorationRadius = 50f;
    [SerializeField] private float minExplorationRadius = 20f;
    [SerializeField] private int explorationAttempts = 5;
    [SerializeField] private float minEntitySeparation = 5f;
    [SerializeField] private float maxWanderDistance = 100f;
    [SerializeField] private float depositDistance = 2f;
    [SerializeField] private float neutralizedTime = 2f;

    public IEntityConsumable foodTarget;

    EntityInfo entityInfo;


    private float neutralized;
    public bool Neutralized
    {
        get
        {
            return neutralized > 0f;
        }
        set
        {
            neutralized = value ? neutralizedTime : 0f;
            entityInfo.SetNeutralized(Neutralized);

            if (Neutralized)
            {
                ResetHeadRotation();
            }
        }
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = State.Idle;
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.avoidancePriority = UnityEngine.Random.Range(40, 80);
        agent.radius = 0.3f;
        agent.stoppingDistance = 0.1f;
        agent.autoRepath = true;

        var obj = Resources.Load<GameObject>("Entity Info");
        if (obj != null)
        {
            var go = Instantiate(obj, transform);
            var _entityInfo = go.GetComponent<EntityInfo>();

            if (_entityInfo)
            {
                entityInfo = _entityInfo;
                entityInfo.Init(EntityName, entityConsumableScore, agent.height + 1f);
            }
        }

        SetRandomIdleTime();

        SetLeader(false);
        EntityGroup.RegisterEntity(this);
        entityPack = EntityGroup.FindEntityPack(this);

        ConsumableManager.Instance.RegisterConsumable(this);

        Neutralized = false;
    }


    protected virtual void Update()
    {
        if (Neutralized)
        {
            neutralizedTime -= Time.deltaTime;

            if (!Neutralized)
            {
                Neutralized = false;
            }

            return;
        }

        idleTimer -= Time.deltaTime;
        grabbingFoodTime -= Time.deltaTime;

        if (isGathering)
        {
            ConsumableManager.Instance.CheckForConsumption(this);
            if (foodTarget == null)
            {
                isGathering = false;
            }
        }
        else if (needsToReturnToLeader)
        {
            ReturnToLeader();
            CheckForDeposit();
        }
        else
        {
            switch (currentState)
            {
                case State.Idle:
                    LookAround();
                    if (idleTimer <= 0)
                    {
                        FindFoodSource();
                        SetRandomIdleTime();
                    }
                    break;
                case State.Walk:
                    ResetHeadRotation();
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        currentState = State.Idle;
                        SetRandomIdleTime();
                    }
                    break;
                case State.GetFood:
                    if (grabbingFoodTime <= 0f)
                    {
                        currentState = State.Idle;
                        SetRandomIdleTime();
                        if (foodTarget != null)
                        {
                            AddFoodPoints(foodTarget.GetFoodValue());
                            foodTarget.Consume();
                            ConsumableManager.Instance.UnregisterConsumable(foodTarget);

                            needsToReturnToLeader = foodPoints >= personalQuota;

                            foodTarget = null;
                        }
                    }
                    break;
            }
        }

        UpdateAnimation();
        HandleMovement();
    }

    float? lastRot;
    private void LateUpdate()
    {
        if (head != null)
        {
            Vector3 eulerAngles = head.localEulerAngles;
            eulerAngles.y = Mathf.LerpAngle(lastRot ?? eulerAngles.y, targetHeadRotation, Time.deltaTime * headRotationSpeed);
            lastRot = eulerAngles.y;
            head.localEulerAngles = eulerAngles;
        }
    }

    private void SetRandomIdleTime()
    {
        idleTimer = UnityEngine.Random.Range(minIdleDuration, maxIdleDuration);
    }

    private void MoveTo(Vector3 destination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            currentState = State.Walk;
        }
    }

    public void ForceSetState(State state)
    {
        currentState = state;
    }

    protected void StartWalking()
    {
        Debug.Log($"Entity [{name}] start walking");

        Vector3 bestDestination = transform.position;
        float bestDistance = 0f;
        bool validDestinationFound = false;

        for (int i = 0; i < explorationAttempts; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(minExplorationRadius, explorationRadius);
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, explorationRadius, NavMesh.AllAreas))
            {
                float distanceFromStart = Vector3.Distance(transform.position, hit.position);
                if (distanceFromStart > bestDistance && IsPositionValid(hit.position))
                {
                    bestDestination = hit.position;
                    bestDistance = distanceFromStart;
                    validDestinationFound = true;
                }
            }
        }

        if (validDestinationFound)
        {
            MoveTo(bestDestination);
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        return true;
    }

    public void AddFoodPoints(int amount)
    {
        foodPoints += amount;

        if (entityInfo) entityInfo.SetPoints(amount);
    }
    private void FindFoodSource()
    {
        IEntityConsumable bestFood = ConsumableManager.Instance.LowestDistanceFrom(this, 20f);
        int bestScore = 0;

        if (bestFood != null && bestFood.GetFoodValue() < entityConsumableScore) // NE METTEZ PAS <= OU ILS VONT SENTREBOUFFER
        {
            foodTarget = bestFood;
            foodSourcePosition = ((MonoBehaviour)foodTarget).transform.position;
            MoveTo(foodSourcePosition);
            isGathering = true;
            needsToReturnToLeader = foodPoints + bestScore >= personalQuota;
            if (needsToReturnToLeader)
            {
                Debug.Log($"Entity {EntityName} need to return to his leader.");
            }
        }
        else
        {
            StartWalking();
        }
    }

    private void ReturnToLeader()
    {
        if (entityPack != null && entityPack.Leader != null)
        {
            MoveTo(entityPack.Leader.transform.position);
        }
    }


    private void DepositFood()
    {
        if (entityPack != null)
        {
            entityPack.AddFoodPoints(foodPoints);
            AddFoodPoints(-foodPoints);
            needsToReturnToLeader = false;

            int newGroupQuota = entityPack.CalculateGroupQuota();
            Debug.Log($"[EntitySubject] Nouveau quota du groupe: {newGroupQuota}");

            if (entityPack.TotalFoodPoints >= newGroupQuota)
            {
                Debug.Log($"[EntitySubject] Quota atteint, arrêt de la collecte !");
            }
            else
            {
                FindFoodSource();
            }
        }
    }

    private void CheckForDeposit()
    {
        if (entityPack != null && entityPack.Leader != null)
        {
            if (Vector3.Distance(transform.position, entityPack.Leader.transform.position) <= depositDistance)
            {
                DepositFood();
            }
        }
    }

    private void LookAround()
    {
        headLookTimer -= Time.deltaTime;

        if (head != null && headLookTimer <= 0f)
        {
            targetHeadRotation = UnityEngine.Random.Range(-40f, 40f);
            headLookTimer = headLookInterval;
        }
    }

    public void GetFood()
    {
        isGathering = false;
        grabbingFoodTime = grabbingFoodTimeDuration;
        currentState = State.GetFood;
    }

    private void ResetHeadRotation()
    {
        if (head != null)
        {
            targetHeadRotation = 0f;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("getfood", currentState == State.GetFood);
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void HandleMovement()
    {
        transform.position = agent.nextPosition;

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public int GetFoodValue() => entityConsumableScore;

    public virtual void Consume()
    {
        Debug.Log($"Entity [{name}] is dead.");
        Destroy(gameObject);
    }

    public void SetLeader(bool isLeader)
    {
        if (!isLeader && IsLeader)
        {
            Debug.LogWarning("Tentative de changement de leader");
        }

        if (isLeader) Debug.Log($"New Leader ! {name}");
        this.entityInfo.SetLeader(isLeader);
        IsLeader = isLeader;
    }

    public void Neutralize()
    {
        Neutralized = true;
    }
}
