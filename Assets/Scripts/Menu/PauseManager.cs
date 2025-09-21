using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PauseManager : MonoBehaviour, IInitializable
{
    private static PauseManager inst;
    private static float timeScale;

    private static bool _isPaused;
    public static bool IsPaused
    {   
        get => _isPaused; 
        set
        {
            _isPaused = value;

            if (_isPaused && SettingsManager.CurrentSettings.saveWhenMenuOpening)
                AppDataLoader.SaveData();

            Time.timeScale = value ? 0 : timeScale;
        }
    }
    public static void RestartAutosaveTimer() => inst.RestartAutosave();
    public static void InvokePauseMenu()
    {
        WindowManager.CreateOpen(PrefabBuffer.PauseWindow);
    }

    public InitializeOrder Order => InitializeOrder.MenuManager;
    private Coroutine autosaveCoroutine;
    public void Initialize()
    {
        inst = this;
        timeScale = Time.timeScale;
        RestartAutosaveTimer();
    }
    public void RestartAutosave()
    {
        if (!autosaveCoroutine.IsUnityNull())
            StopCoroutine(autosaveCoroutine);

        autosaveCoroutine = StartCoroutine(AutosaveCoroutine());
    }
    private IEnumerator AutosaveCoroutine()
    {
        while (true)
        {
            while (SettingsManager.CurrentSettings.autosavePeriod.ToSeconds() <= 0)
            {
                Debug.Log("infinity wait...");
                yield return new WaitForSeconds(10);
            }

            yield return new WaitForSeconds(SettingsManager.CurrentSettings.autosavePeriod.ToSeconds());

            AppDataLoader.SaveData();
        }
    }
}
