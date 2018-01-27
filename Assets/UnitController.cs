using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;
using System.Linq;

public class UnitController : MonoBehaviour {

    public int hp = 10;

    public int power = 1;

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

        if (unitType == UnitTypeEnum.enemy) {

            CheckAllyNearRoutine();

        }

    }

    public void action(Vector2 destination, GameObject other) {

        //destination = Camera.main.ScreenToWorldPoint(destination);
        //destination.y = Screen.height - destination.y;

        Vector2 previousPosition = Vector3.zero;

        this.tt("MoveRoutine").Reset().Loop((handler) => {

            isBusy = true;

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

    void handleAction(GameObject other) {

        if (!other) {
            return;
        }

        UnitController otherUnit = other.GetComponent<UnitController>();

        if (otherUnit)
        {
            //
            // stop friendly attacks
            //

            if (this.unitType != otherUnit.unitType)
            {
                float distance = Vector3.Distance(this.transform.position, otherUnit.transform.position);

                print(distance);

                bool isNearEnough = distance <= 1;

                if (isNearEnough)
                {

                    setAnimation.SetInteger("ani", 2);
                    damage(otherUnit);

                }
                else
                {

                    this.action(otherUnit.transform.position, otherUnit.gameObject);

                }

                
            }
        }

    }

    public void damage(UnitController other) {

        Renderer enemyRenderer = other.GetComponent<Renderer>();

        Color defaultColor = enemyRenderer.material.color;

        this.tt().Add(0.2f, handler => {
             
            if(enemyRenderer) enemyRenderer.material.color = Color.red;

        }).Add(0.2f, handler => {

            if (enemyRenderer) enemyRenderer.material.color = defaultColor;

        }).Add(()=> {

            other.hp -= this.power;

            if (other && other.hp <= 0)
            {
                setAnimation.SetInteger("ani", 3);

                if (other.unitType == UnitTypeEnum.ally)
                {
                    GameContext.Get.allyUnits.Remove(other);
                }
                else
                {
                    GameContext.Get.enemyUnits.Remove(other);
                }

                isBusy = false;

                Destroy(other.gameObject);

                // death animation

                

                // destroy

            }

        }).If(()=> other && other.hp > 0).Repeat();

        //
        // damage the other unit
        //

        
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

        this.tt("CheckAllyNearRoutine").If(() => !isBusy ).Add(()=> {

            List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 5f).ToList();

            colliders = colliders.OrderBy(
                x => Vector2.Distance(this.transform.position, x.transform.position)
            ).ToList();

            foreach (Collider2D collider in colliders) {

                UnitController unit = collider.GetComponent<UnitController>();

                if (unit && unit.unitType == UnitTypeEnum.ally) {

                    debugCircles.Add(unit.gameObject);

                    this.action(unit.transform.position, unit.gameObject);

                }

            }

            print("cada cuanto corre esto");

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
