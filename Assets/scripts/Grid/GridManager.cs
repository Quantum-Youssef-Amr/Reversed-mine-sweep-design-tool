using System;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int MaxAttempt = 50;
    private Ceil[,] _gridCeils;
    private int _width, _height;
    private int _attempts;
    void Start()
    {
        EventBus.OnMapGenrated += InitGrid;
        EventBus.OnCanPlaceItemCheck += CheckPlace;
        EventBus.OnItemSpawned += Spawnitem;
        EventBus.OnItemDelete += DeleteItem;

        EventBus.OnBoomPlaceReq += (Booms, target) =>
        {
            InitGrid(_width, _height);
            PlaceBooms(Booms, target);
        };
    }
    private void PlaceBooms((int, int, int) BoomsConfig, int target)
    {
        if (_attempts >= MaxAttempt)
        {
            Debug.LogError("this problem may couse a stack overflow");
            _attempts = 0;
            return;
        }
        _attempts++;

        PlaceCornerBooms(BoomsConfig);
        PlaceOuterBooms(BoomsConfig);
        PlaceInnerBooms(BoomsConfig);

        if (CheckBrunch(target, out int m_currentScore))
        {
            EventBus.OnBoomsPlaced?.Invoke(target - m_currentScore, _gridCeils);
            EventBus.OnBoardUpdate?.Invoke(_gridCeils);
        }
        else
        {
            InitGrid(_width, _height);
            PlaceBooms(BoomsConfig, target);
        }
    }

    private bool CheckBrunch(int target, out int _currentScore)
    {
        _currentScore = CalcTotalScore();
        return _currentScore == target;
    }

    private void PlaceInnerBooms((int, int, int) BoomsConfig)
    {
        for (int _InnerBoom = 0; _InnerBoom < BoomsConfig.Item1; _InnerBoom++)
        {
            int x = UnityEngine.Random.Range(1, _width - 2);
            int y = UnityEngine.Random.Range(1, _height - 2);

            if (_gridCeils[x, y].ceilHave != item.None)
            {
                _InnerBoom--;
                continue;
            }

            _gridCeils[x, y].ceilHave = item.boom;
            UpdateCielScores(x, y);
        }
    }

    private void PlaceOuterBooms((int, int, int) BoomsConfig)
    {
        for (int _outerBooms = 0; _outerBooms < BoomsConfig.Item2; _outerBooms++)
        {
            int x = UnityEngine.Random.Range(0, _width);
            int y = UnityEngine.Random.Range(0, _height);
            float m_randomchance = UnityEngine.Random.value;

            if (m_randomchance > 0.5f)
            {
                if (_gridCeils[x, 0].ceilHave != item.None)
                {
                    _outerBooms--;
                    continue;
                }
                _gridCeils[x, 0].ceilHave = item.boom;
                UpdateCielScores(x, 0);
            }
            else
            {
                if (_gridCeils[0, y].ceilHave != item.None)
                {
                    _outerBooms--;
                    continue;
                }
                _gridCeils[0, y].ceilHave = item.boom;
                UpdateCielScores(0, y);
            }
        }
    }

    private void PlaceCornerBooms((int, int, int) BoomsConfig)
    {
        (int x, int y)[] corners = { (0, 0), (_width - 1, 0), (0, _height - 1), (_width - 1, _height - 1) };
        for (int _CornerBooms = 0; _CornerBooms < BoomsConfig.Item3; _CornerBooms++)
        {
            var (x, y) = corners[_CornerBooms];
            if (_gridCeils[x, y].ceilHave != item.None)
            {
                _CornerBooms--;
                continue;
            }

            _gridCeils[x, y].ceilHave = item.boom;
            UpdateCielScores(x, y);
        }
    }

    private void DeleteItem(int x, int y)
    {
        if (_gridCeils[x, y].ceilHave == item.boom)
            UpdateCielScores(x, y, false);

        _gridCeils[x, y].ceilHave = item.None;

        EventBus.OnBoardUpdate?.Invoke(_gridCeils);
    }

    private void Spawnitem(int x, int y, item item)
    {
        _gridCeils[x, y].ceilHave = item;

        if (item == item.boom)
            UpdateCielScores(x, y);

        EventBus.OnBoardUpdate?.Invoke(_gridCeils);
    }

    private void UpdateCielScores(int x, int y, bool add = true)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                try
                {
                    _gridCeils[x + j, y + i].score += add ? 1 : -1;
                    _gridCeils[x + j, y + i].score = Mathf.Max(0, _gridCeils[x + j, y + i].score);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
    }

    private void CheckPlace(int x, int y)
    {
        EventBus.OnCanPlaceItemResponce?.Invoke(_gridCeils[x, y].ceilHave == item.None);
    }

    private void InitGrid(int width, int height)
    {
        _gridCeils = new Ceil[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _gridCeils[x, y] = new();
            }
        }

        _width = width;
        _height = height;
    }

    private int CalcTotalScore()
    {
        int m_score = 0;
        foreach (Ceil ceil in _gridCeils)
        {
            if (ceil.ceilHave != item.None)
                continue;

            m_score += ceil.score;
        }

        return m_score;
    }
}
public enum item
{
    None, boom, wall
}

public class Ceil
{
    public int score;
    public item ceilHave;
    public Ceil()
    {
        score = 0;
        ceilHave = item.None;
    }

}