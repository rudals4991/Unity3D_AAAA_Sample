using System;
using System.Collections.Generic;
using UnityEngine;

// 객체의 연결을 도와주는 DI (의존성 주입 Dependency Injection) 방식의 스크립트입니다.
public static class DIContainer
{
    //Type을 key로, 실제 인스턴스를 value로 저장하는 Dictionary입니다.
    private static readonly Dictionary<Type, object> container = new();
    
    public static void Register<T>(T instance) where T : class
    {
        //특정 타입의 인스턴스 (스크립트)를 container에 등록하는 메서드입니다.
        //DI 규칙: 등록(Reigster)는 무조건 Awake, 혹은 Initialize에서 가장 먼저 실행합니다.
        container[typeof(T)] = instance;
    }
    public static T Resolve<T>() where T : class
    {
        //저장된 인스턴스를 찾는 메서드입니다.
        container.TryGetValue(typeof(T), out var instance); //container에서 타입에 해당하는 인스턴스를 가져옵니다.
        return instance as T; // T로 캐스팅해서 반환합니다.
    }
}
