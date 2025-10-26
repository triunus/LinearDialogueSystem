## 프로젝트 개요
 - 해당 Unity 프로젝트는 2D 일러스트 기반의 '대화 시스템'을 구현한 프로젝트입니다.
 - '대화 시스템'에서 수행 가능한 '다양한 연출'에 대응할 수 있는 프로젝트를 목표로 합니다.

## 주요 기능
 - 텍스트 출력.
 - 선택지를 통한, 연출 흐름 분기.
 - '연출'의 확장 용이성 확보.
   ( '연출'의 확장이 용이하도록, '연출 데이터'의 특정 속성을 통하여 '연출 View'를 구분하여 동작시킵니다. )
 - '2D 대화 캐릭터' 배치, 활성/비활성, FadeIn/Out, Move 등 행동 수행 기능 구현.
 - '행동'의 확장 용이성 확보.
   ( '행동'의 확장이 용이하도록, 각 '행동'에 대한 Facede - PlugInHub - PlugInView 방식으로 구현. )
 - '연출 대상'의 확장 용이성 확보.
   ( '행동'의 대상을 '2D 대화 캐릭터'로 한정하지 않고, 데이터를 통해 전달되는 'Key'값만 일치하면 동작하도록 구현. )
 - '연출 데이터'에 '다음 연출 번로 지정 방식'을 명시하는 NextDirectiveCommand 속성을 추가하여, 데이터를 통한 연출 흐름 제어 수행.

 - '마우스 입력'을 통한 '연출 제어'
   ( '텍스트 출력'의 완성 & '연출 애니메이션'의 상태를 '마지막 상태값'으로 지정. )
 - '자동 버튼'을 통한 일정 시각 후 자동 연출 재생.

## 구현 필요 기능
 - '구글 시트'를 통한 Json으로 Response 받아서, 파싱하여 '연출 테이블' 등록하는 기능
 - '로그 기능' 구현.

ps. '연출 테이블', '2D 캐릭터 이미지' 등 다양한 '리소스 로드' 기능은 해당 '프로젝트 기획'에 따라 달라지는 경우가 다분하여 제외할 생각합니다. )


## 관련 UML

-- 임시 기록 중 -- 
#### 수정된 E-R 다이어그램 : https://drive.google.com/file/d/1sp9_nr6cUr5Tn_hoPtcfk-kMK_NvdQdR/view?usp=sharing 
#### 수정된 예상 작성 형태 : https://docs.google.com/spreadsheets/d/1_u7W4s_oY4yn4f57er6G2Q-WMZA8heKXz8naUUmPWXM/edit?usp=sharing
#### 수정전 FlowChart : https://drive.google.com/file/d/1OOkjpIC_HJHOJNVN_h2qunzeoP-QmN28/view?usp=sharing
#### 수정된 FlowChart : https://drive.google.com/file/d/1G8QYRILhcR2k8LiYxUQGoUUIDBACHaHR/view?usp=sharing


-- 임시 결과 -- 
#### E-R 다이어그램 : 정리 후 추가 예정
 ( 핵심은, 서로 다른 연출에 필요한 데이터를 '_'와 같은 노이즈를 통해 1개의 연출 데이터 속성값으로 변경하여 관리하는 것입니다. )
#### Flow Chart : 정리 후 추가 예정
 ( 구현 전 전체적인 흐름 파악 + ToDo 리스트 + 기억 상기 용도 )
#### 클래스 다이어그램 : https://drive.google.com/file/d/1PrKH2Mx-1BUf-ggsV0x5T4CmjUCTo-hL/view?usp=sharing
 ( 주로 기억 상기시키는 용도로 사용합니다. )
#### 연출 데이터 테이블 작성 방식 : https://docs.google.com/spreadsheets/d/1LWUHIGT2I7f-VWlpmZkNvObnse6o2H_VeaBodFuuZs4/edit?usp=sharing
 ( '스토리' 작업자에게 부담을 최대한 줄이기 위한 작업, 피드백으로 통해 지속적으로 개선할 예정입니다. )
