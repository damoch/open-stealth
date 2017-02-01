using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

public class GuardController : MonoBehaviour {
    //Ta właściwość jest publiczna, aby była widoczna inspektorze
    public GameObject Projectile;
    public NavigationPoint navPoint;
    NavMeshAgent navAgent;
    Light fieldOfViewIndicator;
    public FieldOfViewController fieldOfView;
    GameObject model;
    Transform Rifle;
    Vector3 playerLastKnownPosition;
    public GuardState state { get; set; }
    IEnumerator evasionHandle;
    private bool evasing;
    GameController gameController;
    public AudioClip shootSfx_1;

    void Start () { 
        //Przypisywanie komponentów
        navAgent = GetComponent<NavMeshAgent>();
        fieldOfViewIndicator = transform.GetChild(1).GetComponent<Light>();
        fieldOfView = transform.GetChild(2).GetComponent<FieldOfViewController>();
        model = transform.GetChild(0).gameObject;
        Rifle = transform.GetChild(4);
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        evasionHandle = Evasion();

        goToWaypoint();

        evasing = true;
        setCalm(transform.position);

        StartCoroutine("triggerControl");
    }

    void Update()
    {
        //Działa na zasadzie maszyny stanowej (automatu skończonego czy jak tam zwał etc.)
        //https://pl.wikipedia.org/wiki/Automat_sko%C5%84czony
        if (GameController.gameState.Equals(GameState.RUNNING))
        {
            switch (state)
            {
                case GuardState.CALM:
                    if (navAgent.remainingDistance < 0.5f)
                    {
                        navPoint = navPoint.next;
                        goToWaypoint();
                    }

                    if (fieldOfView.playerInRange)
                    {
                        searchFieldOfVision();
                    }
                    break;

                case GuardState.SUSPICIOUS:
                    if (fieldOfView.playerInRange)
                    {
                        searchFieldOfVision();
                    }
                    if (navAgent.remainingDistance < 0.5f)
                    {
                        evasionHandle = Evasion();
                        StartCoroutine(evasionHandle);
                    }
                    break;

                case GuardState.ALERTED:
                    if (!fieldOfView.playerInRange)
                    {
                        //setSuspicious(playerLastKnownPosition);
                        gameController.checkForOtherGuardsState();
                    }
                    else
                    {
                        navAgent.Stop();
                    }
                    break;
            }
        }
         
    }

    void searchFieldOfVision()
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(model.transform.position.x, model.transform.position.y, transform.position.z);
        Ray ray = new Ray(origin, GameController.player.transform.position - origin);
        Debug.DrawRay(origin, GameController.player.transform.position - origin);
       
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "PlayerObject")
            {
                if (evasing)
                {
                    StopCoroutine(evasionHandle);
                    evasing = false;
                }
                if (!state.Equals(GuardState.ALERTED))
                {
                    setAlert(hit.collider.gameObject.transform.position);
                }
            }
        }
    }

    public void setCalm(Vector3 position)
    {
        state = GuardState.CALM;
        fieldOfViewIndicator.color = Color.green;
        goToPosition(position);
    }

    public void setSuspicious(Vector3 position, bool isGlobal)
    {
        if (!isGlobal)
        {
            StartCoroutine(evasionHandle);
        }
        goToPosition(position);
        state = GuardState.SUSPICIOUS;
        fieldOfViewIndicator.color = Color.yellow;
    }

    public void setAlert(Vector3 alertPosition)
    {
        playerLastKnownPosition = alertPosition;
        state = GuardState.ALERTED;
        fieldOfViewIndicator.color = Color.red;
        goToPosition(alertPosition);
        gameController.checkForOtherGuardsState();
    }

    private void goToWaypoint()
    {
        if (!state.Equals(GuardState.SUSPICIOUS) && navPoint != null)
        {
            goToPosition(navPoint.getPosition());
        }
    }

    private void goToPosition(Vector3 position)
    {
        navAgent.SetDestination(position);
        navAgent.Resume();
    }

    IEnumerator Evasion()
    {
        evasing = true;
        yield return new WaitForSeconds(10f);
        setCalm(navPoint.transform.position);        
    }

    IEnumerator triggerControl()
    {
        while (true)
        {
            if (state.Equals(GuardState.ALERTED))
            {
                transform.LookAt(GameController.player.transform);
                shootBullet();
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.7f));
        }
    }

    private void shootBullet()
    {
        var bullet = (GameObject)Instantiate(Projectile,
            Rifle.position,
            Rifle.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        SoundManager.instance.PlaySingle(shootSfx_1);
        Destroy(bullet, 2.0f);
    }
}