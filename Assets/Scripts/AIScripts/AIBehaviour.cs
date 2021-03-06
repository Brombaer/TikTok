using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    private enum MovementType
    {
        Random,
        Waypoint
    };

    private GameObject _playerReference;
    [SerializeField] private float _fieldOfView = 120;
    [SerializeField] private float _viewDistance = 15;
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private GameObject[] _skins;
    [SerializeField] private Transform _rigRoot;
    
    public int AttackDamage;
    [SerializeField] private float _attackRange = 1;
    
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _chaseSpeed = 2;

    [SerializeField] private float _looseThreshold = 10;
    [SerializeField] private float _moveRadius = 3;
    [SerializeField] private MovementType _movementType = MovementType.Random;

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Transform _head;

    private bool _isAware = false;
    private bool _isDetecting = false;
    private float _looseTimer = 0;
    
    private Animator _animator;
    private int _currentWaypointIndex = 0;
    private Vector3 _movePosition;
    private NavMeshAgent _agent;
    
    [SerializeField] private float _timer = 60;
    [SerializeField] private float _currentTime;
    [SerializeField] private float _preparationTime = 10;
    [SerializeField] private float _awarenessFactor = 1.5f;

    private bool _isPreparationTimeOver = false;
    private SphereCollider _sphereCollider;

    [SerializeField] private GameObject _leftHandFist;
    [SerializeField] private GameObject _rightHandFist;

    private int _skinIndex;
    FMOD.Studio.EventInstance ZombieAudio;
    FMOD.Studio.EventInstance DeathHit;
    private int maleVoice;
    private int femVoice;
    private static readonly int isAware = Animator.StringToHash("isAware");
    private static readonly int attack = Animator.StringToHash("Attack");

    public void Start()
    {
        AttackDamage = Random.Range(10, 20);
        
        _playerReference = GameObject.Find("Character/Root/Hips/Spine_01");
        _head = transform.Find("RaycastStart");
        
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _movePosition = RandomMovePosition();

        _sphereCollider = GetComponent<SphereCollider>();

        _leftHandFist = gameObject.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L").gameObject;
        _rightHandFist = gameObject.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R").gameObject;

        if (_skins != null)
        {
            _skins[0].SetActive(false);

            _skinIndex = Random.Range(0, _skins.Length);
            _skins[_skinIndex].SetActive(true);
        }

        maleVoice = Random.Range(0, 5);
        femVoice = Random.Range(6, 14);
        SetZombieVoice();
        _currentTime = _timer;
    }

    public void Update()
    {
        if (_health <= 0)
        {
            Die();
            DeathAudio();
            return;
        }
        
        ChasePlayer();

        _currentTime -= 1 * Time.deltaTime;
        _preparationTime -= 1 * Time.deltaTime;

        if (_timer != 0 || _preparationTime != 0)
        {
            if (_preparationTime <= 0 && _isPreparationTimeOver != true)
            {
                ChangeAwareness();
            }
        }
    }

    private void ChangeAwareness()
    {
        _preparationTime = 0;
        _isPreparationTimeOver = true;
        _sphereCollider.radius *= _awarenessFactor;
    }

    private void ChasePlayer()
    {
        SearchForPlayer();
        
        if (_isAware)
        {
            _agent.SetDestination(_playerReference.transform.position);
            _animator.SetBool(isAware, true);
            _agent.speed = _chaseSpeed;

            if (_agent.remainingDistance <= _attackRange)
            {
                GetComponent<Animator>().SetTrigger(attack);
            }
            
            if (_isDetecting == false)
            {
                _looseTimer += Time.deltaTime;

                if (_looseTimer >= _looseThreshold)
                {
                    _isAware = false;
                    _looseTimer = 0;
                }
            }
        }
        else
        {
            Move();
            _animator.SetBool(isAware, false);
            _agent.speed = _walkSpeed;
        }
    }

    private void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(_playerReference.transform.position)) < _fieldOfView / 2)
        {
            if (Vector3.Distance(_playerReference.transform.position, transform.position) < _viewDistance)
            {
                RaycastHit hit;
                
                if (Physics.Linecast(_head.transform.position, _playerReference.transform.position, out hit, -1))
                {
                    Debug.DrawLine(_head.transform.position, _playerReference.transform.position, Color.magenta);
                    
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        _isDetecting = false;
                    }
                }
                else
                {
                    _isDetecting = false;
                }
            }
            else
            {
                _isDetecting = false;
            }
        }
        else
        {
            _isDetecting = false;
        }
    }

    private void Move()
    {
        if (_movementType == MovementType.Random)
        {
            if (Vector3.Distance(transform.position, _movePosition) < 2)
            {
                _movePosition = RandomMovePosition();
            }
            else
            {
                _agent.SetDestination(_movePosition);
            }
        }
        else
        {
            if (_waypoints.Length >= 2)
            {
                if (Vector3.Distance(_waypoints[_currentWaypointIndex].position, transform.position) < 2)
                {
                    if (_currentWaypointIndex == _waypoints.Length - 1)
                    {
                        _currentWaypointIndex = 0;
                    }
                    else
                    {
                        _currentWaypointIndex++;
                    }
                }
                else
                {
                    _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
                }
            }
            else
            {
                Debug.LogWarning("Use at least 2 waypoints for the AI's movement" + gameObject.name);
            }
        }
    }

    public void OnHit(int damage)
    {
        _health -= damage;
        if (damage != 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Zombies/WeaponHit", GetComponent<Transform>().position);
        }
    }

    public void activateFists()
    {
        _leftHandFist.GetComponent<Collider>().enabled = true;
        _rightHandFist.GetComponent<Collider>().enabled = true;
    }

    public void deactivateFists()
    {
        _leftHandFist.GetComponent<Collider>().enabled = false;
        _rightHandFist.GetComponent<Collider>().enabled = false;
    }

    public void Die()
    {
        _agent.speed = 0;
        _animator.enabled = false;

        if (_ragdoll != null)
        {
            var ragdoll = Instantiate(_ragdoll, transform.position, transform.rotation);
            ragdoll.GetComponent<Ragdoll>().SetSkin(_skinIndex);
            
            Debug.Break();
            ragdoll.GetComponent<Ragdoll>().MatchRig(_rigRoot);
            Destroy(ragdoll, 10f);
        }
        
        Destroy(gameObject);
    }

    private Vector3 RandomMovePosition()
    {
        Vector3 randomPosition = (Random.insideUnitSphere * _moveRadius) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomPosition, out navMeshHit, _moveRadius, -1);
        
        return new Vector3(navMeshHit.position.x, transform.position.y, navMeshHit.position.z);
    }

    public  void OnAware()
    {
        _isAware = true;
        _isDetecting = true;
        _looseTimer = 0;
    }

    //FMODAudio



    public void SetZombieVoice()
    {

        if (_skinIndex <= 15)
        {
            ZombieAudio.setParameterByName("Zombie#", maleVoice);
        }
        else
        {
            ZombieAudio.setParameterByName("Zombie#", femVoice);
        }
    }


    public void DeathAudio()
    {
        DeathHit = FMODUnity.RuntimeManager.CreateInstance("event:/Zombies/Death");
        
        if (_skinIndex <= 15)
        {
            DeathHit.setParameterByName("Zombie#", maleVoice);
        }
        else
        {
            DeathHit.setParameterByName("Zombie#", femVoice);
        }
        
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(DeathHit, transform, GetComponent<Rigidbody>());
        DeathHit.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        DeathHit.start();
        DeathHit.release();
    }

    public void Roaming()
    {
        ZombieAudio = FMODUnity.RuntimeManager.CreateInstance("event:/Zombies/Roaming");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ZombieAudio, transform, GetComponent<Rigidbody>());
        ZombieAudio.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        ZombieAudio.start();
        ZombieAudio.release();
    }

    private void StopAudio()
    {
        ZombieAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.DetachInstanceFromGameObject(ZombieAudio);
        ZombieAudio.release();
        ZombieAudio.clearHandle();

        DeathHit.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        DeathHit.release();
        DeathHit.clearHandle();
    }
    
    private void OnEnable()
    {
        Roaming();
    }

    private void OnDisable()
    {
        StopAudio();
    }
}
