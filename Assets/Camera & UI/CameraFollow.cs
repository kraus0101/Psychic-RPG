using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Todo 카메라 스타일 결정 
    //1. 플레이어를 중심으로 돈다 (카메라 로테이션 스크립) (다크소울 스타일) 2. 플레이어 뒷모습만 보여지며 마우스는 화면에 고정되어있. (마우스 고정 스크립트) ( 사이퍼즈 스타일)
    //TODO make Enemy Targeting

    GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
