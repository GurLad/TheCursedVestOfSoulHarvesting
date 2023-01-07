using Godot;
using System;
using System.Collections.Generic;

public class TurnFlowController : Node
{
    public static TurnFlowController Current;
    [Export]
    public PackedScene BasePanel;
    [Export]
    public NodePath PanelHolderPath;
    private Control panelHolder;
    private List<UnitPanel> allUnits = new List<UnitPanel>();
    private int currentUnit;

    public override void _Ready()
    {
        base._Ready();
        Current = this;
        panelHolder = GetNode<Control>(PanelHolderPath);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (allUnits[currentUnit].Unit.Moved)
        {
            NextUnit();
        }
    }

    public void AddUnit(Unit unit)
    {
        UnitPanel unitPanel = BasePanel.Instance<UnitPanel>();
        unitPanel.Init();
        unitPanel.UpdateUnitData(unit);
        allUnits.Add(unitPanel);
        panelHolder.AddChild(unitPanel);
    }

    public void RemoveUnit(Unit unit)
    {
        UnitPanel panel = allUnits.Find(a => a.Unit == unit);
        if (panel != null)
        {
            panel.QueueFree();
            panel.Unit.QueueFree();
        }
        else
        {
            throw new Exception("Killing a non-existent unit!");
        }
    }

    public void Begin()
    {
        currentUnit = allUnits.FindIndex(a => a.Unit.HasVest);
        ActivateUnit(currentUnit);
    }

    private void NextUnit()
    {
        DeactivateUnit(currentUnit);
        ActivateUnit((currentUnit + 1) % allUnits.Count);
    }

    private void DeactivateUnit(int index)
    {
        allUnits[index].Unit.Moved = false;
        allUnits[index].Deactivate();
    }

    private void ActivateUnit(int index)
    {
        allUnits[currentUnit = index].Activate(() => allUnits[index].Unit.BeginTurn());
    }
}