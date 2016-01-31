using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GunStatusControl : MonoBehaviour
{
    public enum STATUS
    {
        disabled,
        idle,
        charging,
        charged,
    }

    private UnityEvent _onCharging;
    public UnityEvent onCharging { get { return _onCharging ?? (_onCharging = new UnityEvent()); } }

    private UnityEvent _onCharged;
    public UnityEvent onCharged { get { return _onCharged ?? (_onCharged = new UnityEvent()); } }

    private UnityEvent _onFired;
    public UnityEvent onFired { get { return _onFired ?? (_onFired = new UnityEvent()); } }

    private UnityEvent _onDisabled;
    public UnityEvent onDisabled { get { return _onDisabled ?? (_onDisabled = new UnityEvent()); } }

    public float chargingRatio { get; private set; }

    private STATUS _status = STATUS.disabled;
    public STATUS status
    {
        get { return _status; }
        private set
        {
            STATUS previousStatus = _status;
            _status = value;
            if (value != previousStatus && previousStatus != STATUS.disabled)
                switch (value)
                {
                    case STATUS.charging:
                        onCharging.Invoke();
                        break;
                    case STATUS.charged:
                        onCharged.Invoke();
                        break;
                    case STATUS.idle:
                        onFired.Invoke();
                        break;
                    case STATUS.disabled:
                        onDisabled.Invoke();
                        break;
                    default:
                        break;
                }
        }
    }

    private float chargingStartTime = float.NegativeInfinity;

    void Awake()
    {
        chargingRatio = 0;
        Game.instance.onStateChanged.AddListener((Game.STATE state) => { status = (state == Game.STATE.play) ? STATUS.idle : STATUS.disabled; });
        if (Game.State == Game.STATE.play)
            status = STATUS.idle;
        Game.InputManager.onClickStateChanged.AddListener(OnClickStateChanged);
    }

    void FixedUpdate()
    {
        if (status == STATUS.charging)
        {
            if (Time.time - chargingStartTime >= Game.GameData.gunChargeTime)
                status = STATUS.charged;
            chargingRatio = Mathf.Clamp01((Time.time - chargingStartTime) / Game.GameData.gunChargeTime);
        }
    }

    void OnClickStateChanged(bool isClicked)
    {
        if (isClicked && status == STATUS.idle)
        {
            status = STATUS.charging;
            chargingStartTime = Time.time;
        }
        if (!isClicked && (status == STATUS.charging || status == STATUS.charged))
            status = STATUS.idle;
    }

    
}
