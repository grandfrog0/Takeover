using System.Collections.Generic;
using UnityEngine.UIElements;

public interface IBuilder
{
    bool IsFinished { get; }
    void Deload();
    bool NextStep();
    bool CanNext();
    bool HasNext();
    Dictionary<string, string> GetResult();
    void End();
    void DestroyModel();
}
