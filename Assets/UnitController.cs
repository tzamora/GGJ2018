using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;
using System.Linq;

public class UnitController : MonoBehaviour {

    public int hp = 10;

    public int power = 1;

    public GameObject damageLabelPrefab;

    public enum UnitTypeEnum { ally, enemy };

    public UnitTypeEnum unitType;

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;
    
    Animator setAnimation;

    bool isBusy = false;

    List<GameObject> debugCircles = new List<GameObject>();

    // Use this for initialization
    void Start () {

        setAnimation = GetComponent<Animator>();

        //
        // run enemy AI
        //

        if (unitType == UnitTypeEnum.enemy) {

            CheckAllyNearRoutine();

        }

    }

    public void MoveRoutine(Vector2 destination, GameObject other) {

        Vector2 previousPosition = Vector3.zero;

        this.tt("MoveUnitRoutine").Loop(delegate (ttHandler handler)
        {
            print("estoy aqui pegadito");

            Vector2 direction = (Vector2)transform.position - previousPosition;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            previousPosition = transform.position;

            setAnimation.SetInteger("ani", 1);

            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);

            if (transform.position == (Vector3)destination)
            {
                handleAction(other);

                handler.EndLoop();
            }

        });

    }

    public void handleAction(GameObject other) {

        if (!other) {
            return;
        }

        UnitController otherUnit = other.GetComponent<UnitController>();

        if (otherUnit)
        {
            print("atacando: " + otherUnit);

            //
            // stop friendly attacks
            //

            if (this.unitType != otherUnit.unitType)
            {
                float distance = Vector3.Distance(this.transform.position, otherUnit.transform.position);

                bool isNearEnough = distance <= 1;

                if (isNearEnough)
                {
                    setAnimation.SetInteger("ani", 2);

                    damage(otherUnit);

                }
                else
                {
                    MoveRoutine(otherUnit.transform.position, otherUnit.gameObject);
                }
            }
        }
        else {

            MineralController mineral = other.GetComponent<MineralController>();

            if (mineral)
            {
                mineral.extract();
            }
            else
            {

                AllySpawnerController allySpawner = other.GetComponent<AllySpawnerController>();

                if (allySpawner) {

                    allySpawner.trySpawn();

                }
            }

        }


    }

    public void damage(UnitController other) {

        Renderer enemyRenderer = other.GetComponent<Renderer>();

        Color defaultColor = enemyRenderer.material.color;

        this.tt("DamageRoutine").Add(0.2f, handler => {
             
            if(enemyRenderer) enemyRenderer.material.color = Color.red;

        }).Add(0.2f, handler => {

            if (enemyRenderer) enemyRenderer.material.color = defaultColor;

        }).Add(0.2f,()=> {

            other.hp -= this.power;

            //
            // 
            //

            other.showDamageLabel(this.power);

            if (other && (other.hp <= 0))
            {
                other.setAnimation.SetInteger("ani", 3);
                this.setAnimation.SetInteger("ani", 0);

                if (other.unitType == UnitTypeEnum.ally)
                {
                    GameContext.Get.allyUnits.Remove(other);
                }
                else
                {
                    GameContext.Get.enemyUnits.Remove(other);
                }

                isBusy = false;
                this.tt("DamageRoutine").Release();
                Destroy(other.gameObject,5);
            }

        }).Repeat().Immutable();
    }

    public void showDamageLabel(int amount) {


        Vector3 labelPosition = transform.position + new Vector3(0, 0.7f, 0);

        GameObject damageLabel = GameObject.Instantiate(damageLabelPrefab, labelPosition, Quaternion.identity);

        damageLabel.transform.localScale = new Vector3(0.08f, 0.08f, 1);

        damageLabel.GetComponent<TextMesh>().text = "-"+amount+"";

        Destroy(damageLabel, 4f);
    }

    public void select() {

        Renderer renderer = GetComponent<Renderer>();

        originalMaterial = renderer.material;

        renderer.material = highlightMaterial;

        //
        // add to selected list
        //

        if (this.unitType == UnitTypeEnum.ally) {

            if (!GameContext.Get.selectedPlayerUnits.Contains(this))
            {
                GameContext.Get.selectedPlayerUnits.Add(this);
            }
        }

        if (this.unitType == UnitTypeEnum.enemy)
        {
            if (!GameContext.Get.selectedEnemyUnits.Contains(this))
            {
                GameContext.Get.selectedEnemyUnits.Add(this);
            }
        }


    }

    void deselect() {

        if (this.unitType == UnitTypeEnum.ally)
        {
            GameContext.Get.selectedPlayerUnits.Remove(this);
        }

        if (this.unitType == UnitTypeEnum.enemy)
        {
            GameContext.Get.selectedEnemyUnits.Remove(this);
        }

    }

    void CheckAllyNearRoutine() {

        this.tt("CheckAllyNearRoutine").Add(()=> {

            List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 5f).ToList();

            colliders = colliders.OrderBy(
                x => Vector2.Distance(this.transform.position, x.transform.position)
            ).ToList();

            foreach (Collider2D collider in colliders) {

                UnitController unit = collider.GetComponent<UnitController>();

                if (unit && unit.unitType == UnitTypeEnum.ally && unit.hp > 0) {

                    //break;

                    //debugCircles.Add(unit.gameObject);

                    if (!isBusy) {

                        isBusy = true;

                        MoveRoutine(unit.transform.position, unit.gameObject);
                    }

                }

            }

            print("esto deberia de correr mas lentito");

        }).Add(1).Repeat();

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    foreach (var circle in debugCircles) {
    //        Gizmos.DrawWireSphere(circle.transform.position, 1f);
    //    }
    //}
}
