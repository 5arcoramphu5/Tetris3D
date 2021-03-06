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
        Move gridMove = new Move(this);

        if(Input.GetKey(KeyCode.UpArrow))   
            gridMove.Add(CameraMovement.localForward);

        if(Input.GetKey(KeyCode.DownArrow))  
            gridMove.Add(-CameraMovement.localForward);

        if(Input.GetKey(KeyCode.RightArrow)) 
            gridMove.Add(CameraMovement.localRight);

        if(Input.GetKey(KeyCode.LeftArrow))  
            gridMove.Add(-CameraMovement.localRight);

        if(Input.GetKeyDown(KeyCode.X))
            gridMove.AddRotatation(CameraMovement.localRight);

        if(Input.GetKeyDown(KeyCode.Z))
            gridMove.AddRotatation(CameraMovement.localForward);

        if(Input.GetKey(KeyCode.Space))
            gridMove.FastFall();

        Movement.TryToMove(gridMove);
    }

    public void ChangeColor(Color c)
    {
        MeshRenderer[] childrenRends = transform.GetComponentsInChildren<MeshRenderer>();
        if(childrenRends.Length == 0)
            return;
            
        Material newMaterial = new Material(childrenRends[0].material);
        for(int i = 0; i< childrenRends.Length; ++i)
        {
            newMaterial.color = c;
            childrenRends[i].material = newMaterial;
        }
    }

    public void Deactivate()
    {
        Destroy(this);
    }
}
