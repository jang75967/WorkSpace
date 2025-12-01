#!/bin/bash

# 사용법: ./build_and_push.sh k1.0.0

# sudo 권한 확인
if [ "$EUID" -ne 0 ]; then
  echo "❌ 이 스크립트는 sudo 권한으로 실행해야 합니다."
  echo "사용법: sudo $0 <VERSION>"
  exit 1
fi

# 입력값 확인
if [ -z "$1" ]; then
  echo "사용법: $0 <VERSION>"
  exit 1
fi

VERSION=$1
REPO_URL=192.168.70.161:7000

# 서비스 목록
SERVICES=(
  "admintool"
  "datamanager"
  "trigger"
  "detector"
  "loader"
  "transmitter"
  "logmanager"
  "scheduler-eds"
  "scheduler-autodelete"
)

# 각 서비스별 디렉터리 이름 매핑
declare -A DIR_MAP=(
  ["admintool"]="dms.admintool.api"
  ["datamanager"]="dms.datamanager.api"
  ["trigger"]="dms.trigger"
  ["detector"]="dms.detector"
  ["loader"]="dms.loader"
  ["transmitter"]="dms.transmitter"
  ["logmanager"]="dms.logmanager"
  ["scheduler-eds"]="dms.scheduler.eds"
  ["scheduler-autodelete"]="dms.scheduler.autodelete"
)

# 빌드 및 푸시 루프
for SERVICE in "${SERVICES[@]}"; do
  DIR=${DIR_MAP[$SERVICE]}
  IMAGE_NAME=dms/$SERVICE:$VERSION
  FULL_TAG=$REPO_URL/$IMAGE_NAME

  echo "==== $SERVICE 이미지 빌드 시작 ===="
  podman build -t $IMAGE_NAME ./$DIR
  if [ $? -ne 0 ]; then
    echo "❌ $SERVICE 빌드 실패"
    exit 1
  fi

  echo "🔄 태깅: $IMAGE_NAME → $FULL_TAG"
  podman tag $IMAGE_NAME $FULL_TAG

  echo "🚀 푸시: $FULL_TAG"
  podman push $FULL_TAG
  if [ $? -ne 0 ]; then
    echo "❌ $SERVICE 푸시 실패"
    exit 1
  fi

  echo "✅ $SERVICE 완료"
  echo
done

echo "🎉 모든 이미지가 성공적으로 푸시되었습니다."

