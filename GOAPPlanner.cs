using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GOAPPlanner : MonoBehaviour
{
    public GameObject[] actionParents;
    public GOAPActionParent[] actions;
    public string[] gameState;
    public GOAPActionParent[] actionPlan;
    int actionPlanIndex=0;

    // Start is called before the first frame update
    void Start()
    {
        actions=new GOAPActionParent[actionParents.Length];
        for (int i=0; i<actionParents.Length; i++) {
            actions[i]=actionParents[i].GetComponent<GOAPActionParent>();
        }

        //planActions(actions[0], new GOAPActionParent[100], 0);
        GameObject.Find("Enemy Pirate (5)").GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(GameObject.Find("Target").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] planActions(GOAPActionParent action, GOAPActionParent[] actionsCalled, int actionsIndex) {
        print(action.name);
        int[] costs=new int[100];
        string[] plans=new string[100];
        for (int i=0; i<costs.Length; i++) {
            costs[i]=0;
            plans[i]="";
        }
        int currentIndex=0;

        if (action.prerequisites.Length<1) {
            print("Cost: "+action.cost.ToString());
            return new string[] {action.cost.ToString(), action.name};
        }
        for (int j=0; j<actionsCalled.Length; j++) {
            if (action==actionsCalled[j]) {
                print("Cost: "+action.cost.ToString());
                return new string[] {action.cost.ToString(), action.name};
            }
        }

        actionsCalled[actionsIndex]=action;
        actionsIndex++;

        bool inDict=false;
        bool con;
        string[] act;
        for (int x=0; x<action.prerequisites.Length; x++) {
            con=true;
            for (int y=0; y<gameState.Length; y++) {
                if (action.prerequisites[x]==gameState[y]) {
                    costs[currentIndex]=actions[statesIndex(action.prerequisites[x])].cost;
                    plans[currentIndex]=actions[statesIndex(action.prerequisites[x])].name;
                    currentIndex++;
                    con=false;
                    break;
                }
            }
            if (con) {
                for (int z=0; z<gameState.Length; z++) {
                    if (states(action.prerequisites[x])==gameState[z]) {
                        inDict=true;
                        costs[currentIndex]=actions[statesIndex(action.prerequisites[x])].cost;
                        plans[currentIndex]=actions[statesIndex(action.prerequisites[x])].name;
                        currentIndex++;
                        if (actions[statesIndex(action.prerequisites[x])].prerequisites.Length>0) {
                            act=planActions(actions[statesIndex(action.prerequisites[x])], actionsCalled, actionsIndex);
                            costs[x]+=Int32.Parse(act[0]);
                            plans[x]+=" ";
                            plans[x]+=act[1];
                        }
                        break;
                    }
                }
                if (!inDict) {
                    costs[currentIndex]=-1;
                    currentIndex++;
                }
            }
        }

        int minCost=100000;
        int minCostIndex=0;
        string minCostPlan="";
        print("{");
        for (int ads=0; ads<currentIndex; ads++) {
            print("Costs: "+costs[ads].ToString());
        }
        print("}");
        for (int a=0; a<currentIndex; a++) {
            if (costs[a]<minCost && costs[a]>=0) {
                minCost=costs[a];
                minCostIndex=a;
                minCostPlan=plans[a];
            }
        }
        print("Action: "+actions[statesIndex(action.prerequisites[minCostIndex])].name);
        print("Min cost: "+minCost.ToString());
        print("Min cost index: "+minCostIndex.ToString());
        print("Plan: "+minCostPlan);
        return new string[] {minCost.ToString(), minCostPlan};
    }

    public string states(string state) {
        switch(state) {
            case "reached target":
                return "target not in range";

            case "target defeated":
                return "target alive";

            case "patrol completed":
                return "patrol not finished";

            case "idle":
                return "not idle";

            case "path clear":
                return "path blocked";

            case "action performed":
                return "no action";

            case "found target":
                return "target lost";

        }
        return "";
    }

    public int statesIndex(string state) {
        switch(state) {
            case "reached target":
                return 0;

            case "target defeated":
                return 1;
                
            case "patrol completed":
                return 2;
                
            case "idle":
                return 3;
                
            case "path clear":
                return 4;
                
            case "action performed":
                return 5;
                
            case "found target":
                return 6;

            case "target not in range":
                return 0;


            case "target alive":
                return 1;

            case "patrol not finished":
                return 2;

            case "not idle":
                return 3;

            case "path blocked":
                return 4;

            case "no action":
                return 5;

            case "target lost":
                return 6;
                
        }
        return 0;
    }

    public void parseActions(string[] plan) {
        actionPlan=new GOAPActionParent[100];
        string planString=plan[1];
        string word="";
        for (int x=0; x<planString.Length; x++) {
            if (planString[x]!=' ') {
                word+=planString[x];
            }
            else {
                for (int y=0; y<actions.Length; y++) {
                    if (word==actions[y].name) {
                        actionPlan[actionPlanIndex]=actions[y];
                        actionPlanIndex++;
                        word="";
                    }
                }
            }
        }
        for (int z=0; z<actions.Length; z++) {
                    if (word==actions[z].name) {
                        actionPlan[actionPlanIndex]=actions[z];
                        actionPlanIndex++;
                        word="";
                    }
                }
        for (int a=0; a<actionPlanIndex; a++) {
            print(actionPlan[a].name);
        }

        executeActions();
    }

    public void executeActions() {
        print("Hi");
        for (int x=0; x<actionPlanIndex; x++) {
            print("men");
            actionPlan[x].fun();
        }
    }
}
