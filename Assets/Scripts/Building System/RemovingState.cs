using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData structureData;
    ObjectPlacer objectPlacer;
    MapGenerator mapGenerator;
    ParticleSystem[] particleEffects;
    AudioSource failedSfx;

    public RemovingState(Grid grid,
                       PreviewSystem previewSystem,
                       GridData floorData,
                       GridData structureData,
                       ObjectPlacer objectPlacer,
                       MapGenerator mapGenerator,
                       ParticleSystem[] effects,
                       AudioSource sfx)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;
        this.mapGenerator = mapGenerator;
        this.particleEffects = effects;
        this.failedSfx = sfx;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if(structureData.CanPlaceObjectAt(gridPosition,Vector2Int.one,-1,mapGenerator) == false)
        {
            selectedData = structureData;
        }
        else if(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one,-1, mapGenerator) == false)
        {
            selectedData= floorData;
        }

        if(selectedData == null)
        {
            failedSfx.Play();
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex, particleEffects[0]);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(structureData.CanPlaceObjectAt(gridPosition, Vector2Int.one,-1,mapGenerator)
               && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one,-1, mapGenerator));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
