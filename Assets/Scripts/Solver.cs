﻿using UnityEngine;
using System.Collections;

public class Solver : MonoBehaviour {

    public RubiksCubePrefab RCP;

	// Use this for initialization
	void Start () {
        RCP.RC.Scramble(50);
        Stage2();
        Stage3();
        RCP.RC.turnCubeZ(true);
        RCP.RC.turnCubeZ(true);
	}

    void Stage2()//Solve the white cross
    {

        for (int i = 0; i < 4; i++)
        {
            Color TargetColor = RCP.RC.cubeMatrix[1][1][0].getColor(Cube.sides.FRONT);
            //Debug.Log("TargetColor: " + TargetColor);
            Vector3 Pos = RCP.RC.sideCubeWithColors(TargetColor, Cube.WHITECOLOR);
            //Debug.Log("Pos: " + Pos);
            Vector3 TargetPos = new Vector3(1, 2, 0);
            //Debug.Log("TargetPos: " + TargetPos);
            if (TargetPos != Pos)//if the cube is not where it should be
            {
                if (Pos.y == 2)//is on top row
                {
                    if (Pos.x == 0)
                    {
                        RCP.RC.rotateLeftFace(true);
                    }
                    else if (Pos.x == 2)
                    {
                        RCP.RC.rotateRightFace(false);
                    }
                    else if (Pos.x == 1)
                    {
                        RCP.RC.rotateBackFace(true);
                        RCP.RC.rotateBackFace(true);
                    }

                }
                else if (Pos.y == 1 && Pos.z == 2)//middle row on the back
                {
                    if (Pos.x == 0)
                    {
                        RCP.RC.rotateBackFace(true);
                        RCP.RC.rotateBottomFace(true);
                        RCP.RC.rotateBackFace(false);
                    }
                    if (Pos.x == 2)
                    {
                        RCP.RC.rotateBackFace(false);
                        RCP.RC.rotateBottomFace(true);
                        RCP.RC.rotateBackFace(true);
                    }
                }

                //cube is now on bottom or front
                //requery pos
                Pos = RCP.RC.sideCubeWithColors(TargetColor, Cube.WHITECOLOR);
                if (Pos.y == 0)//is on bottom
                {
                    while (Pos.z != 0)//while cube is not on the bottom
                    {
                        RCP.RC.rotateBottomFace(true);
                        Pos = RCP.RC.sideCubeWithColors(TargetColor, Cube.WHITECOLOR);
                    }
                }
                while (Pos.y != 2)
                {
                    RCP.RC.rotateFrontFace(true);
                    Pos = RCP.RC.sideCubeWithColors(TargetColor, Cube.WHITECOLOR);
                }
            }
            //cube is now where it should be
            Cube temp = RCP.RC.cubeMatrix[(int)TargetPos.x][(int)TargetPos.y][(int)TargetPos.z];

            if (temp.getColor(Cube.sides.TOP) != Cube.WHITECOLOR)
            {
                Debug.Log("running sequence 0");
                RCP.RC.turnCubeY(true);
                RCP.RC.RunSequence(0);
                RCP.RC.turnCubeY(false);
            }
            RCP.RC.turnCubeY(true);
        }
    }
	
    void Stage3()//sove the white corners
    {
        for (int i = 0; i < 4; i++)
        {
            Color TargetColorA = RCP.RC.cubeMatrix[1][1][0].getColor(Cube.sides.FRONT);//front center
            Color TargetColorB = RCP.RC.cubeMatrix[2][1][1].getColor(Cube.sides.RIGHT);//right center
            Debug.Log("TargetColors: " + TargetColorA + TargetColorB);
            Vector3 Pos = RCP.RC.cornerCubeWithColors(TargetColorA, TargetColorB, Cube.WHITECOLOR);
            Debug.Log("Pos: " + Pos);
            Vector3 TargetPos = new Vector3(2, 2, 0);
            Debug.Log("TargetPos: " + TargetPos);

            if (TargetPos != Pos)//not in the right spot
            {
                if (Pos.y == 2)//in the top but in the wrong spot
                {
                    if (Pos.z == 0)//top front left pos
                    {
                        RCP.RC.rotateLeftFace(true);
                        RCP.RC.rotateBottomFace(true);
                        RCP.RC.rotateLeftFace(false);
                    }
                    if (Pos.z == 2)//top back left or top back right
                    {
                        if (Pos.x == 0)
                        {
                            RCP.RC.rotateLeftFace(false);
                            RCP.RC.rotateBottomFace(true);
                            RCP.RC.rotateLeftFace(true);
                        }
                        if (Pos.x == 2)
                        {
                            RCP.RC.rotateRightFace(true);
                            RCP.RC.rotateBottomFace(true);
                            RCP.RC.rotateRightFace(false);
                        }
                    }
                }
                //cube is now on the bottom
                Pos = RCP.RC.cornerCubeWithColors(TargetColorA, TargetColorB, Cube.WHITECOLOR);
                while (Pos != new Vector3(2, 0, 0))
                {
                    RCP.RC.rotateBottomFace(true);
                    Pos = RCP.RC.cornerCubeWithColors(TargetColorA, TargetColorB, Cube.WHITECOLOR);
                }
                //cube should now be directly below it's target position or already in target position
            }

            while (true)
            {
                Pos = RCP.RC.cornerCubeWithColors(TargetColorA, TargetColorB, Cube.WHITECOLOR);
                Cube tempCube = RCP.RC.cubeMatrix[(int)Pos.x][(int)Pos.y][(int)Pos.z];
                if (Pos == TargetPos && tempCube.getColor(Cube.sides.FRONT) == TargetColorA && tempCube.getColor(Cube.sides.TOP) == Cube.WHITECOLOR)
                    break;

                RCP.RC.RunSequence(1);
            }

            RCP.RC.turnCubeY(true);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
