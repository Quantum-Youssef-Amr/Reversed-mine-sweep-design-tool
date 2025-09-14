using System;
using UnityEngine;

public static class EventBus
{
    public static Action<int> OnTotalScore;
    /// <summary>
    /// a call for generating a new map with a width and height
    /// </summary>
    public static Action<int, int> OnMapGenrated;

    public static Action<int> OnMapReqSolve;
    public static Action<(int, int, int), int> OnBoomPlaceReq;
    public static Action<(int, int, int, Rect)> OnMapSolved;
    public static Action<int, Ceil[,]> OnBoomsPlaced;

    /// <summary>
    /// a call when an item is spowned to update the grid data
    /// </summary>
    public static Action<int, int, item> OnItemSpawned;

    /// <summary>
    /// a call when an item is deleted to update the grid data
    /// </summary>
    public static Action<int, int> OnItemDelete;

    public static Action<int, int> OnCanPlaceItemCheck;
    public static Action<bool> OnCanPlaceItemResponce;

    /// <summary>
    /// a call for updating the board visuls
    /// </summary>
    public static Action<Ceil[,]> OnBoardUpdate;

}
