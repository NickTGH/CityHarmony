using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    public Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    [HideInInspector]
    public GridData floorData, structureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField]
    private ResourceManager resourceManager;

    [SerializeField]
    private MapGenerator mapGenerator;

    [Space(20)]
    [SerializeField]
    private ParticleSystem[] particleEffects;

    [Space(40)]
    [SerializeField]
    private AudioSource enterStateSfx;
	[SerializeField]
	private AudioSource placeSfx;
	[SerializeField]
	private AudioSource destroySfx;
    [SerializeField]
    private AudioSource failedSfx;

	IBuildingState buildingState;

    private void Awake()
    {
        StopPlacement();
        floorData = new();
        structureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        enterStateSfx.Play();
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           structureData,
                                           resourceManager,
                                           objectPlacer,
                                           mapGenerator,
                                           particleEffects,
                                           failedSfx);
        if (ID == 1)
        {
            inputManager.OnHeld += PlaceStructure;
        }
        else
        {
            inputManager.OnClicked += PlaceStructure;
        }
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        enterStateSfx.Play();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid,
                                          preview,
                                          floorData,
                                          structureData,
                                          objectPlacer,
                                          mapGenerator,
                                          particleEffects,
                                          failedSfx);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnHeld += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector2 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
        placeSfx.Play();
    }

    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnHeld -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
    private void Update()
    {
       if(buildingState==null)
       {
           return;
       }
        Vector2 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}
