using IrisFenrir.Input;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    public TapKey tapKey;
    public int count;

    public PressKey pressKey;
    public bool isPressing;
    public float pressedTime;

    public ValueKey valueKey;
    public float value;

    public AxisKey axisKey;
    public float axisValue;
    public float axisValueX;
    public float axisValueY;

    public MultiKey multiKey;

    public ComboKey comboKey;
    public int combo;

    public InputData inputData;

    public GraphicRaycaster caster;

    private void Start()
    {
        valueKey.Init();
        axisKey.Init();

        multiKey.Init();
    }

    private void Update()
    {
        //tapKey.Update();
        //if (tapKey.isTriggered)
        //    print("Triggered");
        //count = tapKey.currentCount;


        //pressKey.Update();
        //if (pressKey.isDown)
        //    print("Down");
        //if (pressKey.isUp)
        //    print("Up");
        //isPressing = pressKey.isPressing;
        //pressedTime = pressKey.pressedTime;

        //valueKey.Update();
        //value = valueKey.value;

        //axisKey.Update();
        //axisValue = axisKey.value1D;
        //axisValueX = axisKey.value2D.x;
        //axisValueY = axisKey.value2D.y;

        multiKey.Update();
        if (multiKey.isTriggered)
        {
            print("Multi Triggered");
        }

        //comboKey.Update();
        //combo = comboKey.combo;
        //if(comboKey.isTriggered)
        //{
        //    print("Combo Triggered");
        //}

        //if(UIRaycaster.RaycastWithClick(caster,out var results))
        //{
        //    print(results[0].gameObject);
        //}


    }


}
