# 🏛 Задание по созданию API для Instagram

## 🎯 Цель
Создайте API для управления данными Instagram (пользователи, посты, комментарии) с использованием C# и Entity Framework Core. В проекте необходимо настроить базу данных PostgreSQL, применить ограничения на модели, реализовать CRUD-операции, использовать DTO, подключить Swagger и выполнить дополнительные задания с использованием JOIN-запросов.

---

## 🧱 Структура проекта
1. **Domain** — модели данных.  
2. **Infrastructure** — контекст EF Core, конфигурации, миграции.  
3. **WebAPI** — контроллеры, настройка Swagger и подключение к базе.

---

## 1. Модели данных

### 👤 User
| Поле       | Тип           | Ограничения и свойства                              |
|------------|---------------|-----------------------------------------------------|
| `Id`       | `int`         | Первичный ключ                                      |
| `Username` | `string`      | **Обязательное**, уникальное, макс. длина 50       |
| `Email`    | `string`      | **Обязательное**, макс. длина 100                  |
| `Bio`      | `string`      | Макс. длина 200                                    |
| `Posts`    | `List<Post>`  | Навигационное свойство                             |

---

### 📷 Post
| Поле        | Тип           | Ограничения и свойства                              |
|-------------|---------------|-----------------------------------------------------|
| `Id`        | `int`         | Первичный ключ                                      |
| `UserId`    | `int`         | Внешний ключ (ссылка на `User`)                    |
| `Content`   | `string`      | **Обязательное**, макс. длина 500                  |
| `CreatedAt` | `DateTime`    | Дата создания                                      |
| `User`      | `User`        | Навигационное свойство                             |
| `Comments`  | `List<Comment>` | Навигационное свойство                           |

---

### 💬 Comment
| Поле        | Тип           | Ограничения и свойства                              |
|-------------|---------------|-----------------------------------------------------|
| `Id`        | `int`         | Первичный ключ                                      |
| `UserId`    | `int`         | Внешний ключ (ссылка на `User`)                    |
| `PostId`    | `int`         | Внешний ключ (ссылка на `Post`)                    |
| `Text`      | `string`      | **Обязательное**, макс. длина 300                  |
| `CreatedAt` | `DateTime`    | Дата создания                                      |
| `User`      | `User`        | Навигационное свойство                             |
| `Post`      | `Post`        | Навигационное свойство                             |

### Требования к моделям
- Используйте **Data Annotations** (`[Required]`, `[StringLength]`, `[Key]`) для задания ограничений на поля.  
- Продублируйте эти ограничения через **Fluent API** в методе `OnModelCreating`.  
- Настройте связи один-ко-многим (`User` → `Posts`, `Post` → `Comments`, `User` → `Comments`) с помощью **Fluent API**.

---

## 2. CRUD-операции 🛠️
- Создайте контроллеры:  
  - `UsersController`  
  - `PostsController`  
  - `CommentsController`  
- Реализуйте методы:  
  - `GET /users`, `GET /users/{id}` — получение всех пользователей и по ID.  
  - `POST /users` — создание пользователя.  
  - `PUT /users/{id}` — обновление пользователя.  
  - `DELETE /users/{id}` — удаление пользователя.  
  - Аналогичные методы для `Posts` и `Comments`.

---

## 3. DTO
Определите DTO для каждой модели:  

### `UserDto`
- `Id` (int)  
- `Username` (string)  
- `Email` (string)  
- `PostCount` (int) — количество постов пользователя  

### `PostDto`
- `Id` (int)  
- `UserId` (int)  
- `Username` (string) — имя пользователя из `User`  
- `Content` (string)  
- `CreatedAt` (DateTime)  
- `CommentCount` (int) — количество комментариев  

### `CommentDto`
- `Id` (int)  
- `UserId` (int)  
- `Username` (string) — имя пользователя из `User`  
- `PostId` (int)  
- `Text` (string)  
- `CreatedAt` (DateTime)  

- Используйте эти DTO в контроллерах для запросов и ответов.

---

## 4. Swagger 📜
- Установите пакет `Swashbuckle.AspNetCore`.  
- Настройте Swagger-документацию в `Program.cs`, чтобы протестировать API.

---

## 5. Настройка базы данных
### `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=instagramdb;Username=admin;Password=pass123"
  }
}
```
- В `Program.cs` настройте `DbContext` с использованием строки подключения:  
  ```csharp
  builder.Services.AddDbContext<InstagramContext>(options => 
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
  ```

---
Вот обновлённый блок с дополнительными заданиями, оформленный стильно и структурировано: с заголовками, таблицами для DTO, жирным выделением ключевых терминов и эмодзи для наглядности. Каждое задание требует создания нового DTO, как ты просил.

---

## 6. Дополнительные задания 🎯

### 1. **GET /posts/latest**  
Создайте endpoint, который возвращает 5 последних постов. Для этого определите новый **DTO** и настройте запрос с сортировкой.  

- **DTO**: `LatestPostsDto`  
  | Поле         | Тип        | Описание                     |  
  |--------------|------------|------------------------------|  
  | `Content`    | `string`   | Текст поста                 |  
  | `CreatedAt`  | `DateTime` | Дата создания поста         |  
  | `Username`   | `string`   | Имя автора поста            |  
- Отсортируйте посты по **CreatedAt** (от новых к старым).  

---

### 2. **GET /users/{id}/all-posts**  
Реализуйте endpoint, который возвращает все посты пользователя. Используйте новый **DTO** и связь между таблицами `User` и `Post`.  

- **DTO**: `UserPostsDto`  
  | Поле         | Тип        | Описание                     |  
  |--------------|------------|------------------------------|  
  | `Content`    | `string`   | Текст поста                 |  
  | `CreatedAt`  | `DateTime` | Дата создания поста         |  
  | `Username`   | `string`   | Имя автора поста            |  

---

### 3. **GET /posts/{id}/with-author**  
Добавьте endpoint, который возвращает данные одного поста по его ID. Создайте новый **DTO** для вывода информации.  

- **DTO**: `PostAuthorDto`  
  | Поле         | Тип        | Описание                     |  
  |--------------|------------|------------------------------|  
  | `Content`    | `string`   | Текст поста                 |  
  | `CreatedAt`  | `DateTime` | Дата создания поста         |  
  | `Username`   | `string`   | Имя автора поста            |  

---

### 4. **GET /users/top-posters**  
Реализуйте endpoint, который возвращает 3 пользователей с наибольшим количеством постов. Определите новый **DTO** и используйте сортировку.  

- **DTO**: `TopPosterDto`  
  | Поле         | Тип        | Описание                     |  
  |--------------|------------|------------------------------|  
  | `Username`   | `string`   | Имя пользователя            |  
  | `Email`      | `string`   | Email пользователя          |  
  | `PostCount`  | `int`      | Количество постов           |  
- Отсортируйте по **PostCount** (по убыванию).  

---

### 5. **GET /posts/{id}/comments-with-users**  
Создайте endpoint, который возвращает пост и все его комментарии. Используйте новый **DTO** с вложенной структурой для комментариев.  

- **DTO**: `PostCommentsDto`  
  | Поле           | Тип                   | Описание                     |  
  |----------------|-----------------------|------------------------------|  
  | `Content`      | `string`             | Текст поста                 |  
  | `Username`     | `string`             | Имя автора поста            |  
  | `Comments`     | `List<CommentInfo>`  | Список комментариев         |  

  - Вложенный объект `CommentInfo`:  
    | Поле           | Тип        | Описание                     |  
    |----------------|------------|------------------------------|  
    | `Text`         | `string`   | Текст комментария           |  
    | `CommentAuthor`| `string`   | Имя автора комментария      |  

---

## Общие требования
- Создайте проект с нуля, разделив его на слои (`Domain`, `Infrastructure`, `WebAPI`).  
- Настройте миграции для создания базы данных.  
- Убедитесь, что все ограничения и связи работают корректно.  
- Проверьте работу API через Swagger.



### 20 дополнительных заданий для Instagram API

#### **UsersController** — Пользователи
1. **GET /users/new-registrations**  
   - Описание: Возвращает пользователей, зарегистрированных за последние 14 дней (добавьте поле `JoinDate`).  
   - DTO: `NewRegistrationDto`  
     - `Username` (string)  
     - `Email` (string)  
     - `JoinDate` (DateTime)  

2. **GET /users/active-posters**  
   - Описание: Показывает пользователей с хотя бы одним постом.  
   - DTO: `ActivePosterDto`  
     - `Username` (string)  
     - `PostCount` (int)  

3. **GET /users/recently-active**  
   - Описание: Возвращает пользователей с количеством постов за последние 7 дней.  
   - DTO: `RecentlyActiveUserDto`  
     - `Username` (string)  
     - `LastPostDate` (DateTime)  
     - `PostCount` (int)  

4. **GET /users/top-creators**  
   - Описание: Показывает 5 пользователей с наибольшим числом постов.  
   - DTO: `TopCreatorDto`  
     - `Username` (string)  
     - `PostCount` (int)  

5. **GET /users/high-interaction**  
   - Описание: Возвращает пользователей, чьи посты получили в среднем больше 5 комментариев.  
   - DTO: `HighInteractionUserDto`  
     - `Username` (string)  
     - `PostCount` (int)  
     - `AvgCommentsPerPost` (double)  

#### **PostsController** — Посты
6. **GET /posts/latest-posts**  
   - Описание: Возвращает 5 самых новых постов.  
   - DTO: `LatestPostDto`  
     - `Content` (string)  
     - `Username` (string)  
     - `CreatedAt` (DateTime)  

7. **GET /posts/user-recent**  
   - Описание: Показывает последние 5 постов пользователя по его ID.  
   - DTO: `UserRecentPostDto`  
     - `Content` (string)  
     - `CreatedAt` (DateTime)  
     - `Username` (string)  

8. **GET /posts/high-comment**  
   - Описание: Находит посты с более чем 10 комментариями.  
   - DTO: `HighCommentPostDto`  
     - `Content` (string)  
     - `Username` (string)  
     - `CommentCount` (int)  

#### **CommentsController** — Комментарии
9. **GET /comments/recent**  
    - Описание: Возвращает 5 последних комментариев.  
    - DTO: `RecentCommentDto`  
      - `Text` (string)  
      - `Username` (string)  
      - `CreatedAt` (DateTime)  

10. **GET /comments/by-post-id**  
    - Описание: Показывает последние 5 комментариев к посту по его ID.  
    - DTO: `PostRecentCommentsDto`  
      - `Text` (string)  
      - `Username` (string)  
      - `CreatedAt` (DateTime)  

11. **GET /comments/long-text**  
    - Описание: Находит комментарии длиннее 200 символов.  
    - DTO: `LongTextCommentDto`  
      - `Text` (string)  
      - `Username` (string)  
      - `TextLength` (int)  

12. **GET /comments/quick-responses**  
    - Описание: Показывает комментарии, оставленные в течение 15 минут после поста.  
    - DTO: `QuickResponseCommentDto`  
      - `Text` (string)    
      - `Username` (string)  
      - `PostId` (int)  
      - `TimeDifference` (TimeSpan)  

#### **Смешанные запросы** — Комбинированные
13. **GET /users/{id}/activity-summary**  
    - Описание: Показывает последние 3 поста и 3 комментария пользователя.  
    - DTO: `ActivitySummaryDto`  
      - `Username` (string)  
      - `Posts` (List: `Content`, `CreatedAt`)  
      - `Comments` (List: `Text`, `PostId`)  

14. **GET /posts/recent-popular**  
    - Описание: Возвращает 5 последних постов с более чем 5 комментариями.  
    - DTO: `RecentPopularPostDto`  
      - `Content` (string)   
      - `Username` (string)  
      - `CreatedAt` (DateTime)  
      - `CommentCount` (int)  

15. **GET /users/top-commenters**  
    - Описание: Показывает 5 пользователей с наибольшим числом комментариев.  
    - DTO: `TopCommenterDto`  
      - `Username` (string)   
      - `CommentCount` (int)  
