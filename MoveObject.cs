using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : GOAPActionParent
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
        print("moveObject");      
        npcScript.agent.isStopped=true;
        npcScript.moving=false;
        npcScript.ob.transform.position=npcScript.socket.position;
        npcScript.ob.transform.parent=npcScript.socket;
        npcScript.transform.rotation=Quaternion.Euler(
                        npcScript.transform.eulerAngles.x,
                        npcScript.transform.eulerAngles.y+90f,
                        npcScript.transform.eulerAngles.z);
        npcScript.ob.transform.parent=null;
        Invoke("continueMoving", 0.5f);
        return 0;
    }

    public void continueMoving() {
        npcScript.ob.transform.Translate(Vector3.forward * Time.deltaTime*100);
        npcScript.transform.rotation=Quaternion.Euler(
                        npcScript.transform.eulerAngles.x,
                        npcScript.transform.eulerAngles.y-90f,
                        npcScript.transform.eulerAngles.z);
        npcScript.agent.isStopped=false;
        npcScript.moving=true;
        npcScript.ob=null;
        npcScript.agent.SetDestination(npcScript.target);
    }
}
