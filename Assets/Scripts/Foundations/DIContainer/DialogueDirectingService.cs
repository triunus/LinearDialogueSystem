using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundations.DIContainer
{
    // 1. Boot에서 런타임 내 사용하고자 하는 시스템 객체 + 시스템 객체 접근 인터페이스 등록.
    //  ( 만약에, 해당 시스템이 배포된다는 가정으로 보면, Boot 클래스 시스템의 시작 지점이 되야 할듯. )
    //  ( 또는, 시스템 지점 + Boot의 교환? 전략패턴느낌으로? 하면, 1개의 시스템이 다양한 기능을 수행하게 할 수 있을 거 같음. )
    // 2. 실제 사용시점에서, 특정 '시스템 객체 접근 인터페이스'를 Resolve 요청.
    //  ( 이때, 해당 시스템이 필요로 하는 모든 의존 객체를 생성 및 의존성을 주입함. )
    //  ( 예상 문제 : '이미 생성한 객체'를 사용하고 싶으면? ---> 객체마다, 스코프 타입을 주는 것으로 해결. )
    //  ( 예상 문제 : '의존 상태가 순환'이 애초에 되면 안되지만, 이걸 Block 하는게 필요하기 해 보임. )
    // 3. 해당 시스템의 모든 접근은 'DI 컨테이너'를 통해 받아온 interface를 통해 수행.
    // 4. 해당 시스템이 필요 없어진 경우, 삭제 --> 관련 의존 객체 전부 삭제.
    //  ( 이때, 해당 시스템을 위해 외부에서 추가된 객체는 어떻게 됨? ---> 애초에 그런 경우가 없어야 되는게 좀 더 패키지 적이지만, 만약에 그런 경우, 객체의 '스코프 타입'을 통해 삭제하지 않을 객체 판단하는게 좋아보임. )
    
    // 5. 유니티 특유 문제 : MoneBehaviour 어떻게 할건데?
    // 6. 내가 주고 싶은 건 : 사용하는 개발자가 안 속이 어떻게 개발되는지 몰라도 되는 것.

    // 아무리 생각해 봐도, DIContainer는 큰 범위에서 사용하는 것으로 판단이 됨.
    // 배포가 가능할 정도로, 큰 범위에서의 연결.
    // 접근에하는 방식에 절대적인 변경이 존재해서는 안되는 영역.
    // 즉, 단위 기능 합성의 최상단이라고 보임.
    // Dialogue의 연출 시스템의 마지막 연결에 고려해보는 정도. 더 확장되는 경우, 해당 부분을 넣을 수 없음.
    // 예를 들어, 공격과 대화가 번갈아가면서 일어나는 경우, DI Container가 Binder가 더 적합함.
    public interface IDialogueDirectingService
    {

    }

    public class DialogueDirectingService
    {
        private IResourcesLoadService ResourcesLoadService;

        // Dialogue의 리소스들을 받아오기 위한, 로드 Service.
        // 
        // 서로 다른 Service의 등록
        // 

        public DialogueDirectingService()
        {

        }
    }

    public interface IResourcesLoadService
    {

    }
}