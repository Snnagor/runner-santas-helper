using UnityEngine;
using Zenject;

public class InputHandler
{ 
    private float nowPosition;

    Vector2 beginTouch;
    Vector2 endTouch;

    public bool isSidestep { get; set; } = false;

    #region Injects

    private SignalBus signalBus;
    private float deltaSwipeMin;
    private Config config;
    private GameManager gameManager;
    private RunnerAnimation runnerAnim;
    private SoundManager soundManager;

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           GameManager _gameManager, 
                           RunnerAnimation _runnerAnim,
                           SoundManager _soundManager)
    {
        deltaSwipeMin = _config.DeltaSwipeMin;
        signalBus = _signalBus;
        config = _config;
        gameManager = _gameManager;
        runnerAnim = _runnerAnim;
        soundManager = _soundManager;
    }

    #endregion    
    
    /// <summary>
    /// ���������� �������
    /// </summary>
    /// <returns></returns>
    private Vector2 GetSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //��������� �������
            if (touch.phase == TouchPhase.Began)
            {
                beginTouch = touch.position;
            }

            //�������� �������
            if (touch.phase == TouchPhase.Ended)
            {
                endTouch = touch.position;

                float deltaPosX = endTouch.x - beginTouch.x;
                float deltaPosY = endTouch.y - beginTouch.y;

                if (deltaPosX > 0 && Mathf.Abs(deltaPosX) > deltaSwipeMin && Mathf.Abs(deltaPosY) < Mathf.Abs(deltaPosX))
                {
                    return Vector2.right;
                }

                if (deltaPosX < 0 && Mathf.Abs(deltaPosX) > deltaSwipeMin && Mathf.Abs(deltaPosY) < Mathf.Abs(deltaPosX))
                {
                    return Vector2.left;
                }

                if (deltaPosY > 0 && Mathf.Abs(deltaPosY) > deltaSwipeMin && Mathf.Abs(deltaPosX) < Mathf.Abs(deltaPosY))
                {
                    return Vector2.up;
                }

                if (deltaPosY < 0 && Mathf.Abs(deltaPosY) > deltaSwipeMin && Mathf.Abs(deltaPosX) < Mathf.Abs(deltaPosY))
                {
                    return Vector2.down;
                }

            }
        }

        return Vector2.zero;
    }

    /// <summary>
    /// ���������� �����������
    /// </summary>
    /// <returns></returns>
    private Vector2 GetArrow()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Vector2.down;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// �������� �� ���� � ����������
    /// </summary>
    /// <returns></returns>
    private bool IsKeyDown()
    {        
        return Input.anyKeyDown;
    }

    /// <summary>
    /// ����������� ����������� �� ��� X
    /// </summary>
    /// <returns></returns>
    public float InputX()
    {        
        if (IsThereTouchOnScreen())
        {
            return Scope(GetSwipe().x);
        }

        if (IsKeyDown())
        {
            return Scope(GetArrow().x);
        }

        return 0f;
    }

    /// <summary>
    /// ����������� �����������
    /// </summary>
    /// <param name="directionX"></param>
    /// <returns></returns>
    private float Scope(float directionX)
    {        
        int rightPos = (int) Mathf.Floor(gameManager.CountTrack/2);
        int leftPos = rightPos - gameManager.CountTrack + 1;
       
        if (Mathf.Abs(directionX) == 1)
           isSidestep = true;
        
        nowPosition += directionX;

        if (nowPosition > rightPos) { nowPosition = rightPos; isSidestep = false; }
        if (nowPosition < leftPos)  { nowPosition = leftPos; isSidestep = false; }

        if (isSidestep)
        {
            soundManager.SlideStep();

            if (directionX > 0)
                runnerAnim.AnimSidestepRight();
            else
                runnerAnim.AnimSidestepLeft();

            isSidestep = false;
        }
        
        return nowPosition;
    }

    /// <summary>
    /// ����������� �� ��� Y
    /// </summary>
    /// <returns></returns>
    public float InputY()
    {
        if (IsThereTouchOnScreen())
        {
            return GetSwipe().y;
        }

        if (IsKeyDown())
        {
            return GetArrow().y;
        }

        return 0f;
    }

    /// <summary>
    /// ��� �� ���� � ���������� ��� � ������
    /// </summary>
    /// <returns></returns>
    public bool IsInput()
    {        
        if (GetSwipe().x != 0 || IsKeyDown())
            return true;

        return false;
    }

    /// <summary>
    /// �������� �� ���� � ������
    /// </summary>
    /// <returns></returns>
    private bool IsThereTouchOnScreen()
    {
        if (Input.touchCount > 0 && GetSwipe() != Vector2.zero) return true;
        else return false;
    }

}
