using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPositionable
{
    V2 Position { get; }
    void AddMovedListener(UnityAction<V2, V2> action);
    void RemoveMovedListener(UnityAction<V2, V2> action);
}
