using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    
    private GunSystem activeGun;
    public List<GunSystem> allGuns = new List<GunSystem>();
    public int currentGunNumber;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GunSystem gun in allGuns)
        {
            gun.gameObject.SetActive(false);
        }
        activeGun = allGuns[currentGunNumber];
        activeGun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }
    }

    private void SwitchGun()
    {
         activeGun.gameObject.SetActive(false);
         currentGunNumber++;

         if(currentGunNumber >= allGuns.Count){
                currentGunNumber = 0;
         }

         activeGun = allGuns[currentGunNumber];
         activeGun.gameObject.SetActive(true);
    }

}