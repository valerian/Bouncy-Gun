using UnityEngine;
using System.Collections;

public class Ephemeral : MonoBehaviour
{
    [SerializeField]
    private float _totalDuration;
    public float totalDuration { get { return _totalDuration; } }

    private float _birthTime = Time.time;
    public float birthTime { get { return _birthTime; } }

    public float elapsedTime { get { return Time.time - _birthTime; } }
    public float remainingTime { get { return _birthTime + totalDuration - Time.time; } }

    public static Ephemeral AddOrUpdateComponent(GameObject where, float totalDuration)
    {
        Ephemeral ephemeral = where.GetComponent<Ephemeral>() ?? where.AddComponent<Ephemeral>();
        ephemeral._totalDuration = totalDuration;
        return ephemeral;
    }

    void OnEnable()
    {
        _birthTime = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time > _birthTime + _totalDuration)
            Pool.Despawn(gameObject);
    }
}
