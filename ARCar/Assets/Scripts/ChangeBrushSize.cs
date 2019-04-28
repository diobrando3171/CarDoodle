using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBrushSize : MonoBehaviour {
    private Slider sliderBrushSize;
    private float valueSlider;
    public TexturePainter painter;
	// Use this for initialization
	void Start () {
        sliderBrushSize = this.transform.Find("SliderBrushSize").GetComponent<Slider>();
        sliderBrushSize.onValueChanged.AddListener(
            (value) => {painter.SetBrushSize(value); }
        );
	}
}
