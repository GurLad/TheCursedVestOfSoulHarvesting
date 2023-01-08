using Godot;
using System;

public class UAGiveVest : AUnitAction
{
    public override string Name => "Transfer Vest";
    public override string Description => "Gives the Cursed Vest to a target.\nRequires the target to be adjacent.";
    public override Vector2Int Range => new Vector2Int(thisUnit.AttackRange);
    public override bool RequiresTarget => true;
    public override bool FreeAction => false;
    public override bool UseMoveMarkers => false;

    protected override void ActivateEffect(Vector2Int target = null)
    {
        Unit targetUnit = thisUnit.TurnFlowController.GetUnitAtPos(target);
        if (targetUnit != null)
        {
            thisUnit.QueueAnimation(new AnimKnock(), new AnimKnock.Args(0.3f, 30, targetUnit.Pos - thisUnit.Pos, true, false));
            thisUnit.QueueImmediateAction(() =>
            {
                thisUnit.HasVest = false;
                targetUnit.HasVest = true;
                thisUnit.TurnFlowController.UpdateUI();
            });
            thisUnit.QueueAnimation(new AnimKnock(), new AnimKnock.Args(0.3f, -30, targetUnit.Pos - thisUnit.Pos, true, false));
            thisUnit.QueueAnimation(new AnimDelay(), new AnimDelay.Args(0.5f));
        }
        else
        {
            throw new Exception("Literally impossible");
        }
    }

    public override void PassiveEffect()
    {
        // Do nothing
    }
}