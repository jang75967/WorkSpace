# Å°¿öµå

1. Dependency Injection
2. ÀÇÁ¸¼º ¿ªÀü ¿øÄ¢(DIP)
3. EF Core
4. Repository Pattern
5. ÅëÇÕ Å×½ºÆ®(Container)

* DI¸¸ Á¶Á¤ÇÏ¸ç, Postgresql -> Oracle -> MsSQL-> MongoDB¸¦ ÀÚÀ¯·Ó°Ô º¯°æ °¡´É
* Docker¸¦ »ç¿ëÇØ¼­ IntegratedTest¸¦ »ç¿ë ÇÒ ¶§ Database¸¦ ÀÚµ¿À¸·Î ±¸ÃàÇØ¼­ »ç¿ëÇÏµµ·Ï ÇÔ

## Database º¯°æ Å×½ºÆ®

* 4Â÷¿¡¼­´Â Postgres¿Í MongoDB¸¸ ³²±â°í MSSQL°ú OracleÀº Á¦°Å ÇÒ ¿¹Á¤

#### 1. Postgresql, MsSQL

* µû·Î ¼³Á¤ ÇÒ ÇÊ¿ä ¾øÀÌ Å×½ºÆ® ÄÚµå ½ÇÇà °¡´É
* PostgresFactory.csÀÇ Line 24¿¡¼­ BreakPoint¸¦ Âï°í Á¢¼Ó URL¸¦ È®ÀÎ °¡´É(Port ¹ÙÀÎµùÀÌ ·£´ıÀ¸·Î µÇ±â ¶§¹®)
* GetConnectionString()¸¦ F12·Î µé¾î°¡º¸¸é 
Database: Postgres
Username : postgres
Password : postgres
·Î µÇ¾î ÀÖÀ½
Port´Â 5432ÀÌÁö¸¸ Container°¡ ¶ç¿öÁö¸é¼­ ·£´ı Port·Î Æ÷¿öµù µÇ±â ¶§¹®¿¡ È®ÀÎÇØ¼­ Á¢¼ÓÇØ¾ßÇÔ.


#### 3. ¿À¶óÅ¬

* ºñÃß Docker Image 4.5GB
* ½ÇÇàÇÏ°í ½ÍÀ¸¸é, Migrations Æú´õ¿¡¼­ migration.bat ½ÇÇàÇØ¾ßÇÔ.


## ApplicationDbContext Ãß°¡ ¼³¸í

* ÀÎÅÍ³İ¿¡ CleanArchitecture ¼Ò½º ÄÚµå³ª ADCEdge ¼Ò½º ÄÚµå¸¦ º¸¸é ApplicationDbContext°¡ Application Layer¿¡ Á¸ÀçÇÔ.
* ÀÌÀ¯´Â EF Core ÀÚÃ¼°¡ Ãß»óÈ­ µÇ¾î ÀÖ´Ù°í ÆÇ´ÜÇÏ±â ¶§¹®¿¡
  * ½±°Ô ÀÌ¾ß±âÇÏ¸é EF Core°¡ Oracle, Postgres, MsSQL µî ¾öÃ» ¸¹Àº Database¸¦ Áö¿øÇÏ±â ¶§¹®¿¡ µû·Î Application Layer¿¡¼­ Ãß»óÈ­ ÇÒ ÇÊ¿ä°¡ ¾ø´Ù
* ÇÏÁö¸¸ ¿©±â¼­´Â IRepository¸¦ ¸¸µé°í ApplicationDbContext°¡ Infrastructure Layer¿¡ Á¸Àç
* ÀÌÀ¯´Â EF Core°¡ MongoDB¸¦ Áö¿øÇÏÁö ¾Ê°í, EF Core <-> Dapper¸¦ º¯°æ °¡´ÉÇÏµµ·Ï ¸¸µé±â À§ÇÔ


## °úÁ¦(ÇÏ°í ½ÍÀ¸¸é ½Ãµµ)
* Dapper¸¦ Ãß°¡ÇÑ´Ù.
* Migrations(https://fluentmigrator.github.io/) »ç¿ëÇÏ¸é ‰?