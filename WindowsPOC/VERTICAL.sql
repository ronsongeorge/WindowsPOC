
SELECT v.VerticalName as PName,v.SubVerticalName as SUB,AVG(Value) as A1Val ,AVG(TT.Val)
FROM AccountMonthVerticalData V 
LEFT JOIN 
	(SELECT v.VerticalName as PName,v.SubVerticalName as SUB,AVG(Value) as Val 
	FROM AccountMonthVerticalData V 
	WHERE V.MasterID IN
	(SELECT  MasterID from AccountMonthMasterData M where m.Accountid=2)
	
	GROUP BY VerticalName,SubVerticalName
	) TT on TT.PName=v.VerticalName and TT.SUB=v.SubVerticalName
WHERE V.MasterID IN (SELECT  MasterID from AccountMonthMasterData M where m.Accountid=1)
GROUP BY VerticalName,SubVerticalName



