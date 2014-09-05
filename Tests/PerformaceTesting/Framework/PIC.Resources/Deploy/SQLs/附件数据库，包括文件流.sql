-- 附件数据库，包括文件系统

CREATE DATABASE [BITC_PicDoc] ON 
( FILENAME = N'D:\PiS\Workspaces\Projs\BiTC\Data\BITC_PicDoc.mdf'), 
( FILENAME = N'D:\PiS\Workspaces\Projs\BiTC\Data\BITC_PicDoc_log.ldf'), 
FILEGROUP [BiTC_PicDoc_fs] CONTAINS FILESTREAM DEFAULT 
( NAME = N'BiTC_PicDoc_fs', FILENAME = N'D:\PiS\Workspaces\Projs\BiTC\Data\BiTC_PicDoc_fs' ) 
FOR ATTACH 