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

    public float range;

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;
    
    Animator setAnimation;

    public AudioClip audioWalk;

    public AudioClip audioAttack;

    public bool isBusy = false;


    List<GameObject> debugCircles = new List<GameObject>();

    // Use this for initialization
    void Start () {

        if (this.unitType == UnitTypeEnum.ally)
        {
            GameContext.Get.allyUnits.Add(this);
        }
        else
        {
            GameContext.Get.enemyUnits.Add(this);
        }

        setAnimation = GetComponent<Animator>();

        debugCircles.Add(this.gameObject);

        //
        // run enemy AI
        //

        if (unitType == UnitTypeEnum.enemy) {

            EnemyAIRoutine();

        }

    }

    public void resetRoutine() {

        this.tt("MoveUnitRoutine").Reset().Release();

    }

    public void MoveRoutine(Vector2 destination, GameObject other) {

        Vector2 previousPosition = Vector3.zero;

        SoundManager.Get.PlayClip(audioWalk, true);

        this.tt("MoveUnitRoutine").Reset().Loop(delegate (ttHandler handler)
        {
            if (other != null)
            {
                destination = other.transform.position;
            }

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
                setAnimation.SetInteger("ani", 0);

                handleAction(other);

                SoundManager.Get.StopClip(audioWalk);

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

            if (mineral && !mineral.isDepleted)
            {
                mineral.extract(this);
            }
            else
            {

                AllySpawnerController allySpawner = other.GetComponent<AllySpawnerController>();

                if (allySpawner && allySpawner.readyToUse) {

                    allySpawner.spawnAlly(this);

                }
            }

        }


    }

    public void damage(UnitController other) {

        if (other == null) {
            return;
        }

        Renderer enemyRenderer = other.GetComponent<Renderer>();

        Color defaultColor = Color.white; 

        if (enemyRenderer.material.GetType().GetProperty("color") != null) {
            defaultColor = enemyRenderer.material.color;

        }

        this.tt("DamageRoutine").Add(0.2f, handler => {
             
            if(enemyRenderer) enemyRenderer.material.color = Color.red;

        }).Add(0.2f, handler => {

            if (enemyRenderer) enemyRenderer.material.color = defaultColor;

        }).Add(0.2f,()=> {

            if (other != null)
            {
                other.hp -= this.power;

                SoundManager.Get.PlayClip(audioAttack, false);

                //
                // 
                //

                other.showDamageLabel(this.power);

                if (other && (other.hp <= 0))
                {
                    other.setAnimation.SetInteger("ani", 3);
                    this.setAnimation.SetInteger("ani", 0);
                    SoundManager.Get.StopClip(audioWalk);

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
                    Destroy(other.gameObject, 5);
                }

            }
            else {
                this.tt("DamageRoutine").Release();
            }

            //
            // validate that i'm not dead
            //

            if (hp <= 0) {
                this.tt("DamageRoutine").Release();
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

    void EnemyAIRoutine() {

        this.tt("CheckAllyNearRoutine").Add(()=> {

            List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, range).ToList();

            colliders = colliders.OrderBy(
                x => Vector2.Distance(this.transform.position, x.transform.position)
            ).ToList();

            foreach (Collider2D collider in colliders) {
                
                UnitController unit = collider.GetComponent<UnitController>();

                if (!isBusy)
                {
                    if (unit && unit.unitType == UnitTypeEnum.ally && unit.hp > 0)
                    {


                        isBusy = true;

                        MoveRoutine(unit.transform.position, unit.gameObject);

                        break;


                    }
                    else
                    {

                        MineralController mineral = collider.GetComponent<MineralController>();

                        if (mineral && !mineral.isDepleted)
                        {

                            isBusy = true;

                            MoveRoutine(mineral.transform.position, mineral.gameObject);

                            break;

                        }
                        else
                        {

                            AllySpawnerController allySpawner = collider.GetComponent<AllySpawnerController>();

                            if (allySpawner && allySpawner.readyToUse)
                            {


                                isBusy = true;

                                MoveRoutine(allySpawner.transform.position, allySpawner.gameObject);

                                break;


                            }
                        }

                    }
                }
            }

        }).Add(1).Repeat();

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (var circle in debugCircles) {
            Gizmos.DrawWireSphere(circle.transform.position, range);
        }
    }
}
