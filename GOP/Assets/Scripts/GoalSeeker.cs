using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeeker : MonoBehaviour
{
    public UI uiManager;
    public static string goalString = "";


    Goal[] mGoals;
    Action[] mActions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 5.0f;
    void Start()
    {
        mGoals = new Goal[3];
        mGoals[0] = new Goal("Speed", 1);
        mGoals[1] = new Goal("Fuel", 50);
        mGoals[2] = new Goal("Distance", 0);

        mActions = new Action[6];
        mActions[0] = new Action("Green Light Driving");
        mActions[0].targetGoals.Add(new Goal("Speed", +5f));
        mActions[0].targetGoals.Add(new Goal("Fuel", -10f));
        mActions[0].targetGoals.Add(new Goal("Distance", +10f));

        mActions[1] = new Action("Red Light Stop");
        mActions[1].targetGoals.Add(new Goal("Speed", 0f));
        mActions[1].targetGoals.Add(new Goal("Fuel", -1f));
        mActions[1].targetGoals.Add(new Goal("Distance", +0f));

        mActions[2] = new Action("Yellow Light Slow Down");
        mActions[2].targetGoals.Add(new Goal("Speed", -5f));
        mActions[2].targetGoals.Add(new Goal("Fuel", -2f));
        mActions[2].targetGoals.Add(new Goal("Distance", +1f));

        mActions[3] = new Action("Gas Station Refuel");
        mActions[3].targetGoals.Add(new Goal("Speed", 0f));
        mActions[3].targetGoals.Add(new Goal("Fuel", +50f));
        mActions[3].targetGoals.Add(new Goal("Distance", +1f));

        mActions[4] = new Action("Ope I forgot something, I better turn around");
        mActions[4].targetGoals.Add(new Goal("Speed", +5f));
        mActions[4].targetGoals.Add(new Goal("Fuel", -10f));
        mActions[4].targetGoals.Add(new Goal("Distance", -10f));

        mActions[5] = new Action("*Sirens Flashing* I better make a run for it");
        mActions[5].targetGoals.Add(new Goal("Speed", +100f));
        mActions[5].targetGoals.Add(new Goal("Fuel", -50f));
        mActions[5].targetGoals.Add(new Goal("Distance", +50f));

        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Speed", +1f));
        mChangeOverTime.targetGoals.Add(new Goal("Fuel", -2f));
        mChangeOverTime.targetGoals.Add(new Goal("Distance", +2f));

        Debug.Log("Driving started Actions will be evaluated every: " + TICK_LENGTH + " seconds.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Hit E to do something.");
    }

    void Tick()
    {
        foreach (Goal goal in mGoals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0);
        }
        PrintGoals();
    }

    string PrintGoals()
    {
        goalString = "";
        foreach (Goal goal in mGoals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        goalString += "Discontentment: " + CurrentDiscontentment();
        Debug.Log(goalString);
        return goalString;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Goal.SafeDriving = !Goal.SafeDriving;
            Debug.Log("Safe Driving Mode: " + (Goal.SafeDriving ? "ON" : "OFF"));
            uiManager.UpdateToggleUI(Goal.SafeDriving);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Action bestThingToDo = ChooseAction(mActions, mGoals);
            Debug.Log(bestThingToDo.name);

            foreach (Goal goal in mGoals)
            {
                goal.value += bestThingToDo.GetGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
            }

            PrintGoals();
            uiManager.UpdateActionUI(goalString, bestThingToDo.name);
        }

        uiManager.UpdateStatsUI(goalString);
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);
  
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }

        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);

            discontentment += goal.GetDiscontentment(newValue);
        }

        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach (Goal goal in mGoals)
        {
            total += (goal.value * goal.value);
        }
        return total;
    }
}
