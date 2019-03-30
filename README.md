# Test4Ok
Это решение – результат выполнения тестового задания, условия и пояснения к которому содержатся в PDF-файле.

Источники новостей передаются консольному приложению как аргументы командной строки в виде пар *название URL*, разделённых пробелами или табуляциями. По-умолчанию передаются источники, указанные в PDF-файле.

Перед запуском Web-приложения нужно:
- Установить jQuery с помощью Library Manager (см. [документацию](https://docs.microsoft.com/ru-ru/aspnet/core/client-side/libman/?view=aspnetcore-2.2))
- Установить пакеты с помощью NPM, запустив из консоли `npm install`
- Установить Gulp CLI с помощью NPM, запустив из консоли `npm install -g gulp-cli`
- При необходимости изменить URL к API, присваиваемый константе `apiUrl` в файле *wwwroot\js\script.js*
- Выполнить задачу по-умолчанию с помощью Gulp, запустив из консоли `gulp`

По-умолчанию в качестве СУБД используется Microsoft SQL Server (LocalDB), но можно использовать SQLite. Для этого надо или в файле *sharedappsettings.json* решения, или в *appsettings.json* проектов изменить значение свойства `UseSqlServer` на `false`. Кроме этого, нужно вручную скопировать файл базы данных (по-умолчанию *News.db*) из папки консольного приложения в папку Web-приложения.
