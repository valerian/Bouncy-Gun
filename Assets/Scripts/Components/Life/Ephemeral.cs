using UnityEngine;
using System.Collections;

public class Ephemeral : MonoBehaviourDelayedEnable
{
    [SerializeField]
    protected float _totalDuration;
    public float totalDuration { get { return _totalDuration; } protected set { _totalDuration = value; } }

    protected float _birthTime = Time.time;
    public float birthTime { get { return _birthTime; } private set { _birthTime = value; } }

    public float elapsedTime { get { return Time.time - _birthTime; } }
    public float remainingTime { get { return _birthTime + totalDuration - Time.time; } }

    public static Ephemeral AddOrUpdateComponent(GameObject where, float totalDuration)
    {
        Ephemeral ephemeral = where.GetComponent<Ephemeral>() ?? where.AddComponent<Ephemeral>();
        ephemeral._totalDuration = totalDuration;
        return ephemeral;
    }

    protected void OnEnableAwake()
    {
        birthTime = Time.time;
    }

    void OnEnableDelayed()
    {
        StartCoroutine(DespawnAfterRemainingTime());
    }

    protected IEnumerator DespawnAfterRemainingTime()
    {
        yield return new WaitForSeconds(remainingTime);
        Pool.Despawn(gameObject);
    }
}
