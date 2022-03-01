using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int centerGridPosition;
    [HideInInspector]
    public TileSegment[] segments;

    private void Start() 
    {
        centerGridPosition = GridController.instance.topCenter;
        transform.position = GridController.gridToWorldSpace(centerGridPosition);

        segments = GetComponentsInChildren<TileSegment>();
    }

    private void Update() 
    {
        handleMovement();
    }

    private void handleMovement()
    {
        Move gridMovement = new Move(this);

        if(Input.GetKey(KeyCode.UpArrow))   
            gridMovement.Add(0, 1);

        if(Input.GetKey(KeyCode.DownArrow))  
            gridMovement.Add(0, -1);

        if(Input.GetKey(KeyCode.RightArrow)) 
            gridMovement.Add(1, 0);

        if(Input.GetKey(KeyCode.LeftArrow))  
            gridMovement.Add(-1, 0);

        if(Input.GetKeyDown(KeyCode.X))
            gridMovement.AddRotatation();

        if(Input.GetKeyDown(KeyCode.Z))
            gridMovement.AddFlip();

        if(Input.GetKey(KeyCode.Space))
            gridMovement.FastFall();

        Movement.TryToMove(gridMovement);
    }

    public void ChangeColor(Color c)
    {
        MeshRenderer[] childrenRends = transform.GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i< childrenRends.Length; ++i)
        {
            Material newMaterial = new Material(childrenRends[i].material);
            newMaterial.color = c;
            childrenRends[i].material = newMaterial;
        }
    }

    public void Deactivate()
    {
        Destroy(this);
    }
}
