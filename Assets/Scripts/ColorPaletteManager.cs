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

    public void SetColorPastelPink() => SelectedColor = HexToColor("#FFC8FF");
    public void SetColorPastelLightBlue() => SelectedColor = HexToColor("#DCFAFF");
    public void SetColorCreamYellow() => SelectedColor = HexToColor("#FFEFC5");
    public void SetColorPastelLilac() => SelectedColor = HexToColor("#EBEBFF");
    public void SetColorDustyRose() => SelectedColor = HexToColor("#FFD2D2");
    public void SetColorSandBeige() => SelectedColor = HexToColor("#F0E6DC");
    public void SetColorPastelLemon() => SelectedColor = HexToColor("#FFFFBE");
    public void SetColorPastelMint() => SelectedColor = HexToColor("#EBFIDC");

    public void SetColorCoralPink() => SelectedColor = HexToColor("#FA6982");
    public void SetColorGray() => SelectedColor = HexToColor("#808080");
    public void SetColorSkyBlue() => SelectedColor = HexToColor("#6EC8FA");
    public void SetColorGrassGreen() => SelectedColor = HexToColor("#96F06E");
    public void SetColorMagentaBerry() => SelectedColor = HexToColor("#DC6EA5");
    public void SetColorOrange() => SelectedColor = HexToColor("#F59B14");

    public void SetColorWhite() => SelectedColor = HexToColor("#FFFFFF");
    public void SetColorBlack() => SelectedColor = HexToColor("#000000");
}
