using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {

    private ArrayList actions = new ArrayList();
    private int number;
    private bool isTerminal;

    public State(int number)
    {
        this.number = number;
        this.isTerminal = false;
    }
    public State(int number, bool isTerminal)
    {
        this.number = number;
        this.isTerminal = isTerminal;
    }
    public void AddActions(string action , float value, int nextStateNumber)
    {
        ArrayList ActionValue = new ArrayList();
        ActionValue.Insert(0, action);
        ActionValue.Insert(1, value);
        ActionValue.Insert(2, nextStateNumber);
        actions.Add(ActionValue);
    }
    public ArrayList GetActions()
    {
        return actions;
    }
    public int GetNumber()
    {
        return number;
    }

    public bool IsTerminal()
    {
        return isTerminal;
    }
}
