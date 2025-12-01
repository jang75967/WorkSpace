FROM dms/aspnet:8.0 AS base


RUN addgroup -gid 1001 mirero && adduser -uid 1001 -gid 1001 mirero
RUN echo 'mirero ALL=(ALL) NOPASSWD:/bin/umount,/bin/mount' >> /etc/sudoers
RUN mkdir -p /mnt/input && chmod 775 /mnt/input && chown -R mirero:mirero /mnt/input

ADD ./app /app
WORKDIR /app

ENTRYPOINT ["dotnet","Service.Loader.dll"]
