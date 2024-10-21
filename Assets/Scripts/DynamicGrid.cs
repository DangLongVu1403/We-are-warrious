using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // Gán GridLayoutGroup của navigation bottom ở đây
    public RectTransform navigationRectTransform; // RectTransform của thanh điều hướng
    public int numberOfColumns = 4; // Số lượng cột bạn muốn

    void Start()
    {
        AdjustCellSize();
    }

    void AdjustCellSize()
    {
        // Lấy chiều rộng của thanh điều hướng
        float width = navigationRectTransform.rect.width;
        
        // Tính toán kích thước của ô dựa trên chiều rộng màn hình và số cột
        float cellWidth = width / numberOfColumns;
        
        // Gán kích thước ô cho Grid Layout Group
        gridLayoutGroup.cellSize = new Vector2(cellWidth, gridLayoutGroup.cellSize.y); // Giữ nguyên chiều cao (y)
    }

    void Update()
    {
        AdjustCellSize(); // Điều chỉnh khi có thay đổi kích thước (nếu cần)
    }
}
