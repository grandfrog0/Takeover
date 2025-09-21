using System.Collections.Generic;
using UnityEngine;

public class BuildMaterialsContainer : MonoBehaviour, IInitializable
{
    private static BuildMaterialsContainer inst;

    [SerializeField] ChoosePositionStep _choosePositionStep;
    public static ChoosePositionStep ChoosePositionStep => inst._choosePositionStep;
    public static T Inst<T>(T obj) where T : MonoBehaviour => Instantiate(obj.gameObject).GetComponent<T>();

    public InitializeOrder Order => InitializeOrder.BeforeInitialization;
    public void Initialize()
    {
        inst = this;
    }
}