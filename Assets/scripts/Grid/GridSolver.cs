using System;
using UnityEngine;

public class GridSolver : MonoBehaviour
{

    private Rect _MapDimantions;
    private int _InnerBoomsMax, _OuterBoomsMax, _CornerBoomsMax = 4;
    private int _InnerBooms, _OuterBooms, _CornerBooms;
    void Start()
    {
        EventBus.OnMapGenrated += SaveMapInfo;
        EventBus.OnMapReqSolve += SolveMap;
    }

    private void SaveMapInfo(int width, int height)
    {
        _MapDimantions.size = new Vector2(width, height);
    }

    private void SolveMap(int target)
    {
        _InnerBoomsMax = (int)Mathf.Max(0, (_MapDimantions.width - 2) * (_MapDimantions.height - 2));

        _OuterBoomsMax = (int)Mathf.Max(0, 2 * (_MapDimantions.width - 2) + 2 * (_MapDimantions.height - 2));

        int m_InitialBoomNumber = Mathf.CeilToInt(target / 8f);

        if (target < 8)
        {
            EventBus.OnMapSolved?.Invoke((1, 0, 0, _MapDimantions));
            return;
        }

        for (int currentBoomNumber = m_InitialBoomNumber; currentBoomNumber < _MapDimantions.width * _MapDimantions.height; currentBoomNumber++)
        {
            int m_delta = 8 * currentBoomNumber - target;

            if (m_delta < 0)
                break;

            for (int currentCornerBoomsNumber = 0; currentCornerBoomsNumber < _CornerBoomsMax; currentCornerBoomsNumber++)
            {
                // rest = 3CB + 5OB - 2*IB
                int m_rest = m_delta - 5 * currentCornerBoomsNumber - 2 * (currentBoomNumber - 1);

                if (m_rest < 0 || m_rest % 3 != 0)
                    continue;

                _OuterBooms = m_rest / 3;
                _InnerBooms = currentBoomNumber - _OuterBooms - currentCornerBoomsNumber;
                _CornerBooms = currentCornerBoomsNumber;

                if (_OuterBooms >= 0 && _OuterBooms <= _OuterBoomsMax &&
                    _InnerBooms >= 0 && _InnerBooms <= _InnerBoomsMax &&
                    _CornerBooms >= 0 && _CornerBooms <= _CornerBoomsMax)
                {
                    if (8 * _InnerBooms + 5 * _OuterBooms + 3 * _CornerBooms >= target)
                    {
                        EventBus.OnMapSolved((_InnerBooms, _OuterBooms, _CornerBooms, _MapDimantions));
                        return;
                    }
                }

            }
        }
        EventBus.OnMapSolved?.Invoke((-1, 0, 0, _MapDimantions));
    }
}
