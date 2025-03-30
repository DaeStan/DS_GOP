using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public static bool SafeDriving = false;

    public string name;
    public float value;

    public Goal(string goalName, float goalValue)
    {
        name = goalName;
        value = goalValue;
    }

    public float GetDiscontentment(float newValue)
    {
        if (SafeDriving == true)
        {
            return newValue * newValue;
        }
        else
        {
            return 1 / (newValue + 1);
        }
    }
}

public class Action
{
    public string name;
    public List<Goal> targetGoals;

    public Action(string actionName)
    {
        name = actionName;
        targetGoals = new List<Goal>();
    }

    public float GetGoalChange(Goal goal)
    {
        foreach (Goal target in targetGoals)
        {
            if (target.name == goal.name)
            {
                return target.value;
            }
        }
        return 0f;
    }
}
