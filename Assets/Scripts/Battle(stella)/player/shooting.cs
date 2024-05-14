using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public InputActionProperty move;
    public InputActionProperty shoot;
    public InputActionProperty back;

    public BattleManager battleManager;
    public EventSystem eventSystem;
    public GameObject shootButton;

    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;

    public float speed;

    private RaycastHit2D[] hits;
    private Transform hitT;
    private bool shooting = false;
    private bool shot = false;
    private int shoots;

    [SerializeField] private List<Transform> bullets = new();

    private int[] damage;
    public void StartShooting()
    {
        if (GlobalVariables.EquippedWeapon != null)
        {
            if (GlobalVariables.EquippedWeaponAmmo == 0)
            {
                switch (GlobalVariables.EquippedWeapon.weaponType)
                {
                    case 0:
                        GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.LightAmmo);
                        break;
                    case 1:
                        GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.ShotgunAmmo);
                        break;
                    case 2:
                        GlobalVariables.EquippedWeaponAmmo = Mathf.Clamp(GlobalVariables.EquippedWeapon.weaponMaxAmmo, 0, GlobalVariables.MediumAmmo);
                        break;
                    default:
                        GlobalVariables.EquippedWeaponAmmo = -1;
                        break;
                }

            }
            else
            {
                t.position = new Vector3(0f, 1f, 0f);
                cc.radius = 0.03f;
                shooting = true;
                shoots = GlobalVariables.EquippedWeapon.weaponFireRate;
            }
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        t = GetComponent<Transform>();
        damage = new int[battleManager.transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {

            Vector2 movement = move.action.ReadValue<Vector2>();
            if (!shot)
                rb.velocity = movement * speed;

            if (back.action.WasPressedThisFrame() && shoots == GlobalVariables.EquippedWeapon.weaponFireRate)
            {
                eventSystem.SetSelectedGameObject(shootButton);

                rb.velocity = Vector2.zero;
                transform.position = new Vector3(0, 10, 0);
                shooting = false;
            }
            else
            {
                if (shoot.action.WasPressedThisFrame() && shot)
                {
                    foreach (Transform bullet in bullets)
                    {
                        hitT = null;
                        hits = Physics2D.CircleCastAll(bullet.position, 0.02f, new Vector2(0, 1), 0, 256);
                        int sorrtingOrder = -10;
                        foreach (RaycastHit2D i in hits)
                        {
                            if (i.transform != null && i.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder > sorrtingOrder)
                            {
                                sorrtingOrder = i.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                                hitT = i.transform;
                            }
                        }
                        if (hitT != null)
                        {
                            if (hitT.GetComponent<BaseEnemyRelay>() != null)
                            {
                                damage[hitT.GetSiblingIndex()] += GlobalVariables.EquippedWeapon.weaponDamage;
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
                else if (shoot.action.WasPressedThisFrame() && eventSystem.currentSelectedGameObject == null && !shot)
                {
                    if (GlobalVariables.EquippedWeapon.weaponType == 0)
                    {
                        GlobalVariables.LightAmmo--;
                        bullets[0].position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    else if (GlobalVariables.EquippedWeapon.weaponType == 1)
                    {
                        GlobalVariables.ShotgunAmmo--;
                        foreach (Transform bullet in bullets)
                            bullet.position = transform.position + Random.insideUnitSphere * GlobalVariables.EquippedWeapon.weaponSpread / 10;
                    }
                    if (GlobalVariables.EquippedWeapon.weaponType == 2)
                    {
                        GlobalVariables.MediumAmmo--;
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
