using UnityEngine;
using UnityEngine.Events;

public class UnitUIManager : MonoBehaviour, IInitializable
{
    private static UnitUIManager inst;
    public InitializeOrder Order => InitializeOrder.UnitUIManager;

    [SerializeField] UnitBar unitBarPrefab, unitBotBarPrefab, creatorBarPrefab;
    [SerializeField] Transform parent;

    public static void ConnectUI(Unit unit)
    {
        bool isBot = unit is UnitBot;
        bool isCreator = unit is Creator;

        UnitBar unitBar = Instantiate(isBot ? (isCreator ? inst.creatorBarPrefab : inst.unitBotBarPrefab) : inst.unitBarPrefab, inst.parent);

        UnityAction<V2, V2> moveAction = (_, to) => unitBar.transform.position = (Vector3)to + Vector3.up * (unit.gridTransform.AddHeight + 0.5f * unit.transform.localScale.y);
        moveAction((0, 0), (unit.gridTransform.Position));
        unit.gridTransform.onMoved.AddListener(moveAction);

        unitBar.HealthPercent = unit.HealthPercent;
        unit.OnHealthChanged.AddListener(x => unitBar.HealthPercent = x);
        if (isBot)
        {
            UnitBotBar botBar = (UnitBotBar)unitBar;
            UnitBot bot = (UnitBot)unit;

            botBar.EnergyPercent = bot.EnergyPercent;
            botBar.PowerPercent = bot.PowerPercent;

            bot.OnEnergyChanged.AddListener(x => botBar.EnergyPercent = x);
            bot.OnPowerChanged.AddListener(x => botBar.PowerPercent = x);

            if (isCreator)
            {
                CreatorBar creatorBar = (CreatorBar)botBar;
                Creator creator = (Creator)bot;

                creatorBar.TimerValue = (int)creator.TimeLeft;
                creator.OnTimerChanged.AddListener(x => creatorBar.TimerValue = x);
                creator.OnTimerChanged.AddListener(_ => creatorBar.TimerNormalized = creator.TimeNormalized);
            }
        }

        unit.OnDestroyed.AddListener(unitBar.Destroy);
    }

    public void Initialize()
    {
        inst = this;
    }
}
