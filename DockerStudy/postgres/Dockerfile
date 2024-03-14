FROM postgres:latest

#ENV POSTGRES_USER=jdg
#ENV POSTGRES_PASSWORD=7596
#ENV POSTGRES_DB=jdgpostgres

# init.sql 파일을 Docker 컨테이너 내부의 /docker-entrypoint-initdb.d 디렉토리로 복사
COPY init.sql /docker-entrypoint-initdb.d/