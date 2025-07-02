using UnityEngine;

public class StageClearChecker : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] spawners;
    [SerializeField] private GameObject portalTrigger;

    private bool portalShown = false;

    void Update()
    {
        if (!portalShown && AllSpawnersCleared())
        {
            portalShown = true;
            portalTrigger.SetActive(true);
        }
    }

    private bool AllSpawnersCleared()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            if (spawner != null && spawner.isActiveAndEnabled && !spawner.IsCleared)
                return false;
        }
        return true;
    }
}