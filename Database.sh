//Database-first
dotnet ef dbcontext scaffold "Server=localhost;Database=dotnet1_blog_demo;User=root;Password=123456789;Allow User Variables=True;" Pomelo.EntityFrameworkCore.MySql -o Models/Tables -f
dotnet ef dbcontext scaffold "Server=localhost;Database=dotnet1_blog_demo;User=root;Password=123456789;Allow User Variables=True;" Pomelo.EntityFrameworkCore.MySql -o Models/Tables -c dotnet1_blog_demoContext --force
dotnet ef dbcontext scaffold "Server=localhost;Database=dotnet1_blog_demo;User=root;Password=123456789;Allow User Variables=True;" Pomelo.EntityFrameworkCore.MySql -o Models/Tables -c dotnet1_blog_demoContext --table users --force
dotnet-aspnet-codegenerator controller -name UserController -async -m User -dc dotnet1_blog_demoContext -outDir Controllers

//Code-first
C:\Program Files\MySQL\MySQL Server 8.0\bin // Add to PATH
mysqldump -u root -p --routines --triggers --single-transaction dotnet1_blog_demo > dotnet1_blog_demo_backup.sql //cmd
C:\WINDOWS\system32\ //Check backup
mysql -u root -p dotnet1_blog_demo < dotnet1_blog_demo_backup.sql

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
dotnet ef migrations add InitialCreate
dotnet ef migrations add AddMultipleFieldsToUsers
dotnet ef database update
dotnet ef migrations remove