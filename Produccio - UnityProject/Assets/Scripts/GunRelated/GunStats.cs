using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GunType", order = 1)]
public class GunStats : ScriptableObject
{
    public string name;
    public float fireRate;
    public int maxAmmo;
    public int magazineSize;
    public GunSelector fireType;

    [SerializeField]
    public GameObject BuleltPrefab;



    public enum GunSelector
    {
        Single,
        Auto
    }
}
