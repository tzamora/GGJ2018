using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public int hp = 10;

    public int power = 1;

    public enum UnitTypeEnum { ally, enemy };

    public UnitTypeEnum unitType;

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;

    Animator setAnimation;

    GameObject debugCircle;

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

            //Animator.play();

            Vector2 direction = (Vector2)transform.position - previousPosition;

            if (direction.x > 0) {

                transform.localScale = new Vector3(1, 1, 1);

            }

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // if right (theScale.x *= -1;)
            // if left  (theScale.x *= 1;)

            previousPosition = transform.position;

            setAnimation.SetInteger("ani", 1);

            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);

            if (transform.position == (Vector3)destination)
            {
                handleAction(other);

                // .stop
                setAnimation.SetInteger("ani", 0);

                handler.EndLoop();
            }

        });

    }

    void handleAction(GameObject other) {

        print("vamos a hacer una accion");

        if (!other) {

            return;

        }

        UnitController enemyUnit = other.GetComponent<UnitController>();

        if (enemyUnit)
        {
            UnitController unit = other.GetComponent<UnitController>();

            if (unit.unitType == UnitTypeEnum.enemy)
            {
                print("vamos a atacarlo");
                damage(unit);
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

        this.tt("CheckAllyNearRoutine").Loop((handler)=> {

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 4f);

            foreach (Collider2D collider in colliders) {

                UnitController unit = collider.GetComponent<UnitController>();

                if (unit && unit.unitType == UnitTypeEnum.ally) {

                    debugCircle = unit.gameObject;

                    this.action(unit.transform.position, unit.gameObject);

                    handler.EndLoop();
                }

            }

            handler.Wait(1);

            print("cada cuanto corre esto");

        });

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (debugCircle) {
            Gizmos.DrawWireSphere(debugCircle.transform.position, 4f);
        }
        
    }
}
