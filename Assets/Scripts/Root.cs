using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using UnityEngine.U2D;

public class Root : MonoBehaviour {
    public TexturePainter painter;
    private Color[] arrColors = new Color[6] { Color.red, Color.blue, Color.cyan, Color.gray, Color.yellow, Color.magenta };
    private Transform transPanelColor;
    private Transform transPanelBrush;
    private Transform transPanelTool;
    private GameObject goCar;
    private LOD[] arrLOD;

    // Use this for initialization
    void Start () {
        transPanelBrush = this.transform.Find("PanelBrush");
        if(transPanelBrush != null){
            InitBrushes();
        }
        InitColors();
        //
        transPanelTool = this.transform.Find("PanelTools");
        if (transPanelTool == null) return;
        //
        InitBtnBrush();
        Transform transBtnConfirm = transPanelTool.Find("BtnConfirm");
        InitBtns("BtnClear", HandleBtnClear);
        InitBtns("BtnConfirm", HandlerBtnConfirm);
        //
        goCar = GameObject.FindGameObjectWithTag("Player");
        if (goCar == null) return;
        LODGroup lodGroup = goCar.GetComponent<LODGroup>();
        arrLOD = lodGroup.GetLODs();
        //

    }

    private void ChangeColor(Color color){
        LOD lod;
        for (int i = 0; i < arrLOD.Length;i++){
            lod = arrLOD[i];
            Renderer[] arrRenderers = lod.renderers;
            foreach(Renderer render in arrRenderers)
            {
                if(render.name.Contains("Paint_")){
                    render.material.color = color;
                    break;
                }
            }
        }
    }

    private void InitBrushes(){
        foreach(Transform transBtn in transPanelBrush){
            Button btnBrush = transBtn.GetComponent<Button>();
            btnBrush.onClick.AddListener(() => {
                Image imgBrush = transBtn.GetComponent<Image>();
                string strSprite = imgBrush.sprite.name;
                Painter_BrushMode mode = GetBrushMode(strSprite);
                painter.SetBrushMode(mode,strSprite);
                transPanelBrush.gameObject.SetActive(false);
            });
        }
    }

    private void HandleBtnClear()
    {
        ChangeColor(Color.white);
        painter.ClearDraw();
    }

    private void HandlerBtnConfirm(){
    }


    private void InitColors(){
        transPanelColor = this.transform.Find("PanelColors");
        uint index = 0;
        foreach (Transform transChild in transPanelColor)
        {
            Image img = transChild.GetComponent<Image>();
            if (index >= arrColors.Length)
            {
                return;
            }
            img.color = arrColors[index];
            index++;
            Button btn = transChild.GetComponent<Button>();
            btn.onClick.AddListener(() => { 
                ChangeColor(img.color);
            });
        }
    }

    private void InitBtnBrush(){
        Transform transBtnBrush = transPanelTool.Find("Text").Find("BtnBrush");
        Transform transBtnBrush1 = transPanelTool.Find("Text/BtnBrush");
        Transform transBtnBrush2 = transPanelTool.Find("BtnBrush");
        if (transBtnBrush == null) return;
        Button btnBrush = transBtnBrush.GetComponent<Button>();
        btnBrush.onClick.AddListener(() => {
            if (transPanelBrush == null) return;
            if(transPanelBrush.gameObject.activeInHierarchy){
                transPanelBrush.gameObject.SetActive(false);
            }
            else{
                transPanelBrush.gameObject.SetActive(true);
            }
        });
    }

    private void InitBtns(string strBtnName,UnityAction action){
        Transform transBtnClear = transPanelTool.Find(strBtnName);
        if (transBtnClear == null) return;
        Button btnComp = transBtnClear.GetComponent<Button>();
        btnComp.onClick.AddListener(action);
    }

    private Painter_BrushMode GetBrushMode(string strSprite){
        if (string.IsNullOrEmpty(strSprite)) return Painter_BrushMode.PAINT;
        Painter_BrushMode mode;
        switch(strSprite){
            case "Decal":
                mode = Painter_BrushMode.DECAL;
                break;
            case "Heart":
                mode = Painter_BrushMode.HEART;
                break;
            default:
                mode = Painter_BrushMode.PAINT;
                break;
        }
        return mode;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
