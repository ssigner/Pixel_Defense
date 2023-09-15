using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraDrag : MonoBehaviour
{
    public Camera mainCamera;

    // Start is called before the first frame update
    public Tilemap tilemap;
    private Vector3 touchStart;
    public float cameraSpeed = 5f;

    private float minX, maxX, minY, maxY;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    public float maxXExtraScreenPosX = 0;
    float prevCameraOrthographicSize = 0f;
    void Start()
    {
        SetCameraMoveBound();
    }

    private void SetCameraMoveBound()
    {
        var tileMapWorldBound = FindTileMapBounds();

        var cameraOrthographicSize = mainCamera.orthographicSize;
        var cameraAspect = mainCamera.aspect;

        var wPosExtra = mainCamera.ScreenToWorldPoint(new Vector3(maxXExtraScreenPosX, 0))

            - mainCamera.ScreenToWorldPoint(new Vector3(0, 0)); ;
        Debug.Log("wPosExtra " + wPosExtra);
        // 최소 및 최대 이동 제한 설정
        minX = tileMapWorldBound.xMin + cameraOrthographicSize * cameraAspect;
        maxX = tileMapWorldBound.xMax - cameraOrthographicSize * cameraAspect + wPosExtra.x;
        minY = tileMapWorldBound.yMin + cameraOrthographicSize;
        maxY = tileMapWorldBound.yMax - cameraOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += direction * cameraSpeed * Time.deltaTime;

            // 카메라 위치를 제한하여 Tilemap 범위 내에서 이동
            limitCamera();
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * 2, zoomOutMin, zoomOutMax);

        if(Mathf.Approximately(mainCamera.orthographicSize,   prevCameraOrthographicSize)==false)
        {
            SetCameraMoveBound();
            prevCameraOrthographicSize = mainCamera.orthographicSize;
            limitCamera();
        }
        


    }

    void limitCamera()
    {
        mainCamera.transform.position = new Vector3(
                Mathf.Clamp(mainCamera.transform.position.x, minX, maxX),
                Mathf.Clamp(mainCamera.transform.position.y, minY, maxY),
                mainCamera.transform.position.z
            );
    }

    Rect FindTileMapBounds()
    {

        BoundsInt bounds = tilemap.cellBounds;
        // Tilemap의 월드 바운드 계산
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 minTile = Vector3.zero;
        Vector3 maxTile = Vector3.zero;


        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                Vector3 cellCenter = tilemap.GetCellCenterWorld(pos);
                if (cellCenter.x < minTile.x || minTile == Vector3.zero)
                    minTile.x = cellCenter.x;
                if (cellCenter.x > maxTile.x || maxTile == Vector3.zero)
                    maxTile.x = cellCenter.x;
                if (cellCenter.y < minTile.y || minTile == Vector3.zero)
                    minTile.y = cellCenter.y;
                if (cellCenter.y > maxTile.y || maxTile == Vector3.zero)
                    maxTile.y = cellCenter.y;
            }
        }

        minTile -= tilemap.cellSize * 0.5f;
        maxTile += tilemap.cellSize * 0.5f;

        return new Rect(minTile, maxTile - minTile);

    }

    void OnDrawGizmos()
    {
        if (tilemap == null)
            return;

        BoundsInt bounds = tilemap.cellBounds;
        // Tilemap의 월드 바운드 계산
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 minTile = Vector3.zero;
        Vector3 maxTile = Vector3.zero;


        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                Vector3 cellCenter = tilemap.GetCellCenterWorld(pos);
                if (cellCenter.x < minTile.x || minTile == Vector3.zero)
                    minTile.x = cellCenter.x;
                if (cellCenter.x > maxTile.x || maxTile == Vector3.zero)
                    maxTile.x = cellCenter.x;
                if (cellCenter.y < minTile.y || minTile == Vector3.zero)
                    minTile.y = cellCenter.y;
                if (cellCenter.y > maxTile.y || maxTile == Vector3.zero)
                    maxTile.y = cellCenter.y;
            }
        }

        minTile -= tilemap.cellSize * 0.5f;
        maxTile += tilemap.cellSize * 0.5f;
        // Tilemap의 월드 바운드를 기즈모로 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((minTile + maxTile) / 2f, maxTile - minTile);
    }
}
