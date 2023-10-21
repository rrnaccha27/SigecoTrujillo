CREATE PROCEDURE [dbo].[SIGEMO_listarCuotaContratoCliente]
  @codigoContrato VARCHAR(50)
    
    
AS
BEGIN
    SET NOCOUNT ON;

if(@codigoContrato='1202') 
begin
 select '01' cuota  ,'10/12/2017' fecha, '563.23' monto , 'S/.' moneda,  'Atrasado' estado
 union all
  select '02' cuota  ,'10/01/2018' fecha, '563.23' monto , 'S/.' moneda, 'Pendiente' estado
   union all
  select '03' cuota  ,'10/02/2018' fecha, '563.23' monto , 'S/.' moneda, 'Pendiente' estado 
   union all
  select '04' cuota  ,'10/03/2018' fecha, '563.23' monto , 'S/.' moneda, 'Pendiente' estado
   union all
  select '05' cuota  ,'10/04/2018' fecha, '563.23' monto , 'S/.' moneda, 'Pendiente' estado
  union 
  select '06' cuota  ,'10/05/2018' fecha, '563.23' monto , 'S/.' moneda, 'Pendiente' estado
end
else
begin
select '01' cuota  ,'03/11/2017' fecha, '250' monto , 'S/.' moneda,  'Atrasado' estado
 union all
  select '02' cuota  ,'03/12/2018' fecha, '250' monto , 'S/.' moneda, 'Atrasado' estado
   union all
  select '03' cuota  ,'03/01/2018' fecha, '250' monto , 'S/.' moneda, 'Pendiente' estado 
   union all
  select '04' cuota  ,'03/02/2018' fecha, '250' monto , 'S/.' moneda, 'Pendiente' estado
   union all
  select '05' cuota  ,'03/03/2018' fecha, '250' monto , 'S/.' moneda, 'Pendiente' estado
  union all
  select '06' cuota  ,'03/04/2018' fecha, '250' monto , 'S/.' moneda, 'Pendiente' estado
end

END