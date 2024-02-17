using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData structureData;
    ResourceManager resourceManager;
    ObjectPlacer objectPlacer;
    MapGenerator mapGenerator;
    ParticleSystem[] particleEffects;
    AudioSource failedSfx;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData structureData,
                          ResourceManager resourceManager,
                          ObjectPlacer objectPlacer,
                          MapGenerator mapGenerator,
                          ParticleSystem[] effects,
                          AudioSource sfx)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.structureData = structureData;
        this.resourceManager = resourceManager;
        this.objectPlacer = objectPlacer;
        this.mapGenerator = mapGenerator;
        this.particleEffects = effects;
        this.failedSfx = sfx;

        selectedObjectIndex = database.objectsData.FindIndex(x => x.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size*5);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, mapGenerator)
								 && resourceManager.CanAffordStructure(database.objectsData[selectedObjectIndex].ResourceCost);
        if (placementValidity == false)
        {
            failedSfx.Play();
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex],
											 grid.CellToWorld(gridPosition),
											 resourceManager,
                                             particleEffects);
        resourceManager.DecreaseResourcesAfterPlacement(database.objectsData[selectedObjectIndex].ResourceCost);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : structureData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex, MapGenerator mapGenerator)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : structureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size,selectedObjectIndex, mapGenerator);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex, mapGenerator)
								 && resourceManager.CanAffordStructure(database.objectsData[selectedObjectIndex].ResourceCost);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
