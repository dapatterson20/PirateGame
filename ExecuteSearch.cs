using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteSearch : GOAPActionParent
{
    
    // Start is called before the first frame update
    void Start()
    {
        fun=new Func<int>(action);
        npcScript=npc.GetComponent<NPCVariablesScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int action() {
        print(name);
        if (npcScript.startCounting) {
            InvokeRepeating("SearchTimer",0,1);
            npcScript.startCounting=false;
        }
        if (npcScript.beginSearch) {
            npcScript.beginSearch=false;
            InvokeRepeating("ValidTimer",0,1);
            UpdateSearchDestination();
        }
        if (Vector3.Distance(npcScript.transform.position, npcScript.target)<1) {
            UpdateSearchDestination();
            npcScript.validCheckSeconds=0;
            CancelInvoke("ValidTimer");
            InvokeRepeating("ValidTimer",0,1);
        }
        return 0;
    }

    public void UpdateSearchDestination() {
        //Move to waypoint
        //target=searchWaypoints[searchWaypointIndex].position;
        Vector3 rndPoint=Random.insideUnitCircle * 5;
        npcScript.target=npcScript.transform.position + new Vector3(rndPoint.x, 0, rndPoint.y);
        npcScript.agent.SetDestination(npcScript.target);
        npcScript.gameObject.GetComponent<Animator>().SetFloat("VInput", 1.0f);
    }

    public void SearchTimer() {
        print("searching...");
        if (npcScript.secondsSearched<npcScript.maxSearchSeconds) {
            npcScript.secondsSearched++;
        }
        else {
            npcScript.secondsSearched=0;
            CancelInvoke("SearchTimer");
            npcScript.seenPlayer=false;

        }
    }

    public void ValidTimer() {
        if (npcScript.validCheckSeconds>=2) {
            UpdateSearchDestination();
            CancelInvoke("ValidTimer");
        }
        else {
            npcScript.validCheckSeconds++;
        }
    }
}
