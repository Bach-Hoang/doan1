CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

CREATE TABLE TableFood
(
ID INT IDENTITY PRIMARY KEY,
NAME NVARCHAR(100) NOT NULL DEFAULT N'ch?a ??t tên',
STATUS NVARCHAR(100) NOT NULL DEFAULT N'Tr?ng',

)
GO

CREATE TABLE ACCOUNT
(
USERNAME NVARCHAR(100) PRIMARY KEY,
DISPLAYNAME NVARCHAR(100) NOT NULL DEFAULT N'ch?a ??t tên',
PASSWORD NVARCHAR(1000) NOT NULL DEFAULT 0,
TYPE INT NOT NULL DEFAULT 0
)
GO

CREATE TABLE FOODCATEGORY
(
ID INT IDENTITY PRIMARY KEY,
name NVARCHAR(100) DEFAULT N'ch?a ??t tên'
)
GO

CREATE TABLE FOOD
(
ID INT IDENTITY PRIMARY KEY,
NAME NVARCHAR(100) NOT NULL DEFAULT N'ch?a ??t tên' ,
IDCATEGORY INT NOT NULL,
PRICE INT NOT NULL DEFAULT 0
FOREIGN KEY (IDCATEGORY) REFERENCES dbo.FOODCATEGORY(ID)
)
GO

CREATE TABLE BILL
(
ID INT IDENTITY PRIMARY KEY,
DATECHECKIN DATE NOT NULL DEFAULT GETDATE(),
DATECHECKOUT DATE,
IDTABLE INT NOT NULL,
STATUS INT NOT NULL DEFAULT 0
FOREIGN KEY (IDTABLE) REFERENCES dbo.TABLEFOOD(ID)
)
GO

CREATE TABLE BILLINFO
(
ID INT IDENTITY PRIMARY KEY,
IDBILL INT NOT NULL,
IDFOOD INT NOT NULL,
COUNT INT NOT NULL DEFAULT 0
FOREIGN KEY (IDBILL) REFERENCES dbo.BILL(ID),
FOREIGN KEY (IDFOOD) REFERENCES dbo.FOOD(ID)
)
GO


INSERT INTO dbo.ACCOUNT
	( UserName,
	DisplayName,
	Password,
	Type
	)
VALUES ( N'K9', 
		N'RongK9',
		N'1',
		1
		)

INSERT INTO dbo.ACCOUNT
	( UserName,
	DisplayName,
	Password,
	Type
	)
VALUES ( N'staff', 
		N'Nhân viên',
		N'1',
		0
		)
GO

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS
BEGIN 
	SELECT * FROM dbo.ACCOUNT WHERE UserName = @userName
END
GO

GO
CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.ACCOUNT WHERE UserName = @userName AND PassWord = @passWord
END
GO


GO
CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
GO

--Thêm bàn
DECLARE @i INT = 1

WHILE @i <= 10
BEGIN
	INSERT dbo.TableFood (NAME) VALUES (N'Bàn ' + cast(@i AS nvarchar(100)))
	SET @i = @i + 1
END

--Thêm category
INSERT dbo.FOODCATEGORY
		(name)
VALUES ( N'H?i s?n')

INSERT dbo.FOODCATEGORY
		(name)
VALUES ( N'Nông s?n')

INSERT dbo.FOODCATEGORY
		(name)
VALUES ( N'Lâm s?n')

INSERT dbo.FOODCATEGORY
		(name)
VALUES ( N'N??c')

--Thêm món ?n
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'M?c',1,100000)
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'Nghêu',1,60000)
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'H??u',3,300000)
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'L?n',2,150000)
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'Coke',4,10000)
INSERT dbo.FOOD (name,IDCATEGORY,PRICE) VALUES (N'7Up',4,10000)

--Thêm Bill
INSERT dbo.Bill (DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values (getdate(),null,1,0)
INSERT dbo.Bill (DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values (getdate(),null,2,0)
INSERT dbo.Bill (DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values (getdate(),getdate(),2,1)
INSERT dbo.Bill (DATECHECKIN,DATECHECKOUT,IDTABLE,STATUS) values (getdate(),null,3,0)
 
 --Thêm BillInfo
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (1,1,2)
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (1,3,4)
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (1,5,1)
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (2,2,2)
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (2,1,5)
 INSERT dbo.BILLINFO (IDBILL,IDFOOD,count) values (3,1,5)

 --reset ID
DELETE FROM DBO.BILL
DBCC CHECKIDENT ('dbo.BillInfo', RESEED, 0);
select * from dbo.billinfo

GO
CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN 
	INSERT dbo.Bill 
	( DateCheckIn,
		DateCheckOut,
		idTable, 
		status,
		discount
	)
	VALUES ( GETDATE(),
				NULL,
				@idTable,
				0,
				0
				)
END 
GO

GO
CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	
	DECLARE @isExitBillInfo INT
	DECLARE @foodCount INT = 1
	SELECT @isExitBillInfo = ID,@foodCount = count FROM dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExitBillInfo>0)
	BEGIN 
		DECLARE @newCount INT = @foodCount + @count
		IF(@newCount > 0)
			UPDATE dbo.BILLINFO SET count = @foodCount + @count WHERE idFood = @idFood AND IDBILL = @idBill
		ELSE 
			DELETE dbo.BILLINFO WHERE IDBILL= @idBill AND IDFOOD = @idFood
	END
	ELSE
	BEGIN
	INSERT dbo.BILLINFO
	(	idBill,
		idFood,
		Count
	)
	VALUES
	(	@idBill,
		@idFood,
		@count
	)
	END
END
GO

	
ALTER TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS 
BEGIN
	DECLARE @idBill int

	select @idBill=idBill FROM inserted

	DECLARE @idTable int

	SELECT @idTable=IDTABLE From dbo.BILL where id = @idBill and status = 0

	DECLARE @count INT

	SELECT @count = count(*) FROM dbo.BILLINFO WHERE IDBILL = @idBill
	IF(@count>0)
		UPDATE dbo.TableFood SET STATUS = N'Có người' WHERE ID = @IDTABLE
	ELSE
		UPDATE dbo.TableFood SET STATUS = N'Trống' WHERE ID = @IDTABLE
	
END
GO



CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT

	SELECT @idBill = id FROM inserted

	DECLARE @idTable int

	SELECT @idTable=IDTABLE From dbo.BILL where id = @idBill
	
	DECLARE @count INT

	SELECT @count = count(*) FROM dbo.Bill WHERE IDTABLE = @idTable and status = 0

	IF(@count = 0)
		UPDATE dbo.TableFood SET STATUS = N'Tr?ng' WHERE id = @idTable
END
GO

GO
CREATE	 PROC USP_SwitchTable
@idTable1 INT, @idTable2 INT
AS
BEGIN
	
	DECLARE @idFirstBill INT

	DECLARE @idSecondBill INT

	DECLARE @isFirstTableEmpty INT = 1

	DECLARE @isSecondTableEmpty INT = 1
	
	SELECT @idFirstBill = id FROM dbo.BILL WHERE IDTABLE = @idTable1 AND STATUS = 0

	SELECT @idSecondBill = id FROM dbo.BILL WHERE IDTABLE = @idTable2 AND STATUS = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill 
	( DateCheckIn,
		DateCheckOut,
		idTable, 
		status,
		discount
	)
	VALUES ( GETDATE(),
				NULL,
				@idTable1,
				0,
				0
				) 
	SELECT  @idFirstBill = MAX(ID) FROM dbo.Bill WHERE IDTABLE = @idTable1 AND STATUS = 0

	END

	SELECT @isFirstTableEmpty = count(*) FROM dbo.BILLInfo WHERE IDBILL = @idFirstBill

	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill 
	( DateCheckIn,
		DateCheckOut,
		idTable, 
		status,
		discount
	)
	VALUES ( GETDATE(),
				NULL,
				@idTable2,
				0,
				0
				) 
	SELECT  @idSecondBill = MAX(ID) FROM dbo.Bill WHERE IDTABLE = @idTable2 AND STATUS = 0

	END

	SELECT @isSecondTableEmpty = count(*) FROM dbo.BILLInfo WHERE IDBILL = @idSecondBill

	select ID INTO IDBillInfoTable from dbo.BILLINFO WHERE IDBILL =@idSecondBill

	UPDATE dbo.BILLINFO SET IDBILL = @idSecondBill WHERE  IDBILL = @idFirstBill

	UPDATE BILLINFO SET IDBILL = @idFirstBill WHERE  ID IN (select * from IDBillInfoTable )

	DROP TABLE IDBillInfoTable

	IF(@isFirstTableEmpty=0)
		UPDATE dbo.TableFood SET STATUS = N'Trống' WHERE ID = @idTable2

	IF(@isSecondTableEmpty=0)
		UPDATE dbo.TableFood SET STATUS = N'Trống' WHERE ID = @idTable1

END
GO


UPDATE TableFood SET STATUS = N'Trống'