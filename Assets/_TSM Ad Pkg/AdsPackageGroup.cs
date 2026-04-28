
using UnityEngine;

public class AdsPackageGroup : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
