using UnityEngine;

public class ColorPaletteManager : MonoBehaviour
{
    public static Color SelectedColor = Color.clear;

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color result))
        {
            return result;
        }

        return Color.white;
    }

    public void SetColorRosaPastell() => SelectedColor = HexToColor("#FFC8FF");
    public void SetColorHellblauPastell() => SelectedColor = HexToColor("#DCFAFF");
    public void SetColorCremeGelb() => SelectedColor = HexToColor("#FFEFC5");
    public void SetColorLilaPastell() => SelectedColor = HexToColor("#EBEBFF");
    public void SetColorAltrosaPastell() => SelectedColor = HexToColor("#FFD2D2");
    public void SetColorSandBeige() => SelectedColor = HexToColor("#F0E6DC");
    public void SetColorZitronenPastell() => SelectedColor = HexToColor("#FFFFBE");
    public void SetColorMintPastell() => SelectedColor = HexToColor("#EBFIDC");

    public void SetColorKorallePink() => SelectedColor = HexToColor("#FA6982");
    public void SetColorGrau() => SelectedColor = HexToColor("#808080");
    public void SetColorHimmelblau() => SelectedColor = HexToColor("#6EC8FA");
    public void SetColorGrasgruen() => SelectedColor = HexToColor("#96F06E");
    public void SetColorMagentaBeere() => SelectedColor = HexToColor("#DC6EA5");
    public void SetColorOrange() => SelectedColor = HexToColor("#F59B14");

    public void SetColorWeiss() => SelectedColor = HexToColor("#FFFFFF");
    public void SetColorSchwarz() => SelectedColor = HexToColor("#000000");
}