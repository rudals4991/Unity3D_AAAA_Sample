using UnityEngine;

//게임의 상태를 정의한 Enum입니다.
public enum GameState
{
    Title, //타이틀 화면 (상태)
    Loading, //로딩 중
    Ready, //게임 시작 전 준비
    Playing, //게임 진행 중
    GameOver, //게임오버
    Pause //게임 정지
}
