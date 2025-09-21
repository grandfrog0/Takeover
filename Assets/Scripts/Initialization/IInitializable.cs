using System;

public interface IInitializable
{
    InitializeOrder Order => InitializeOrder.AfterInitialization;
    void Initialize();
}