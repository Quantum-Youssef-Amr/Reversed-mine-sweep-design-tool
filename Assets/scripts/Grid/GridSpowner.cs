using UnityEngine;

public class GridSpowner : MonoBehaviour
{
    [SerializeField] private GameObject ceilPrefab;
    [SerializeField] private GameObject BoomPrefab;
    private Transform _gridTransform;
    private int _width, _height;
    void Start()
    {
        _gridTransform = transform;
        EventBus.OnMapGenrated += GenerateMap;
        EventBus.OnBoomsPlaced += (error, map) =>
        {
            PlaceBooms(map);
        };

    }

    private void PlaceBooms(Ceil[,] Map)
    {
        for (int child = 0; child < _gridTransform.childCount; child++)
        {
            GameObject m_child = _gridTransform.GetChild(child).GetChild(0).gameObject;
            if (m_child.name.Contains("boom"))
                Destroy(m_child);
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (Map[x, y].ceilHave == item.None)
                    continue;

                GameObject m_boom = Instantiate(BoomPrefab, transform.GetChild(x + y * _width).transform);
                m_boom.transform.SetAsFirstSibling();
            }
        }
    }

    private void GenerateMap(int width, int height)
    {
        for (int child = 0; child < _gridTransform.childCount; child++)
        {
            Destroy(_gridTransform.GetChild(child).gameObject);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject m_spawnedceil = Instantiate(ceilPrefab, transform);
                m_spawnedceil.transform.localPosition = new(x, y, 0);
            }
        }
        _gridTransform.position = Vector3.zero;
        _gridTransform.position = new(-width / 2f, -height / 2f);

        _width = width;
        _height = height;
    }
}
