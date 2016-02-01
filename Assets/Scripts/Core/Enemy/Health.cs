using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    enum ON_DEPLETED
    {
        doNothing,
        die,
        dieAndDespawn,
    }

    [SerializeField]
    private ON_DEPLETED _onDepleted = ON_DEPLETED.dieAndDespawn;
    [SerializeField]
    private float _healthMax = 100f;
    [SerializeField]
    private float _healthCurrent = 0f;

    private UnityEvent _onDeath;
    public UnityEvent onDeath { get { return _onDeath ?? (_onDeath = new UnityEvent()); } }

    private UnityEvent _onHealthDepleted;
    public UnityEvent onHealthDepleted { get { return _onHealthDepleted ?? (_onHealthDepleted = new UnityEvent()); } }

    private UnityEvent _onHealthFull;
    public UnityEvent onHealthFull { get { return _onHealthFull ?? (_onHealthFull = new UnityEvent()); } }

    private UnityEventFloatFloat _onHealthChanged;
    public UnityEventFloatFloat onHealthChanged { get { return _onHealthChanged ?? (_onHealthChanged = new UnityEventFloatFloat()); } }

    public float healthMax { get { return _healthMax; } set { _healthMax = value; CheckEvents(); } }
    public float healthCurrent { get { return _healthCurrent; } set { _healthCurrent = Mathf.Clamp(value, 0f, _healthMax); CheckEvents(); } }
    public bool isHealthDepleted { get { return Mathf.Approximately(0f, _healthCurrent); } }
    public bool isHealthFull { get { return Mathf.Approximately(_healthMax, _healthCurrent); } }

    void OnEnable()
    {
        _healthCurrent = _healthMax;
    }

    public void FullyHeal()
    {
        healthCurrent = healthMax;
    }

    private void CheckEvents()
    {
        onHealthChanged.Invoke(healthCurrent, healthMax);
        if (isHealthDepleted)
        {
            onHealthDepleted.Invoke();
            if (_onDepleted != ON_DEPLETED.doNothing)
                onDeath.Invoke();
            if (_onDepleted == ON_DEPLETED.dieAndDespawn)
                Pool.Despawn(gameObject);
        }
        else if (isHealthFull)
            onHealthFull.Invoke();
    }
}
