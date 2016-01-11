using UnityEngine;
using System.Collections;

public class DarkenOnDamaged : MonoBehaviour {

    private Renderer _mr = null;
    private Renderer mr
    {
        get
        {
            if (_mr == null)
                _mr = GetComponent<Renderer>();
            return _mr;
        }
    }

    public void Damaged(float percent)
    {
        mr.material.color *= 1f - percent;
    }
}
