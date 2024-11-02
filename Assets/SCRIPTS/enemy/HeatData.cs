using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatData : MonoBehaviour
{
    protected List<(int, float)> heatToItemDropChances = new List<(int, float)>()
    {
        (1, 0.05f),
        (2, 0.025f),
    };

    protected List<(string, float)> heat1Enemies = new List<(string, float)>
    {
        ("grunt", 20f),
        ("caster", 20f),
        ("brute", 20f),
        ("hardcccaster", 20f),
        ("rootbrute", 20f),
        ("scarecrow", 20f),
        ("quandry", 20f),
        //("firecrackerer", 9.5f),
        //("golem", 0.5f)
    };

    //protected List<(string, float)> heat1Enemies = new List<(string, float)>
    //{
    //    ("grunt", 40f),
    //    ("caster", 30f),
    //    ("brute", 20f),
    //    ("firecrackerer", 9.5f),
    //    ("golem", 0.5f)
    //};

    protected List<(string, float)> heat2Enemies = new List<(string, float)>
    {
        ("grunt", 15f),
        ("caster", 25f),
        ("brute", 34f),
        ("firecrackerer", 15f),
        ("golem", 8f)
    };
    protected List<(string, float)> heat3Enemies = new List<(string, float)>
    {
        ("grunt", 15f),
        ("caster", 25f),
        ("brute", 34f),
        ("firecrackerer", 15f),
        ("golem", 8f)
    };

    protected List<string> heat1Items = new List<string>
    {
        "basketHat",
        "tecoFeather",
        "tanto",
        "bow",
        "cowl",
        "hatchet",
        "pointedHatchet",
        "jadeLion",
        "shortRedSword",
        "breastplate"
    };

    protected List<string> heat2Items = new List<string>
    {
        "ravenFeather"
    };
}
