using Assets.Scripts;

public class ActivatableWall : Activatable
{
    protected override bool _needsActivating { get; set; } = true;
    protected override bool Active { get; set; } = false;

    public override void Activate()
    {
        Destroy(gameObject);
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
