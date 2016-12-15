﻿using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    [SerializeField]
    float _bulletSpeed = 5;
    [SerializeField]
    float _rotateSpeed = 100;

    [SerializeField]
    GameObject _deathEffect;

    public Vector3 _targetDirection;
    bool _isBlow  = false;

    [SerializeField]
    bool _doChaseToPlayer = false;


	void Update()
    {   
        if (_isBlow)
        {
            return;
        }

        //チェイス
        if (_doChaseToPlayer)
        {
            HormingToTarget();
        }
        // 前進
        transform.position += transform.TransformDirection(Vector3.forward) * _bulletSpeed * Time.deltaTime;



	}

    void HormingToTarget()
    {

        //FixMe:毎回みないこと
        Vector3 player = GameObject.FindGameObjectWithTag(TagName.Player).transform.position;

        // ターゲットまでの角度を取得
        Vector3    vecTarget  = player - transform.position; // ターゲットへのベクトル
        Vector3    vecForward = transform.TransformDirection(Vector3.forward);   // 弾の正面ベクトル
        float      angleDiff  = Vector3.Angle(vecForward, vecTarget);            // ターゲットまでの角度
        float      angleAdd   = (_rotateSpeed * Time.deltaTime);                              // 回転角
        Quaternion rotTarget  = Quaternion.LookRotation(vecTarget);              // ターゲットへ向けるクォータニオン
        if (angleDiff <= angleAdd)
        {
            // ターゲットが回転角以内なら完全にターゲットの方を向く
            transform.rotation = rotTarget;
        }
        else
        {
            // ターゲットが回転角の外なら、指定角度だけターゲットに向ける
            float t = (angleAdd / angleDiff);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, t);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        //敵キャラ自信にあたってもスルー Todo : 見栄えが悪かったら調整
        if (other.gameObject.tag == TagName.Enemy || other.gameObject.tag == TagName.Finish || other.gameObject.tag == TagName.EnemyBullet)
        {
            return;
        }
        
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.tag == "Bullet")
            {
                //エフェクト
                var effect = Instantiate(_deathEffect);
                effect.transform.position = transform.position;
                //音
                //
                //AudioManager.instance.play3DSe(effect, AudioName.SeName.Thunder);

                //判定消す
                _isBlow = true;
                this.GetComponent<Collider>().enabled = false;
                StartCoroutine(RandomBlow());
                //プレイヤーのたまを消す
                Destroy(other.gameObject);
            }



            return;
        }
        //ScoreManager.instance.AddFlipEnemyBulletCount();
        Destroy(gameObject);
    }



    //ランダムに弾けます
    IEnumerator RandomBlow()
    {
        Vector3 randomDirec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        float deathTime   = 1.0f;
        float acceraition = 2.0f;
        float unacceration = 3.0f;
        float firstTime = 300.0f;

        //初速
        transform.position += randomDirec * (_bulletSpeed + firstTime) * Time.deltaTime;

        //加速
        while (true)
        {   

            transform.position += randomDirec * _bulletSpeed * acceraition * Time.deltaTime;
            acceraition -= Time.deltaTime * unacceration;
            deathTime -= Time.deltaTime;

            if (acceraition < 0)
            {
                break;
            }

            yield return null;
        }

        //Todo :消す時にパッと消えるのいくないかも
        //消す
        Destroy(this.gameObject);

    }


}

