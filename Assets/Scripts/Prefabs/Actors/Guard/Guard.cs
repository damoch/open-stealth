using System.Collections;
using Assets.Scripts.Modules;
using Assets.Scripts.Prefabs.WorldLogic;
using Assets.Scripts.Prefabs.WorlsObjects;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Prefabs.Actors.Guard
{
    public class Guard : MonoBehaviour
    {
        private bool _evasing;
        private IEnumerator _evasionHandle;
        public FieldOfView FieldOfView;
        private Light _fieldOfViewIndicator;
        private RoomManager _roomManager;
        private GameObject _model;
        private NavMeshAgent _navAgent;
        public NavigationPoint NavPoint;
        private Vector3 _playerLastKnownPosition;
        public GameObject Projectile;
        private Rifle _rifle;
        public GuardState State { get; set; }

        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _fieldOfViewIndicator = transform.GetChild(1).GetComponent<Light>();
            FieldOfView = transform.GetChild(2).GetComponent<FieldOfView>();
            _model = transform.GetChild(0).gameObject;
            _rifle = transform.GetChild(4).GetComponent<Rifle>();
            _roomManager = GameObject.FindGameObjectWithTag("Controller").GetComponent<RoomManager>();
            _evasionHandle = Evasion();

            GoToWaypoint();

            _evasing = true;
            SetCalm(transform.position);

            StartCoroutine("TriggerControl");
        }

        private void Update()
        {
            if (GameController.GameState.Equals(GameState.Running))
                switch (State)
                {
                    case GuardState.Calm:
                        if (NavPoint != null && _navAgent.remainingDistance < 0.5f)
                        {
                            NavPoint = NavPoint.Next;
                            GoToWaypoint();
                        }

                        if (FieldOfView.PlayerInRange)
                            SearchFieldOfVision();
                        break;

                    case GuardState.Suspicious:
                        if (FieldOfView.PlayerInRange)
                            SearchFieldOfVision();
                        if (_navAgent.remainingDistance < 0.5f)
                        {
                            _evasionHandle = Evasion();
                            StartCoroutine(_evasionHandle);
                        }
                        break;

                    case GuardState.Alerted:
                        if (!FieldOfView.PlayerInRange)
                            _roomManager.CheckForOtherGuardsState();
                        else
                            _navAgent.Stop();
                        break;
                    default:
                        break;
                }
        }

        private void SearchFieldOfVision()
        {
            RaycastHit hit;
            var origin = new Vector3(_model.transform.position.x, _model.transform.position.y, transform.position.z);
            var ray = new Ray(origin, RoomManager.Player.transform.position - origin);
            Debug.DrawRay(origin, RoomManager.Player.transform.position - origin);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                if (hit.collider.tag.Equals("PlayerObject"))
                {
                    if (_evasing)
                    {
                        StopCoroutine(_evasionHandle);
                        _evasing = false;
                    }
                    if (!State.Equals(GuardState.Alerted))
                        SetAlert(hit.collider.gameObject.transform.position);
                }
        }

        public void SetCalm(Vector3 position)
        {
            State = GuardState.Calm;
            _fieldOfViewIndicator.color = Color.green;
            GoToPosition(position);
        }

        public void SetSuspicious(Vector3 position, bool isGlobal)
        {
            if (!isGlobal)
            {
                StartCoroutine(_evasionHandle);
            }
            GoToPosition(position);
            State = GuardState.Suspicious;
            _fieldOfViewIndicator.color = Color.yellow;
        }

        public void SetAlert(Vector3 alertPosition)
        {
            _playerLastKnownPosition = alertPosition;
            State = GuardState.Alerted;
            _fieldOfViewIndicator.color = Color.red;
            GoToPosition(alertPosition);
            _roomManager.CheckForOtherGuardsState();
        }

        private void GoToWaypoint()
        {
            if (!State.Equals(GuardState.Suspicious) && NavPoint != null)
            {
                GoToPosition(NavPoint.GetPosition());
            }
        }

        private void GoToPosition(Vector3 position)
        {
            _navAgent.SetDestination(position);
            _navAgent.Resume();
        }

        private IEnumerator Evasion()
        {
            _evasing = true;
            yield return new WaitForSeconds(10f);
            SetCalm(NavPoint != null ? NavPoint.transform.position : transform.position);
        }

        private IEnumerator TriggerControl()
        {
            while (true)
            {
                if (State.Equals(GuardState.Alerted))
                {
                    transform.LookAt(RoomManager.Player.transform);
                    _rifle.ShootBullet();
                }
                yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
            }
        }


    }
}