
  SELECT 
	  NEWID() AS Id
	  , [PG1].[Classification] AS [Name]
	  , PG1.Node_Path AS [Description] 
	  , Convert(BIT, 0) AS Deleted
	  , SYSUTCDATETIME() AS CreatedOn
	  , SYSUTCDATETIME() AS ModifiedOn
	  
  FROM ProductCategories PG1

  SELECT COUNT(DISTINCT [Classification]), count([Classification]) FROM ProductCategories

