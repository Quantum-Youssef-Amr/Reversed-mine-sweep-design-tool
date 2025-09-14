using UnityEngine;
using TMPro;

public class SolverUi : MonoBehaviour
{
    [SerializeField] private TMP_InputField targetInput;
    [SerializeField] private TextMeshProUGUI numberOfBooms, numberOfSolutions;

    private int _innerBooms, _outerBooms, _cornerBooms;
    void Start()
    {
        EventBus.OnMapSolved += DisplaySolution;
    }

    private void DisplaySolution((int, int, int, Rect) Booms)
    {
        int m_totalBooms = Booms.Item1 + Booms.Item2 + Booms.Item3;
        if (m_totalBooms < 0)
        {
            numberOfBooms.text = $"Min Booms: {m_totalBooms}";
            numberOfSolutions.text = $"Num. of solutions:0 ::NO SOLUTION";
            return;
        }

        numberOfBooms.text = $"Min Booms: {m_totalBooms}::({Booms.Item1} IN, {Booms.Item2} OUT, {Booms.Item3} COR)";


        int m_InnerBoomsMax = (int)Mathf.Max(0, (Booms.Item4.width - 2) * (Booms.Item4.height - 2));
        int m_OuterBoomsMax = (int)Mathf.Max(0, 2 * (Booms.Item4.width - 2) + 2 * (Booms.Item4.height - 2));

        numberOfSolutions.text = $"Num. of solutions: {m_InnerBoomsMax * Booms.Item1 + m_OuterBoomsMax * Booms.Item2 + 4 * Booms.Item3}";

        _innerBooms = Booms.Item1;
        _outerBooms = Booms.Item2;
        _cornerBooms = Booms.Item3;
    }

    public void SolveMap()
    {
        int.TryParse(targetInput.text, out int target);
        EventBus.OnMapReqSolve?.Invoke(target);
    }

    public void PlaceBooms()
    {
        if (_innerBooms + _outerBooms + _cornerBooms <= 0)
            return;

        int.TryParse(targetInput.text, out int target);
        EventBus.OnBoomPlaceReq?.Invoke((_innerBooms, _outerBooms, _cornerBooms), target);
    }
}
