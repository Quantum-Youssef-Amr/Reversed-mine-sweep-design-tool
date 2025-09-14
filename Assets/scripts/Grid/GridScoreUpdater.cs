using UnityEngine;
using TMPro;

public class GridScoreUpdater : MonoBehaviour
{
    private Transform _GridTransform;
    void Start()
    {
        _GridTransform = transform;
        InitBoard();
        EventBus.OnBoardUpdate += UpdateScores;
    }

    private void InitBoard()
    {
        for (int child = 0; child < _GridTransform.childCount; child++)
        {
            GameObject m_child = _GridTransform.GetChild(child).gameObject;
            m_child.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void UpdateScores(Ceil[,] ceils)
    {
        int m_TotalScore = 0;
        for (int child = 0; child < _GridTransform.childCount; child++)
        {
            GameObject m_child = _GridTransform.GetChild(child).gameObject;
            Ceil m_ceil = ceils[(int)m_child.transform.localPosition.x, (int)m_child.transform.localPosition.y];

            if (m_ceil.ceilHave != item.None || m_ceil.score == 0)
            {
                m_child.transform.Find("score").gameObject.SetActive(false);
                continue;
            }

            m_child.transform.Find("score").gameObject.SetActive(true);
            m_child.transform.Find("score").gameObject.GetComponent<TextMeshPro>().text = m_ceil.score.ToString();
            m_TotalScore += m_ceil.score;
        }

        EventBus.OnTotalScore?.Invoke(m_TotalScore);
    }
}
