using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : GOAPActionParent
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
        npcScript.moving=false;
        npcScript.engaged=false;
        npcScript.agent.isStopped=true;
        return 0;
    }
}
