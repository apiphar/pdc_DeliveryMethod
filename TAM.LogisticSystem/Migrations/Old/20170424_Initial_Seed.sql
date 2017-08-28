INSERT INTO [dbo].[AppMenu]
           ([AppMenuName])
     VALUES
           ('MENU_ADMIN')
GO

INSERT INTO [dbo].[AppRole]
           ([AppRoleName])
     VALUES
           ('SUPER_ADMIN')
GO

INSERT INTO [dbo].[AppRoleMenuMapping]
           ([AppRoleName]
           ,[AppMenuName])
     VALUES
           ('SUPER_ADMIN'
           ,'MENU_ADMIN')
GO

