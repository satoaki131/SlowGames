﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    Easy,
    Normal,
    Hard,

    Last,
}

public enum GeneratePosition
{

   Left = 0,
   Front,
   Right,

   UpLeft,
   UpFront,
   UpRight,

   Last,
  
}

//enemyを生成
public class EnemyGenerator : MonoBehaviour
{
    
    //雑魚敵
    [SerializeField]
    List<GameObject> _easyEnemys = new List<GameObject>();
    [SerializeField]
    List<GameObject> _normalEnemys = new List<GameObject>();
    [SerializeField]
    List<GameObject> _hardEnemys = new List<GameObject>();


    [SerializeField]
    Transform _left;
    [SerializeField]
    Transform _front;
    [SerializeField]
    Transform _right;

    [SerializeField]
    Transform _upLeft;
    [SerializeField]
    Transform _upFront;
    [SerializeField]
    Transform _upRight;



    //生成するエネミーを記憶 
    Dictionary<EnemyType,List<GameObject>> _enemysDic = new Dictionary<EnemyType, List<GameObject>>();
    Dictionary<GeneratePosition,Transform> _generateDic = new Dictionary<GeneratePosition,Transform>();

    void Awake()
    {
        
        _enemysDic.Add(EnemyType.Easy, _easyEnemys);
        _enemysDic.Add(EnemyType.Normal, _normalEnemys);
        _enemysDic.Add(EnemyType.Hard, _hardEnemys);

        _generateDic.Add(GeneratePosition.Left,_left);
        _generateDic.Add(GeneratePosition.Right,_right);
        _generateDic.Add(GeneratePosition.Front,_front);
        _generateDic.Add(GeneratePosition.UpLeft,_upLeft);
        _generateDic.Add(GeneratePosition.UpRight,_upRight);
        _generateDic.Add(GeneratePosition.UpFront,_upFront);

    }

    [SerializeField]
    EnemyType _testGenerateType;

    //ランダムに,生成位置を取得する
    public  GeneratePosition GetRandomGeneratePos(int[] generateCount, int  enemyLimit = 1)
    {
        //生成可能な,配列番号を記憶する
        List<int> canGeneratePos = new List<int>();

        for (int i = 0; i < generateCount.Length; i++)
        {   
            ////敵キャラがいない、または生成上限に達していない場所だったら.
            if (generateCount[i] <= (enemyLimit - 1))
            {
                //敵キャラがいない、または生成上限に達していない場所の、配列番号を記憶
                canGeneratePos.Add(i);
            }
        }

        //もし生成可能な場所が一つもなければ Lastを返す
        if (canGeneratePos.Count == 0)
        {
            Debug.Log("生成可能な場所がないため GeneratePosition.Lastを返してます");
            return GeneratePosition.Last;
        }

        //生成可能場所からランダムに選ぶ
        int random = Random.Range(0,canGeneratePos.Count);

        return (GeneratePosition)(canGeneratePos[random]);

    }

    //ランダムに,地上の生成位置を取得する
    public  GeneratePosition GetRandomGroundGeneratePos()
    {
        int random = Random.Range(0,3);
        return (GeneratePosition)random;
    }

    //ランダムに,空中の生成位置を取得する
    public  GeneratePosition GetRandomSkyGeneratePos()
    {
        int random = Random.Range(3,(int)GeneratePosition.Last);
        return (GeneratePosition)random;
    }
   

    //generatorの生成位置に敵キャラを配置
    public void GenerateEnemy(EnemyType enemyType, GeneratePosition generatePosition  = GeneratePosition.Front)
    {    

        //選んだエネミータイプから,ランダムでpatternを選び生成.
        var enemyList = _enemysDic[enemyType];
        int random = Random.Range(0, (enemyList.Count));

        //生成
        GameObject enemy = enemyList[random];
        Instantiate(enemy);

        //test: ジェネレーターの場所を基準に生成
        var setTransform = _generateDic[generatePosition];
        enemy.transform.position = setTransform.position;
        enemy.transform.rotation = setTransform.rotation;

        //自分がどこに生成された的なのかをキヲクさせる
        enemy.GetComponent<Enemy>()._generatePostion = generatePosition;

    }

}