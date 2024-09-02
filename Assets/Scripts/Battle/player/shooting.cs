using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// the script to manage shooting
/// </summary>
public class Shooting : MonoBehaviour
{
    //all needed controls
    public InputActionProperty moveProperty;
    public InputActionProperty shootProperty;
    public InputActionProperty backProperty;

    public BattleManager battleManager;
    public EventSystem eventSystem;
    public GameObject shootButton;

    private Rigidbody2D rb;
    private Transform t;

    //the speed which the aim moves
    public float speed;
    
    private RaycastHit2D[] hits;
    private Transform hitT;

    //is the player shooting
    private bool shooting = false;

    //did the player fire
    private bool shot = false;

    //number of times the player can shoot
    private int shoots;

    //bullets to show where you hit
    [SerializeField] private List<Transform> bullets = new();

    //amount of damage done to each enemy
    private int[] damage;

    /// <summary>
    /// the player starts shooting or reloads
    /// </summary>
    public void StartShooting()
    {
        if (GlobalVariables.EquippedWeapon != null)
        {
            //does the player need to reload?
            if (GlobalVariables.EquippedWeaponAmmo == 0)
            {
                switch (GlobalVariables.EquippedWeapon.weaponType)
                {
                    case 0:
                        if (GlobalVariables.LightAmmo != 0)
                        {
                            GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.LightAmmo);
                            battleManager.enemyAttacks.Attack();
                            eventSystem.SetSelectedGameObject(null);
                        }
                        break;
                    case 1:
                        if (GlobalVariables.ShotgunAmmo != 0)
                        {
                            GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.ShotgunAmmo);
                            battleManager.enemyAttacks.Attack();
                            eventSystem.SetSelectedGameObject(null);
                        }
                        break;
                    case 2:
                        if (GlobalVariables.MediumAmmo != 0)
                        {
                            GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.MediumAmmo);
                            battleManager.enemyAttacks.Attack();
                            eventSystem.SetSelectedGameObject(null);
                        }
                        break;
                }

            }
            else
            {
                t.position = new Vector3(0f, 1f, 0f);
                shooting = true;
                shoots = GlobalVariables.EquippedWeapon.weaponFireRate;
            }
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        t = GetComponent<Transform>();

        //sizes the damage array
        damage = new int[battleManager.transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            Vector2 movement = moveProperty.action.ReadValue<Vector2>();
            if (!shot)
                rb.velocity = movement * speed;

            //stops shooting
            if (backProperty.action.WasPressedThisFrame() && shoots == GlobalVariables.EquippedWeapon.weaponFireRate)
            {
                eventSystem.SetSelectedGameObject(shootButton);

                rb.velocity = Vector2.zero;
                transform.position = new Vector3(0, 10, 0);
                shooting = false;
            }
            else
            {
                if (shootProperty.action.WasPressedThisFrame() && shot)
                {
                    
                    foreach (Transform bullet in bullets)
                    {
                        hitT = null;
                        hits = Physics2D.CircleCastAll(bullet.position, 0.02f, new Vector2(0, 1), 0, 256);
                        int sorrtingOrder = -10;
                        foreach (RaycastHit2D i in hits)
                        {
                            if (i.transform != null && i.transform.GetComponent<SpriteRenderer>().sortingOrder > sorrtingOrder)
                            {
                                sorrtingOrder = i.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                                hitT = i.transform;
                            }
                        }
                        if (hitT != null)
                        {
                            if (hitT.parent.GetComponent<BaseEnemyRelay>() != null)
                            {
                                damage[hitT.parent.GetSiblingIndex()] += (int)(GlobalVariables.EquippedWeapon.weaponDamage * hitT.GetComponent<EnemyHitbox>().damageMultiplier);
                            }
                        }
                        bullet.position = new Vector3(0, 10, 0);
                    }
                    shoots--;

                    if (shoots < 1 || GlobalVariables.EquippedWeaponAmmo == 0)
                    {
                        shooting = false;
                        for (int i = 0; i < damage.Length; i++)
                        {
                            if (damage[i] == 0)
                                battleManager.transform.GetChild(i).GetComponent<BaseEnemyRelay>().Miss();
                            else
                                battleManager.transform.GetChild(i).GetComponent<BaseEnemyRelay>().Hit(damage[i]);
                            damage[i] = 0;
                        }
                    }
                    else
                    {
                        t.position = new Vector3(0f, 1f, 0f);
                    }
                    battleManager.StartAnimation();

                    shot = false;


                }
                else if (shootProperty.action.WasPressedThisFrame() && eventSystem.currentSelectedGameObject == null && !shot)
                {
                    if (GlobalVariables.EquippedWeapon.weaponType == 0)
                    {
                        GlobalVariables.LightAmmo--;
                        GlobalVariables.EquippedWeaponAmmo--;
                        bullets[0].position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    else if (GlobalVariables.EquippedWeapon.weaponType == 1)
                    {
                        GlobalVariables.ShotgunAmmo--;
                        GlobalVariables.EquippedWeaponAmmo--;
                        foreach (Transform bullet in bullets)
                            bullet.position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    else if (GlobalVariables.EquippedWeapon.weaponType == 2)
                    {
                        GlobalVariables.MediumAmmo--;
                        GlobalVariables.EquippedWeaponAmmo--;
                        bullets[0].position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    else
                    {
                        bullets[0].position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    shot = true;
                    battleManager.StopAnimation();
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(0, 10, 0);
                }

                eventSystem.SetSelectedGameObject(null);
            }
        }
    }
}
