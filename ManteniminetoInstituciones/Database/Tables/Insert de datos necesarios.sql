USE [SIGOB_CONTRALORIA]
GO
SET IDENTITY_INSERT [dbo].[flujos_catalogo] ON 
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (1, N'Flujo Auditor Normal', 1)
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (2, N'Flujo Tecnico', 1)
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (3, N'Flujo Aprobador', 1)
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (4, N'Flujo Encargado', 1)
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (5, N'Flujo Auditor Financiero', 1)
GO
INSERT [dbo].[flujos_catalogo] ([id_flujo], [nombre_flujo], [activo]) VALUES (6, N'Flujo Auditor Legal', 1)
GO
SET IDENTITY_INSERT [dbo].[flujos_catalogo] OFF
GO
SET IDENTITY_INSERT [dbo].[flujo_actividades_config] ON 
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (23, 1, 2, 2086603)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (27, 1, 4, 2083555)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (20, 1, 4, 2084482)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (17, 1, 4, 2086024)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (14, 1, 4, 2086408)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (24, 1, 4, 2086465)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (25, 1, 5, 2083536)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (18, 1, 5, 2084472)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (15, 1, 5, 2086023)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (12, 1, 5, 2086407)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (21, 1, 5, 2086614)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (26, 1, 6, 2083535)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (19, 1, 6, 2084453)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (16, 1, 6, 2086022)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (13, 1, 6, 2086406)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (22, 1, 6, 2086604)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (35, 2011, 1, 6930014)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (34, 2011, 1, 6930025)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (32, 2011, 1, 6930035)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (30, 2011, 1, 6930044)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (33, 2011, 1, 6930075)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (31, 2011, 1, 19243398)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (45, 2011, 2, 6930085)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (44, 2011, 3, 19243559)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (36, 2011, 4, 6928906)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (43, 2011, 4, 6930015)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (42, 2011, 4, 6930026)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (39, 2011, 4, 6930036)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (37, 2011, 4, 6930045)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (40, 2011, 4, 6930056)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (41, 2011, 4, 6930076)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (38, 2011, 4, 19243463)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (28, 2020, 1, 6928905)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (29, 2020, 4, 6928906)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (3, 2033, 1, 6929977)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (4, 2033, 1, 6929983)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (5, 2033, 1, 6930001)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (6, 2033, 1, 6930008)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (7, 2033, 1, 7070806)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (8, 2033, 1, 8705434)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (9, 2033, 1, 8706533)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (10, 2033, 2, 6930002)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (58, 2033, 3, 6929981)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (57, 2033, 3, 6929985)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (56, 2033, 3, 6930006)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (54, 2033, 3, 6930010)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (55, 2033, 3, 7070810)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (60, 2033, 3, 8705437)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (59, 2033, 3, 8706536)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (51, 2033, 4, 6929978)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (50, 2033, 4, 6929984)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (49, 2033, 4, 6930003)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (46, 2033, 4, 6930009)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (47, 2033, 4, 7070807)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (48, 2033, 4, 7070813)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (53, 2033, 4, 8705435)
GO
INSERT [dbo].[flujo_actividades_config] ([id_config], [codigo_tre], [id_flujo], [codigo_actividad]) VALUES (52, 2033, 4, 8706534)
GO
SET IDENTITY_INSERT [dbo].[flujo_actividades_config] OFF
GO
