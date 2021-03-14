using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class RPGPlayerAnimator : MonoBehaviour
{
    private const int MaterialWidth = 102;
    private const int MaterialHeight = 136;
    private const int LineHeight = 32;
    private const int ColumnWidth = 32;
    
    [SerializeField] private Material material = default;
    [SerializeField] private int frameSample = default;
    [SerializeField] private Direction initialDirection = Direction.Down;

    private enum Direction
    {
        Top,
        Down,
        Left,
        Right
    }
    private enum State
    {
        Idle,
        Walk
    }

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Dictionary<Direction, List<Vector2[]>> uvCoordinates;
    private Direction currentDirection;
    private State currentState = State.Idle;
    private int currentFrame = 0;
    private Vector2 direction;
    private float speed = 0f;
    private int frameCount = 0;

    private void Awake()
    {
        currentDirection = initialDirection;
    }

    private void Start()
    {
        InitializeUVCoordinates();
        
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 1);
        vertices[2] = new Vector3(1, 1);
        vertices[3] = new Vector3(1, 0);

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh = new Mesh { vertices = vertices, triangles = triangles };
        UpdateUVCoordinates();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
    }

    private void UpdateUVCoordinates()
    {
        mesh.uv = uvCoordinates[currentDirection][currentFrame];
    }

    private void InitializeUVCoordinates()
    {
        uvCoordinates = new Dictionary<Direction, List<Vector2[]>>
        {
            {Direction.Top, CreateUVCoordinatesForLine(0)},
            {Direction.Down, CreateUVCoordinatesForLine(3)},
            {Direction.Left, CreateUVCoordinatesForLine(2)},
            {Direction.Right, CreateUVCoordinatesForLine(1)}
        };

    }

    private List<Vector2[]> CreateUVCoordinatesForLine(int lineIndex)
    {
        Vector2[] idleUV = ConvertPositionToUVCoordinates(1, lineIndex);
        Vector2[] walking1UV = ConvertPositionToUVCoordinates(0, lineIndex);
        Vector2[] walking2UV = ConvertPositionToUVCoordinates(2, lineIndex);

        List<Vector2[]> coordinates = new List<Vector2[]>
        {
            idleUV, 
            walking1UV, 
            idleUV, 
            walking2UV
        };
        return coordinates;
    }
    private Vector2[] ConvertPositionToUVCoordinates(int columnIndex, int lineIndex)
    {
        return new []
        {
            new Vector2((columnIndex * (ColumnWidth + 2f) + 1f) / MaterialWidth, (lineIndex * (LineHeight + 2f) + 1f) / MaterialHeight),
            new Vector2((columnIndex * (ColumnWidth + 2f) + 1f) / MaterialWidth, ((lineIndex + 1) * (LineHeight + 2f) - 1f) / MaterialHeight),
            new Vector2(((columnIndex + 1) * (ColumnWidth + 2f) - 1f) / MaterialWidth, ((lineIndex + 1) * (LineHeight + 2f) - 1f) / MaterialHeight),
            new Vector2(((columnIndex + 1) * (ColumnWidth + 2f) - 1f) / MaterialWidth, (lineIndex * (LineHeight + 2f) + 1f) / MaterialHeight)
        };
    }

    private void FixedUpdate()
    {
        if (currentState == State.Idle)
            return;

        frameCount++;

        if (frameCount >= frameSample)
        {
            currentFrame = (currentFrame + 1) % 4;
            UpdateUVCoordinates();
            frameCount = 0;
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
        UpdateUVsToRender();
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        UpdateUVsToRender();
    }

    public void SetSpeedAndDirection(float s, Vector3 dir)
    {
        speed = s;
        direction = dir;
        UpdateUVsToRender();
    }

    private void UpdateUVsToRender()
    {
        Direction dir = GuessBestDirection();
        State state = GuessBestState();

        if (currentDirection == dir && currentState == state)
            return;
        
        currentDirection = dir;
        currentState = state;
        currentFrame = 0;
        frameCount = 0;

        UpdateUVCoordinates();
    }

    private Direction GuessBestDirection()
    {
        Direction[] directions = new []
        {
            Direction.Top,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        int position = 0;
        float distance = -1f;

        for (int i = 0; i < 4; i++)
        {
            float magnitude = (DirectionToVector2(directions[i]) - direction).sqrMagnitude;
            if (distance < 0 || magnitude < distance)
            {
                position = i;
                distance = magnitude;
            }
        }
        
        return directions[position];
    }

    private State GuessBestState()
    {
        return Math.Abs(speed) > 0.1 ? State.Walk : State.Idle;
    }

    private Vector2 DirectionToVector2(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return Vector2.down;
            case Direction.Top:
                return Vector2.up;
            case Direction.Left:
                return Vector2.left;
            default:
                return Vector2.right;
        }
    }
}
