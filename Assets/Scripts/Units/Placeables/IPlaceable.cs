using UnityEngine.Events;

public interface IPlaceable
{
    public void Place(V2 pos);
    public bool TryPlace(V2 pos);
    public bool CanPlace(V2 pos, out Unit u);
}
