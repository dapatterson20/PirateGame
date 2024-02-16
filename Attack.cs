using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : GOAPActionParent
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
        print("attack");
        if (Vector3.Distance(npcScript.transform.position, npcScript.enemy.transform.position)<1.3) {
            npcScript.currentRot=npcScript.transform.rotation;
            npcScript.transform.LookAt(npcScript.enemy.transform.position);
            npcScript.transform.rotation=Quaternion.Euler(npcScript.currentRot.x, npcScript.transform.eulerAngles.y, npcScript.currentRot.z);
            npcScript.gameObject.GetComponent<Animator>().SetBool("Attack",true);
            npcScript.moving=false;
            npcScript.engaged=true;
            Invoke("enableDamage",0.54f);
            npcScript.agent.isStopped=true;
            Invoke("enableMovement", 1.667f);
        }
        else {
            npcScript.engaged=false;
            npcScript.agent.isStopped=false;
            enableMovement();
        }
        return 0;
    }

    public void enableMovement() {
        npcScript.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        npcScript.cutlassScript.canDamage=false;
    }
    
    public void enableDamage() {
        npcScript.cutlassScript.canDamage=true;
    }
}
