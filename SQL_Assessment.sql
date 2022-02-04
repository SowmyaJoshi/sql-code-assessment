
--Reproduce given input table with values 

CREATE TABLE Table1 (Date1 datetime,Text1 varchar(20)) 

INSERT INTO Table1 values ('2020-01-31 12:05','My Big idea') 
INSERT INTO Table1 values ('2020-01-01 11:20','Another Big idea')
INSERT INTO Table1 values ('2019-12-03 09:18','A Bad idea')
INSERT INTO Table1 values ('2017-07-04 08:25','The worst idea yet')

--Create temporary table with the specified columns

SELECT 
	Date1 AS  StartDate,
	NULL as EndDate,
	Text1  
INTO #temptable 
FROM Table1

--Use LEAD function to get the enddate from startdate by fetching row below the current row 
SELECT
    StartDate, 
    LEAD(T.StartDate) OVER (ORDER BY t.StartDate) AS EndDate,
    Text1
FROM 
    #temptable t
	ORDER BY t.StartDate

GO
