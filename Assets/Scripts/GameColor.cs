using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public static class GameColor
{
    
    public static Color LIGHT_PINK = new Color(.9372549f, .3411765f, .4666667f); // #ef5777
    public static Color PINK = new Color(.9607843f, .2313726f, .3411765f); // #f53b57
    public static Color LIGHT_NIGHT_BLUE = new Color(.3411765f, .372549f, .8117647f); // #575fcf
    public static Color NIGHT_BLUE = new Color(.2352941f, .2509804f, .7764706f); // #3c40c6
    public static Color LIGHT_BLUE = new Color(.2941177f, .8117647f, .9803922f); // #4bcffa
    public static Color BLUE = new Color(.05882353f, .7372549f, .9764706f); // #0fbcf9
    public static Color LIGHT_CYAN = new Color(.2039216f, .9058824f, .8941177f); // #34e7e4
    public static Color CYAN = new Color(0, .8470588f, .8392157f); // #00d8d6
    public static Color LIGHT_GREEN = new Color(.04313726f, .9098039f, .5058824f); // #0be881
    public static Color GREEN = new Color(.01960784f, .7686275f, .4196078f); // #05c46b
    public static Color LIGHT_ORANGE = new Color(1, .7529412f, .282353f); // #ffc048
    public static Color ORANGE = new Color(1, .6588235f, .003921569f); // #ffa801
    public static Color LIGHT_YELLOW = new Color(1, .8666667f, .3490196f); // #ffdd59
    public static Color YELLOW = new Color(1, .827451f, .1647059f); // #ffd32a
    public static Color LIGHT_RED = new Color(1, .3686275f, .3411765f); // #ff5e57
    public static Color RED = new Color(1, .2470588f, .2039216f); // #ff3f34
    public static Color LIGHT_GREY = new Color(.8235294f, .854902f, .8862745f); // #d2dae2
    public static Color GREY = new Color(.5019608f, .5568628f, .6078432f); // #808e9b
    public static Color LIGHT_BLACK = new Color(.282353f, .3294118f, .3764706f); // #485460
    public static Color BLACK = new Color(.1176471f, .1529412f, .1803922f); // #1e272e

    public static void Dump()
    {
        Dictionary<string, string> test = new Dictionary<string, string>();
        test["LIGHT_PINK"] = "ef5777";
        test["PINK"] = "f53b57";
        test["LIGHT_NIGHT_BLUE"] = "575fcf";
        test["NIGHT_BLUE"] = "3c40c6";
        test["LIGHT_BLUE"] = "4bcffa";
        test["BLUE"] = "0fbcf9";
        test["LIGHT_CYAN"] = "34e7e4";
        test["CYAN"] = "00d8d6";
        test["LIGHT_GREEN"] = "0be881";
        test["GREEN"] = "05c46b";
        test["LIGHT_ORANGE"] = "ffc048";
        test["ORANGE"] = "ffa801";
        test["LIGHT_YELLOW"] = "ffdd59";
        test["YELLOW"] = "ffd32a";
        test["LIGHT_RED"] = "ff5e57";
        test["RED"] = "ff3f34";
        test["LIGHT_GREY"] = "d2dae2";
        test["GREY"] = "808e9b";
        test["LIGHT_BLACK"] = "485460";
        test["BLACK"] = "1e272e";

        string result = "";
        
        foreach (KeyValuePair<string, string> entry in test)
        {
            Color color = UtilsClass.GetColorFromString(entry.Value);
            result += "public static Color " + entry.Key + " = new Color(" + color.r + ", " + color.g + ", " + color.b + "); // #" + entry.Value + "\n";
        }
    }
}
