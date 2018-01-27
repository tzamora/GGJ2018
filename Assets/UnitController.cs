﻿using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public int hp = 10;

    public int power = 10;

    public enum UnitTypeEnum { ally, enemy };

    public UnitTypeEnum unitType;

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;

    Animator setAnimation;

    // Use this for initialization
    void Start () {

        setAnimation = GetComponent<Animator>();

    }

    public void action(Vector2 destination, GameObject other) {

        destination = Camera.main.ScreenToWorldPoint(destination);
        //destination.y = Screen.height - destination.y;

        this.tt("MoveRoutine").Reset().Loop((handler) => {

            //Animator.play();

            // if right (theScale.x *= -1;)
            // if left  (theScale.x *= 1;)

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

        UnitController enemyUnit = other.GetComponent<UnitController>();

        if (enemyUnit)
        {
            print("encontramos algo");

            UnitController unit = other.GetComponent<UnitController>();

            if (unit.unitType == UnitTypeEnum.enemy)
            {
                print("encontramos a un enemigo");

                damage(unit);
            }

        }

    }

    public void damage(UnitController other) {

        Renderer enemyRenderer = other.GetComponent<Renderer>();

        Color defaultColor = enemyRenderer.material.color;

        this.tt().Add(0.2f, handler => {
             
            enemyRenderer.material.color = Color.red;

        }).Add(0.2f, handler => {

            enemyRenderer.material.color = defaultColor;

        }).Add(()=> {

            other.hp -= this.power;

            if (other.hp <= 0)
            {

                Destroy(other.gameObject);

                // death animation

                

                // destroy

            }

        }).If(()=> hp > 0).Repeat();

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

        if (!GameContext.Get.selectedUnits.Contains(this)) {
            GameContext.Get.selectedUnits.Add(this);
        }
        

    }

    void deselect() {
        GameContext.Get.selectedUnits.Remove(this);
    }
}
