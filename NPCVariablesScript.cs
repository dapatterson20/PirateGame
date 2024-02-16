using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVariablesScript : MonoBehaviour
{

    public GameObject[] obs;
    public GameObject[] enemies;
    public string myTag;
    public string enemyTag;
    public int index;
    public bool engaged;
    public bool onPlayerSide;
    public GameObject enemy;
    public bool moving;
    public Quaternion currentRot;

    public GameObject cutlass;
    public CannonBallScript cutlassScript;
    public GameObject flintlock;
    public CannonScript flintlockScript;

    public Animator anims;
    public GameObject ragdollActor;

    public float health;

    //For patrolling
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform[] waypoints;
    public Transform[] searchWaypoints;
    public GameObject[] searchSpots;
    public int waypointIndex;
    public int searchWaypointIndex;
    public Vector3 target;

    //For AI sensing
    public float viewRadius;
    public float viewAngle;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public GameObject player;
    public Vector3 playerTarget;
    public float distanceToTarget;
    public bool seenPlayer;
    public bool beginSearch;
    public int secondsSearched;
    public bool startCounting;
    public int maxSearchSeconds;

    public int validCheckSeconds;

    public Transform socket;
    public delegate TResult Func<out TResult>();
    public Func<bool> fun;
    public string goal;
    public GameObject ob;
    public GameObject objectToPerformOn;

    public bool restartPatrol;

    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        target=GameObject.Find("Target").transform.position;
        cutlassScript=cutlass.GetComponent<CannonBallScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>().velocity != Vector3.zero) {
            GetComponent<Animator>().SetFloat("VInput",1f);
        }
        else {
            GetComponent<Animator>().SetFloat("VInput",0f);
        }
    }
}
