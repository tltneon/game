## Онлайн игра по типу travian и ogame

* использование БД, для взаимодействия с БД использовать Entity Framework
* использование WCF сервиса для логики
* использование WebApi
* аутентификация и авторизация для пользователей
* Angular для веб интерфейса
* логирование
* (необязательно) юнит тесты

* развитие базы, добыча ресурсов, производство войск, оборона (лучше сделать простой вариант, потом развить)
* сражения PvE и PvP (**только PvP**)
* UI по большей части можно текстовый
* предусмотреть статистику

**MVP: Добыча ресурсов(минимум 1 ресурс), производство войск(минимум 2 вида войск), оборона, нападение.**

## Запуск решения
### Требуется:
* Visual Studio
* Visual Code с поддержкой TypeScript

### Запуск основного проекта:
* Открыть csharpgame.sln
* Выполнить "Восстановить пакеты через nuget"
* Свойства решения > Запускаемый проект > Несколько запускаемых проектов > Выбрать wcfservice и webapi
* Запустить решение F5

### Запуск Angular 8 UI:
* Открыть папку .\angular в Visual Code
* Выполнить команду npm i
* Выполнить команду ng serve
* Открыть http://localhost:4200/
