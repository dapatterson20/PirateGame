using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : GOAPActionParent
{

    bool patrol;

    // Start is called before the first frame update
    void Start()
    {
        fun=new Func<int>(action);
        npcScript=npc.GetComponent<NPCVariablesScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (npcScript.beginSearch) {
            //If at a waypoint, update current waypoint index so you can move on to the next waypoint.
            if (Vector3.Distance(npcScript.transform.position, npcScript.target)<1) {
                IterateWaypointIndex();
                if (npcScript.waypoints.Length>1 && npcScript.beginSearch) {
                    UpdateDestination();
                }
                else {
                    npcScript.gameObject.GetComponent<Animator>().SetFloat("VInput",0.0f);
                }
            }
        }
    }

    public int action() {
        print("patrol");
        npcScript.beginSearch=true;
        UpdateDestination();
        return 0;
    }

    public void UpdateDestination() {
        //Move to waypoint
        npcScript.target=npcScript.waypoints[npcScript.waypointIndex].position;
        npcScript.agent.SetDestination(npcScript.target);
        npcScript.gameObject.GetComponent<Animator>().SetFloat("VInput", 1.0f);
    }

    public void IterateWaypointIndex() {
        //Update waypoint destination, if at last waypoint, go back to first.
        npcScript.waypointIndex++;
        if (npcScript.waypointIndex>=npcScript.waypoints.Length) {
            npcScript.waypointIndex=0;
            if (!npcScript.restartPatrol) {
                npcScript.beginSearch=false;
            }
        }
        
    }
}
