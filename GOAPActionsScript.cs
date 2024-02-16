using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionsScript : MonoBehaviour
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

    //GOAP Action Bools to return
    public bool targetDefeated;
    public bool patrolCompleted;
    public bool nothing;
    public bool reachedTarget;
    public bool pathClear;
    public bool actionPerformed;
    public bool foundTarget;

    public bool[] bools;
    public Func<bool>[] funs;
    public bool[][] allPrereqs;
    public Func<bool>[][] allFuncs;
    public int[] allCosts;
    public bool[] funsCalled;
    public Func<bool>[] fnsToExecute;
    public int[][] costs;

    public int[][] test;
    public int fnsCurrentIndex;




    // Start is called before the first frame update
    void Start()
    {
        test=new int[][] {new int[] {1,2},new int[] {1}, new int[] {1,2,3}};
        //print(test[0][1]);
        //print(test[2][2]);
        fun=new Func<bool>(move);

        bools=new bool[] {targetDefeated, patrolCompleted, nothing, reachedTarget, pathClear, actionPerformed, foundTarget};

        funs=new Func<bool>[] {new Func<bool>(attack), new Func<bool>(patrol), new Func<bool>(idle),
            new Func<bool>(move), new Func<bool>(moveObject), new Func<bool>(performAction), new Func<bool>(search)};

        allPrereqs=new bool[][] {new bool[]{reachedTarget}, new bool[]{reachedTarget},
                new bool[]{},new bool[]{pathClear, actionPerformed, targetDefeated, foundTarget},
                new bool[]{reachedTarget},new bool[]{foundTarget, reachedTarget},new bool[]{}};

        allFuncs=new Func<bool>[][] {new Func<bool>[]{new Func<bool>(move)}, new Func<bool>[]{new Func<bool>(move)},new Func<bool>[]{},
                new Func<bool>[]{new Func<bool>(moveObject), new Func<bool>(performAction), new Func<bool>(attack), new Func<bool>(search)},
                new Func<bool>[]{new Func<bool>(move)},
                new Func<bool>[]{new Func<bool>(search), new Func<bool>(move)},
                new Func<bool>[]{}};
        
        allCosts=new int[] {1,1,1,1,1,1,1};

        funsCalled=new bool[] {false, false, false, false, false, false, false};

        costs=new int[][] {new int[]{1}, new int[] {1}, new int[] {}, new int[]{1,1,1,1}, new int[]{1}, new int[]{1,1}, new int[]{}};

        fnsToExecute=new Func<bool>[100];
        fnsCurrentIndex=0;

        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        target=GameObject.Find("Target").transform.position;
        cutlassScript=cutlass.GetComponent<CannonBallScript>();
        //fun();
        planActions(funs[3], costs[3], allPrereqs[3], allFuncs[3], funsCalled, 1);
        executeActions();
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

    public int planActions(Func<bool> fn, int[] optionCosts, bool[] prereqs, Func<bool>[] fns, bool[] funsCalled, int fnCost) {
        print("planned");
        ///*
        fnsToExecute[fnsCurrentIndex]=fn;
        fnsCurrentIndex++;
        if (prereqs.Length==0) {
            return 1;
        } 
        for (int i=0; i<optionCosts.Length; i++) {
            optionCosts[i]=0;
        }
        for (int x=0; x<prereqs.Length; x++) {
            optionCosts[x]+=fnCost;
            if (!prereqs[x]) {
                for (int y=0; y<funs.Length; y++) {
                    if (fns[x]==funs[y]) {
                        if (funsCalled[y]) {
                            optionCosts[x]+=allCosts[y];
                            break;
                        }
                        else {
                            funsCalled[y]=true;
                            optionCosts[x]+=planActions(funs[y], costs[y], allPrereqs[y], allFuncs[y], funsCalled, allCosts[y]);
                            break;
                        }
                    }
                }
            }
        }
        //*/
        int minCost=0;
        int minCostIndex=0;
        for (int a=0; a<optionCosts.Length; a++) {
            if (a==0) {
                minCost=optionCosts[0];
            }
            else {
                if (optionCosts[a]<minCost) {
                    minCost=optionCosts[a];
                    minCostIndex=a;
                }
            }
        }
        fnsToExecute[fnsCurrentIndex]=fns[minCostIndex];
        fnsCurrentIndex++;
        print("Min cost: "+minCost);
        return minCost;
    }

    public void executeActions() {
        for (int x=0; x<fnsCurrentIndex; x++) {
            fnsToExecute[x]();
        }
    }

    /*   GOAP Actions    */

    public bool attack() {
        print("attack");
        int cost=1;
        bool[] prereqs={reachedTarget};
        Func<bool>[] fns={new Func<bool>(move)};
        if (Vector3.Distance(transform.position, enemy.transform.position)<1.3) {
            currentRot=transform.rotation;
            transform.LookAt(enemy.transform.position);
            transform.rotation=Quaternion.Euler(currentRot.x, transform.eulerAngles.y, currentRot.z);
            GetComponent<Animator>().SetBool("Attack",true);
            moving=false;
            engaged=true;
            Invoke("enableDamage",0.54f);
            agent.isStopped=true;
            Invoke("enableMovement", 1.667f);
        }
        else {
            engaged=false;
            agent.isStopped=false;
            enableMovement();
        }
        return targetDefeated;
    }

    public bool idle() {
        print("idle");
        int cost=1;
        bool[] prereqs={};
        Func<bool>[] fns={};

        moving=false;
        engaged=false;
        agent.isStopped=true;
        return nothing;
    }

    public bool move() {
        print("move");
        int cost=1;
        bool[] prereqs={pathClear, actionPerformed, targetDefeated, foundTarget};
        Func<bool>[] fns={new Func<bool>(moveObject), new Func<bool>(performAction), new Func<bool>(attack), new Func<bool>(search)};

        moving=true;
        engaged=false;
        agent.isStopped=false;
        agent.SetDestination(target);
        return reachedTarget;
    }

    public bool patrol() {
        print("patrol");
        int cost=1;
        bool[] prereqs={reachedTarget};
        Func<bool>[] fns={new Func<bool>(move)};

        beginSearch=true;
        //If at a waypoint, update current waypoint index so you can move on to the next waypoint.
        if (Vector3.Distance(transform.position, target)<1) {
            IterateWaypointIndex();
            if (waypoints.Length>1) {
                UpdateDestination();
            }
            else {
                GetComponent<Animator>().SetFloat("VInput",0.0f);
            }
        }
        return patrolCompleted;
    }

    public bool moveObject() {
        print("moveObject");
        int cost=1;
        bool[] prereqs={reachedTarget};
        Func<bool>[] fns={new Func<bool>(move)};
        
        agent.isStopped=true;
        moving=false;
        ob.transform.position=socket.position;
        ob.transform.parent=socket;
        transform.rotation=Quaternion.Euler(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y+90f,
                        transform.eulerAngles.z);
        ob.transform.parent=null;
        Invoke("continueMoving", 0.5f);
        /*
        ob.transform.Translate(Vector3.forward * Time.deltaTime*50);
        transform.rotation=Quaternion.Euler(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y-90f,
                        transform.eulerAngles.z);
        agent.isStopped=false;
        moving=true;
        move();
        */
        return pathClear;
    }

    public bool performAction() {
        print("performAction");
        int cost=1;
        bool[] prereqs={foundTarget, reachedTarget};
        Func<bool>[] fns={new Func<bool>(search), new Func<bool>(move)};

        return actionPerformed;
    }

    public bool search() {
        print("search");
        int cost=1;
        bool[] prereqs={};
        Func<bool>[] fns={};

        if (startCounting) {
            InvokeRepeating("SearchTimer",0,1);
            startCounting=false;
        }
        if (beginSearch) {
            beginSearch=false;
            InvokeRepeating("ValidTimer",0,1);
            UpdateSearchDestination();
        }
        if (Vector3.Distance(transform.position, target)<1) {
            UpdateSearchDestination();
            validCheckSeconds=0;
            CancelInvoke("ValidTimer");
            InvokeRepeating("ValidTimer",0,1);
        }
        return foundTarget;
    }

    /*-------------------*/

    public void OnTriggerEnter(Collider other) {
        if (other.tag=="Object" && moving) {
            ob=other.gameObject;
            moveObject();
        }
    }

    public void continueMoving() {
        ob.transform.Translate(Vector3.forward * Time.deltaTime*100);
        transform.rotation=Quaternion.Euler(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y-90f,
                        transform.eulerAngles.z);
        agent.isStopped=false;
        moving=true;
        ob=null;
        move();
    }

    public void enableMovement() {
        GetComponent<Animator>().SetBool("Attack", false);
        cutlassScript.canDamage=false;
    }
    
    public void enableDamage() {
        cutlassScript.canDamage=true;
    }

    public void UpdateDestination() {
        //Move to waypoint
        target=waypoints[waypointIndex].position;
        agent.SetDestination(target);
        GetComponent<Animator>().SetFloat("VInput", 1.0f);
    }

    public void UpdateSearchDestination() {
        //Move to waypoint
        //target=searchWaypoints[searchWaypointIndex].position;
        Vector3 rndPoint=Random.insideUnitCircle * 5;
        target=transform.position + new Vector3(rndPoint.x, 0, rndPoint.y);
        agent.SetDestination(target);
        GetComponent<Animator>().SetFloat("VInput", 1.0f);
    }

    public void IterateWaypointIndex() {
        //Update waypoint destination, if at last waypoint, go back to first.
        waypointIndex++;
        if (waypointIndex>=waypoints.Length) {
            waypointIndex=0;
        }
    }

    public void IterateSearchWaypointIndex() {
        //Update waypoint destination, if at last waypoint, go back to first.
        searchWaypointIndex++;
        if (searchWaypointIndex==searchWaypoints.Length) {
            searchWaypointIndex=0;
        }
    }

    public void SearchTimer() {
        print("searching...");
        if (secondsSearched<maxSearchSeconds) {
            secondsSearched++;
        }
        else {
            secondsSearched=0;
            CancelInvoke("SearchTimer");
            seenPlayer=false;

        }
    }

    public void ValidTimer() {
        if (validCheckSeconds>=2) {
            UpdateSearchDestination();
            CancelInvoke("ValidTimer");
        }
        else {
            validCheckSeconds++;
        }
    }
}
