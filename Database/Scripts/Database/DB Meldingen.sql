IF NOT EXISTS ( SELECT 1
                FROM master.sys.databases db
                WHERE db.name = 'Meldingen' )
BEGIN
  CREATE DATABASE Meldingen
    ON  PRIMARY ( NAME = 'Meldingen_data_01'
                 ,FILENAME =  'D:\MSSQL\DATA\Meldingen_01.mdf'
                 ,SIZE = 100MB
                 ,MAXSIZE = UNLIMITED
                 ,FILEGROWTH = 100MB )
    LOG ON ( NAME = 'Meldingen_log_01'
            ,FILENAME = 'E:\MSSQL\DATA\Meldingen_log_01.ldf'
            ,SIZE = 100MB
            ,MAXSIZE = UNLIMITED
            ,FILEGROWTH = 100MB )
END