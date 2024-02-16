using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionParent : MonoBehaviour
{
    public string[] prerequisites;
    public string effect;
    public int cost;
    public string name;
    public GameObject npc;
    public NPCVariablesScript npcScript;
    public delegate TResult Func<out TResult>();
    public Func<int> fun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
