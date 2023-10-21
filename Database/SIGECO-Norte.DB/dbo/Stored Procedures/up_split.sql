create proc [dbo].[up_split]  
 (@Cadenas VARCHAR(5000)  )

as
begin        
SET NOCOUNT ON  
  
create TABLE #splitCadena(  
 Cadena varchar(80)  
)        
  
SET @Cadenas = rtrim(ltrim(@Cadenas))  
        
IF @Cadenas <> ''        
BEGIN  
 IF ',' = SUBSTRING(@Cadenas , 1, 1)  
 BEGIN  
  select @Cadenas = SUBSTRING(@Cadenas , 2, len(@Cadenas) - 1)  
 END  
  
  
 IF ',' = SUBSTRING(@Cadenas , len(@Cadenas), len(@Cadenas))  
 BEGIN  
  select @Cadenas = SUBSTRING(@Cadenas , 1, len(@Cadenas)-1)  
 END  
END  
ELSE  
BEGIN  
 SELECT Cadena FROM #splitCadena  
 RETURN 0  
END  
        
Declare  @posicioncoma  smallint  
 ,@longitud     smallint  
 ,@subcadena    varchar(255)  
 ,@CadenasCategoria  varchar(5000)        
  
Select @longitud=len(@Cadenas), @posicioncoma = 1        
  
while @posicioncoma <> 0        
begin        
 Select @posicioncoma = charindex(',',@Cadenas)        
 If @posicioncoma <> 0        
 begin        
  Select @subcadena = substring(@Cadenas, 1, @posicioncoma - 1)        
  Insert into #splitCadena values(convert(varchar(80), @subcadena))        
  Select @Cadenas = substring(@Cadenas, @posicioncoma + 1, @longitud)        
 end        
end        
  
Insert into #splitCadena values(convert(varchar(80), @Cadenas))        
    
SELECT distinct Cadena FROM #splitCadena  
 end;