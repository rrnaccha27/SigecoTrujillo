CREATE VIEW dbo.vw_personal
AS
SELECT        
	p.codigo_personal, p.codigo_equivalencia, p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS nombre_personal, p.estado_registro AS estado_persona, p.usuario_registra, p.fecha_registra, 
	p.usuario_modifica, p.fecha_modifica, pcg.codigo_registro AS codigo_canal_grupo, pcg.estado_registro AS estado_canal_grupo, pcg.es_supervisor_canal, pcg.es_supervisor_grupo, canal.codigo_canal_grupo AS codigo_canal, 
	canal.nombre AS nombre_canal, canal.estado_registro AS estado_canal, canal.codigo_equivalencia AS codigo_equivalencia_canal, grupo.codigo_canal_grupo AS codigo_grupo, grupo.nombre AS nombre_grupo, 
	grupo.estado_registro AS estado_grupo, grupo.codigo_equivalencia AS codigo_equivalencia_grupo, pcg.percibe_comision, pcg.percibe_bono, ISNULL(p.correo_electronico, '') AS correo_electronico, p.validado
FROM
	dbo.personal AS p 
INNER JOIN
	dbo.personal_canal_grupo AS pcg ON pcg.codigo_personal = p.codigo_personal 
INNER JOIN
	dbo.canal_grupo AS canal ON pcg.codigo_canal = canal.codigo_canal_grupo 
INNER JOIN
	dbo.canal_grupo AS grupo ON pcg.codigo_canal_grupo = grupo.codigo_canal_grupo
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 257
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pcg"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 234
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "canal"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "grupo"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 532
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_personal';
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_personal';