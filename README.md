# Сайт онлайн-бронирования авиабилетов и отелей (ASP.Net Core MVC Api + Razor pages)

Web-приложение для онлайн бронирования авиабилетов и отелей. 

## Быстрый старт 🚀

### Предварительные требования ⚙️
- ОС: Windows 10/11
- Backend: .NET 6.0+ SDK
- IDE (рекомендуется): Visual Studio 2022, Visual Studio Code
- База данных: PostgreSQL, MS SQL Server

### Установка и запуск 🔧
1. Клонируйте репозиторий
    ```bash
    git clone https://github.com/BanCoder/FlightTicketsWeb.git
    cd FlightTicketsWeb
    ```
2. Настройка базы данных
   - Создайте базу данных в PostgreSQL или MS SQL Server
        ```sql
        CREATE DATABASE flight_tickets;
        ```
    - в запросе вставьте следующий скрипт:
        ```sql 
        use flight_tickets
        -- Создание таблиц
        CREATE TABLE SystemUser (
            id INT PRIMARY KEY IDENTITY(1,1),
            email NVARCHAR(100) UNIQUE NOT NULL,
            password_hash NVARCHAR(255) NOT NULL,
            first_name NVARCHAR(50) NOT NULL,      
            last_name NVARCHAR(50) NOT NULL,      
            role NVARCHAR(20) DEFAULT 'user' CHECK (role IN ('guest', 'user', 'admin')),
            created_at DATETIME DEFAULT GETDATE()
        ); 

        CREATE TABLE City (
            city_name NVARCHAR(50) PRIMARY KEY,
            country NVARCHAR(50) NOT NULL,
            region NVARCHAR(100) NULL,              
            climate NVARCHAR(30) NULL,                    
            airport_code NVARCHAR(10) NULL 
        );
        CREATE TABLE Airline (
            comp_name NVARCHAR(50) PRIMARY KEY,
            airline_address NVARCHAR(100),
            contact_number NVARCHAR(15),
            logo_url NVARCHAR(200) NULL
        );

        CREATE TABLE Flight (
            flight_id INT PRIMARY KEY IDENTITY(1,1),
            flight_number NVARCHAR(20) NOT NULL,          
            airline_name NVARCHAR(50) NOT NULL,
            departure_city NVARCHAR(50) NOT NULL,        
            arrival_city NVARCHAR(50) NOT NULL,          
            departure_date DATETIME NOT NULL,            
            arrival_date DATETIME NOT NULL,                       
            class NVARCHAR(20) CHECK (class IN ('Эконом', 'Комфорт', 'Бизнес')),
            price DECIMAL(10,2) CHECK (price > 0),
            seats_available INT DEFAULT 100,
            FOREIGN KEY (departure_city) REFERENCES City(city_name),
            FOREIGN KEY (arrival_city) REFERENCES City(city_name),
            FOREIGN KEY (airline_name) REFERENCES Airline(comp_name)
        );

        CREATE TABLE Hotel (
            hotel_id INT PRIMARY KEY IDENTITY(1,1),
            hotel_name NVARCHAR(100) NOT NULL,
            hotel_address NVARCHAR(200) NOT NULL,
            city_name NVARCHAR(50) NOT NULL,
            cost_per_night DECIMAL(10,2) CHECK (cost_per_night > 0),
            stars INT CHECK (stars BETWEEN 1 AND 5),
            category NVARCHAR(30) NULL,                   
            rooms_available INT DEFAULT 50,
            image_url NVARCHAR(500) NOT NULL,        
            FOREIGN KEY (city_name) REFERENCES City(city_name)
        );

        CREATE TABLE Passenger (
            passport_num NVARCHAR(20) PRIMARY KEY,
            surname NVARCHAR(50) NOT NULL,
            first_name NVARCHAR(50) NOT NULL,
            last_name NVARCHAR(50) NOT NULL,
            birth_date DATE NOT NULL,
            sex NVARCHAR(10) CHECK (sex IN ('Мужской', 'Женский')),
            phone NVARCHAR(15),
            email NVARCHAR(100),
            user_id INT NULL,                         
            created_at DATETIME DEFAULT GETDATE(),
            FOREIGN KEY (user_id) REFERENCES SystemUser(id) ON DELETE CASCADE
        );

        CREATE TABLE Booking (
            booking_id INT PRIMARY KEY IDENTITY(1,1),
            booking_date DATETIME DEFAULT GETDATE(),
            booking_code NVARCHAR(10) UNIQUE,            
            flight_id INT NULL,      
            passenger_passport NVARCHAR(20) NOT NULL,
            hotel_id INT NULL,           
            FOREIGN KEY (flight_id) REFERENCES Flight(flight_id) ON DELETE CASCADE,
            FOREIGN KEY (passenger_passport) REFERENCES Passenger(passport_num) ON DELETE CASCADE,
            FOREIGN KEY (hotel_id) REFERENCES Hotel(hotel_id) ON DELETE CASCADE
        );
        -- Вставка данных в таблицы
        use flight_tickets
        INSERT INTO SystemUser (email, password_hash, first_name, last_name, role) 
        VALUES ('admin@airtravel.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'Admin', 'Admin', 'admin');

        INSERT INTO City (city_name, country, region, climate, airport_code) VALUES
        (N'Москва', N'Россия', N'Центральная Россия', N'Умеренный', N'SVO'),
        (N'Санкт-Петербург', N'Россия', N'Северо-Запад', N'Умеренный', N'LED'),
        (N'Стамбул', N'Турция', N'Мраморноморский', N'Средиземноморский', N'IST'),
        (N'Мале', N'Мальдивы', N'Южная Азия', N'Тропический', N'MLE'),
        (N'Сочи', N'Россия', N'Краснодарский край', N'Субтропический', N'AER'),
        (N'Калининград', N'Россия', N'Калининградская область', N'Морской', N'KGD'),
        (N'Екатеринбург', N'Россия', N'Урал', N'Умеренный континентальный', N'SVX'),
        (N'Новосибирск', N'Россия', N'Сибирь', N'Континентальный', N'OVB'),
        (N'Париж', N'Франция', N'Иль-де-Франс', N'Умеренный морской', N'CDG'),
        (N'Лондон', N'Великобритания', N'Англия', N'Умеренный морской', N'LHR'),
        (N'Рим', N'Италия', N'Лацио', N'Средиземноморский', N'FCO'),
        (N'Дубай', N'ОАЭ', N'Эмират Дубай', N'Пустынный', N'DXB'),
        (N'Бангкок', N'Таиланд', N'Центральный Таиланд', N'Тропический', N'BKK'),
        (N'Токио', N'Япония', N'Канто', N'Влажный субтропический', N'HND'),
        (N'Нью-Йорк', N'США', N'Нью-Йорк', N'Влажный континентальный', N'JFK'),
        (N'Майами', N'США', N'Флорида', N'Тропический', N'MIA'),
        (N'Лос-Анджелес', N'США', N'Калифорния', N'Средиземноморский', N'LAX');

        INSERT INTO Airline (comp_name, airline_address, contact_number) VALUES
        (N'Аэрофлот', N'Москва, Шереметьево', N'+74951234567'),
        (N'S7 Airlines', N'Новосибирск, Толмачево', N'+73832223344'),
        (N'Победа', N'Москва, Внуково', N'+74957654321'),
        (N'Red Wings', N'Москва, Домодедово', N'+74959876543'),
        (N'Уральские авиалинии', N'Екатеринбург, Кольцово', N'+73433445566');

        INSERT INTO Flight (flight_number, airline_name, departure_city, arrival_city, departure_date, arrival_date, class, price, seats_available) VALUES
        (N'SU-1234', N'Аэрофлот', N'Москва', N'Санкт-Петербург', '2026-01-15T08:00:00', '2026-01-15T09:30:00', N'Эконом', 4500.00, 50),
        (N'SU-1235', N'Аэрофлот', N'Москва', N'Санкт-Петербург', '2026-01-15T12:00:00', '2026-01-15T13:30:00', N'Комфорт', 6500.00, 30),
        (N'SU-1236', N'Аэрофлот', N'Москва', N'Санкт-Петербург', '2026-01-15T18:00:00', '2026-01-15T19:30:00', N'Бизнес', 9500.00, 20),
        (N'S7-5678', N'S7 Airlines', N'Санкт-Петербург', N'Стамбул', '2026-01-20T14:00:00', '2026-01-20T18:35:00', N'Эконом', 19495.00, 40),
        (N'S7-5679', N'S7 Airlines', N'Санкт-Петербург', N'Стамбул', '2026-01-20T20:00:00', '2026-01-20T22:35:00', N'Бизнес', 29495.00, 15),
        (N'DP-9012', N'Победа', N'Москва', N'Мале', '2026-01-25T10:00:00', '2026-01-25T18:00:00', N'Эконом', 25888.00, 25),
        (N'SU-2345', N'Аэрофлот', N'Москва', N'Сочи', '2026-01-17T07:00:00', '2026-01-17T09:30:00', N'Эконом', 6500.00, 60),
        (N'W6-7890', N'Red Wings', N'Москва', N'Калининград', '2026-01-19T11:00:00', '2026-01-19T12:30:00', N'Эконом', 5200.00, 45),
        (N'U6-3456', N'Уральские авиалинии', N'Екатеринбург', N'Москва', '2026-01-08T09:00:00', '2026-01-08T11:30:00', N'Комфорт', 7800.00, 35),
        (N'SU-4321', N'Аэрофлот', N'Санкт-Петербург', N'Москва', '2026-01-16T10:00:00', '2026-01-16T11:30:00', N'Эконом', 4500.00, 50),
        (N'S7-8765', N'S7 Airlines', N'Стамбул', N'Санкт-Петербург', '2026-01-22T12:00:00', '2026-01-22T16:35:00', N'Эконом', 18995.00, 40),
        (N'AF-1234', N'Аэрофлот', N'Москва', N'Париж', '2026-01-18T10:00:00', '2026-01-18T12:30:00', N'Эконом', 28500.00, 40),
        (N'AF-1235', N'Аэрофлот', N'Москва', N'Париж', '2026-01-18T15:00:00', '2026-01-18T17:30:00', N'Бизнес', 48500.00, 15),
        (N'BA-5678', N'S7 Airlines', N'Москва', N'Лондон', '2026-01-19T11:00:00', '2026-01-19T14:30:00', N'Эконом', 29500.00, 35),
        (N'BA-5679', N'S7 Airlines', N'Москва', N'Лондон', '2026-01-19T17:00:00', '2026-01-19T20:30:00', N'Бизнес', 49500.00, 12),
        (N'EK-7890', N'Аэрофлот', N'Москва', N'Дубай', '2026-01-20T08:00:00', '2026-01-20T13:30:00', N'Эконом', 32500.00, 45),
        (N'EK-7891', N'Аэрофлот', N'Москва', N'Дубай', '2026-01-20T22:00:00', '2026-01-21T03:30:00', N'Бизнес', 52500.00, 18),
        (N'AZ-3456', N'Победа', N'Санкт-Петербург', N'Рим', '2026-01-21T09:00:00', '2026-01-21T12:30:00', N'Эконом', 24500.00, 30),
        (N'SU-9999', N'Аэрофлот', N'Москва', N'Нью-Йорк', '2026-01-22T13:00:00', '2026-01-22T22:30:00', N'Эконом', 58500.00, 25),
        (N'SU-9998', N'Аэрофлот', N'Москва', N'Нью-Йорк', '2026-01-22T16:00:00', '2026-01-23T01:30:00', N'Бизнес', 98500.00, 10),
        (N'TG-8888', N'S7 Airlines', N'Москва', N'Бангкок', '2026-01-23T10:00:00', '2026-01-23T20:00:00', N'Эконом', 38500.00, 40),
        (N'JL-7777', N'Аэрофлот', N'Москва', N'Токио', '2026-01-24T11:00:00', '2026-01-24T21:00:00', N'Эконом', 45500.00, 35);

        INSERT INTO Hotel (hotel_name, hotel_address, city_name, cost_per_night, stars, category, rooms_available, image_url) VALUES
        (N'Rixos Premium', N'Antalya, Konyaaltı Cd. No:22', N'Стамбул', 9500.00, 5, N'Отель', 15, N'/img/hotels/rexios.jpg'),
        (N'Hilton Istanbul', N'Cumhuriyet Cad No:50', N'Стамбул', 12000.00, 5, N'Отель', 20, N'/img/hotels/hilton.jpg'),
        (N'Divan Istanbul', N'Asker Ocağı Cad. No:1', N'Стамбул', 6500.00, 4, N'Отель', 25, N'/img/hotels/divan.jpg'),
        (N'Taksim Suites', N'İstiklal Cad. No:123', N'Стамбул', 4500.00, NULL, N'Апартаменты', 30, N'/img/hotels/taksim.jpg'),
        (N'Коринтия', N'Невский пр-т, 57', N'Санкт-Петербург', 8500.00, 5, N'Отель', 30, N'/img/hotels/korintia.jpg'),
        (N'Азимут', N'Лермонтовский пр-т, 43', N'Санкт-Петербург', 6500.00, 4, N'Отель', 40, N'/img/hotels/azimut.jpg'),
        (N'Станция L1', N'Лиговский пр-т, 1', N'Санкт-Петербург', 3500.00, 3, N'Отель', 50, N'/img/hotels/stancia.jpg'),
        (N'SPB Apartments', N'ул. Рубинштейна, 15', N'Санкт-Петербург', 5500.00, NULL, N'Апартаменты', 20, N'/img/hotels/spb_ap.jpg'),
        (N'Метрополь', N'Театральный проезд, 2', N'Москва', 15000.00, 5, N'Отель', 25, N'/img/hotels/metropol.jpg'),
        (N'Арбат', N'Плотников переулок, 12', N'Москва', 9500.00, 4, N'Отель', 35, N'/img/hotels/arbat.jpg'),
        (N'Ибис', N'Шереметьевская ул, 6', N'Москва', 4500.00, 3, N'Отель', 60, N'/img/hotels/ibis.jpg'),
        (N'Москва Сити Апарт', N'Пресненская наб., 8', N'Москва', 12000.00, NULL, N'Апартаменты', 15, N'/img/hotels/mos_city_ap.jpg'),
        (N'Conrad Maldives', N'Rangali Island', N'Мале', 45000.00, 5, N'Отель', 10, N'/img/hotels/conrad.jpg'),
        (N'One&Only Reethi Rah', N'North Malé Atoll', N'Мале', 60000.00, 5, N'Отель', 8, N'/img/hotels/one_only.jpg'),
        (N'Villa Park', N'Boduthakurufaanu Magu', N'Мале', 18000.00, 4, N'Отель', 20, N'/img/hotels/villa_park.jpg'),
        (N'Рэдиссон Сочи', N'ул. Орджоникидзе, 11', N'Сочи', 7800.00, 5, N'Отель', 25, N'/img/hotels/reddison.jpg'),
        (N'Жемчужина', N'ул. Черноморская, 3', N'Сочи', 6500.00, 4, N'Отель', 40, N'/img/hotels/zhemchuzhina.jpg'),
        (N'Сочи Апарт', N'ул. Навагинская, 9', N'Сочи', 5000.00, NULL, N'Апартаменты', 25, N'/img/hotels/sochi_ap.jpg'),
        (N'Балтика', N'Ленинский пр-т, 10', N'Калининград', 5200.00, 4, N'Отель', 30, N'/img/hotels/baltica.jpg'),
        (N'Калининград Инн', N'ул. Театральная, 4', N'Калининград', 3800.00, 3, N'Отель', 35, N'/img/hotels/kalin_inn.jpg'),
        (N'Hotel de Crillon', N'10 Place de la Concorde', N'Париж', 45000.00, 5, N'Отель', 20, N'/img/hotels/crillon.jpg'),
        (N'Le Bristol', N'112 Rue du Faubourg Saint-Honoré', N'Париж', 38000.00, 5, N'Отель', 25, N'/img/hotels/le_bristol.jpg'),
        (N'Paris Marriott', N'70 Avenue des Champs-Élysées', N'Париж', 25000.00, 4, N'Отель', 40, N'/img/hotels/paris_mar.jpg'),
        (N'Paris Apartments', N'Rue de Rivoli, 15', N'Париж', 18000.00, NULL, N'Апартаменты', 30, N'/img/hotels/paris_apart.jpg'),
        (N'The Ritz London', N'150 Piccadilly', N'Лондон', 55000.00, 5, N'Отель', 18, N'/img/hotels/ritz_london.jpg'),
        (N'Shangri-La Hotel', N'31 St Thomas Street', N'Лондон', 42000.00, 5, N'Отель', 22, N'/img/hotels/shangri_la.jpg'),
        (N'Premier Inn', N'1 Kings Cross Road', N'Лондон', 12000.00, 3, N'Отель', 50, N'/img/hotels/premier_inn.jpg'),
        (N'Burj Al Arab', N'Jumeirah Street', N'Дубай', 85000.00, 5, N'Отель', 15, N'/img/hotels/burj_al.jpg'),
        (N'Atlantis The Palm', N'Crescent Road', N'Дубай', 45000.00, 5, N'Отель', 30, N'/img/hotels/atlantis.jpg'),
        (N'Rove Downtown', N'312 Al Mustaqbal Street', N'Дубай', 15000.00, 3, N'Отель', 45, N'/img/hotels/rove.jpg'),
        (N'The Plaza', N'768 Fifth Avenue', N'Нью-Йорк', 65000.00, 5, N'Отель', 25, N'/img/hotels/plaza.jpg'),
        (N'Marriott Marquis', N'1535 Broadway', N'Нью-Йорк', 32000.00, 4, N'Отель', 60, N'/img/hotels/marriot_marquis.jpg'),
        (N'NYC Loft', N'Chelsea, Manhattan', N'Нью-Йорк', 28000.00, NULL, N'Апартаменты', 20, N'/img/hotels/nyc_loft.jpg');
        ```
        Также советую вставить скрипт запроса на создание триггеров, которые выполняют логгирование операции "вставка", которые администратор, выполняет в своем окне: 
        ```sql
        use flight_tickets
        CREATE TABLE UserAuditLog (
            audit_id INT PRIMARY KEY IDENTITY(1,1),
            user_id INT NOT NULL,
            changed_field NVARCHAR(50) NOT NULL, 
            old_value NVARCHAR(MAX) NULL,         
            new_value NVARCHAR(MAX) NULL,        
            changed_by NVARCHAR(100) DEFAULT SYSTEM_USER,
            changed_at DATETIME DEFAULT GETDATE()
        );

        CREATE TABLE FlightAuditLog (
            audit_id INT PRIMARY KEY IDENTITY(1,1),
            flight_id INT NOT NULL,
            changed_field NVARCHAR(50) NOT NULL,
            old_value NVARCHAR(MAX) NULL,
            new_value NVARCHAR(MAX) NULL,
            changed_by NVARCHAR(100) DEFAULT SYSTEM_USER,
            changed_at DATETIME DEFAULT GETDATE()
        );

        -- Триггер 1
        CREATE TRIGGER trg_AuditUserUpdates ON SystemUser
        AFTER UPDATE AS -- Срабатывает после операции UPDATE
        BEGIN
        INSERT INTO UserAuditLog (user_id, changed_field, old_value, new_value) --Вставляем записи с таблицу аудита для отслеживания изменений
        -- 1. Отслеживание изменения email
        SELECT i.id, 'email', d.email, i.email -- ID пользователя,  название измененного поля, старое значение, новое значение, inserted содержит новые значения, --deleted содержит старые значения
        FROM inserted i
        JOIN deleted d ON i.id = d.id
        WHERE i.email != d.email  -- Записываем только если email действительно изменился
        UNION ALL -- Объединяем результаты следующих запросов

        -- 2. Отслеживание изменения роли пользователя 
        SELECT i.id, 'role', d.role, i.role 
        FROM inserted i
        JOIN deleted d ON i.id = d.id
        WHERE i.role != d.role -- Только если роль изменилась
        UNION ALL 

        -- 3. Отслеживание изменения имени
        SELECT i.id, 'first_name', d.first_name, i.first_name
        FROM inserted i
        JOIN deleted d ON i.id = d.id
        WHERE i.first_name != d.first_name  
        UNION ALL

        -- 4. Отслеживание изменения фамилии
        SELECT i.id, 'last_name', d.last_name, i.last_name
        FROM inserted i
        JOIN deleted d ON i.id = d.id
        WHERE i.last_name != d.last_name;
        END;
        GO

        --Триггер 2
        CREATE TRIGGER trg_AuditFlightUpdates ON Flight
        AFTER UPDATE AS
        BEGIN
        SET NOCOUNT ON; 
        -- Фиксируем изменения в рейсах для анализа и отчетности
        INSERT INTO FlightAuditLog (flight_id, changed_field, old_value, new_value)
        -- 1. Контроль изменения цен на билеты
        SELECT i.flight_id, 'price', CAST(d.price AS NVARCHAR(50)), CAST(i.price AS NVARCHAR(50)) -- Конвертируем decimal в строку для логирования
        FROM inserted i
        JOIN deleted d ON i.flight_id = d.flight_id
        WHERE i.price != d.price -- Логируем только фактические изменения цены
        UNION ALL

        -- 2. Отслеживание доступности мест (важно для управления загрузкой)
        SELECT i.flight_id, 'seats_available', CAST(d.seats_available AS NVARCHAR(50)), CAST(i.seats_available AS NVARCHAR(50))
        FROM inserted i
        JOIN deleted d ON i.flight_id = d.flight_id
        WHERE i.seats_available != d.seats_available
        UNION ALL

        -- 3. Мониторинг изменения даты вылета
        SELECT i.flight_id, 'departure_date', CONVERT(NVARCHAR(50), d.departure_date, 120), CONVERT(NVARCHAR(50), i.departure_date, 120) -- Конвертация даты в строку
        FROM inserted i
        JOIN deleted d ON i.flight_id = d.flight_id
        WHERE i.departure_date != d.departure_date 
        UNION ALL

        -- 4. Мониторинг изменения даты прилета
        SELECT i.flight_id, 'arrival_date', CONVERT(NVARCHAR(50), d.arrival_date, 120), CONVERT(NVARCHAR(50), i.arrival_date, 120)
        FROM inserted i
        JOIN deleted d ON i.flight_id = d.flight_id
        WHERE i.arrival_date != d.arrival_date;
        END;
        GO
        ```
   - Обновите строку подключения в `FlightTicketsWeb/appsettings.json`
        ```json
        "ConnectionSettings": {
        "sqlConnection": "Server=DESKTOP-5B36UD4;Database=flight_tickets;Trusted_Connection=True;Encrypt=false"
        }
        ```
3. Создание контекста базы данных
   - Перейдите в папку с решением 
        ```bash
        cd FlightTicketsWeb
        ```
    - Создайте контекст следующей командой:
        ```bash
        dotnet ef dbcontext scaffold "Server=DESKTOP-5B36UD4;Database=flight_tickets;Trusted_Connection=True;Encrypt=false" Microsoft.EntityFrameworkCore.SqlServer -o Models --force
        ```
4. Запуск приложения:
        Backend и Frontend (в терминале) или просто нажмите вверху на зеленый треугольник
        ```
        cd FlightTicketsWeb\FlightTicketsWeb
        dotnet run
        ```

### Использование 📝
Данное веб-приложение обладает следующим функционалом: 

1. Поиск авиабилетов и отелей, через ввод информации в поля на главной странице
2. Наличие стрницы "Полпулярные направления" с информацией о городах, а также отображением погоды в выбранном вами месте в реальном времени
3. Возможность входа/регистрации пользоваетлся на сайте, что позволяет ему осуществлять бронирование авиабилетов и отелей
4. Возможность входа администратора под своим аккаунтом и выполнении запросов в специальной админ-панели
5. Наличие статичных страниц с информацией для пользователей (вкалдки "О нас", "Акции")
6. Возможность бронирование рейсов и отелей при выборе выбранного вами направления. Если вы не регистрировались или не вошли под своим аккаунтом, вас перебросить на страницу с входом. 
7. После осуществления бронирования, вам отправится на почту сообщение с подтсверждением брони.  

### Особенности ✅
- **Чистый MVC**: полное взаимодействие 3 элементов: model, view, controller
- **Ипользование DI**: позволяет снижать зависимости между классами
- **Валидация**: проверка введенных данных пользователем в поля
- **Использование Razor Pages**: позволяет не создавать отдельное React приложение, а использовать знакомый html код
- **Расширенный функционал для администратора**: Администратор приложения может выполнять все операции над БД, а также отслеживать все действия пользователей и выявлять ошибки при их возникновении
  
### Архитектура 🛠️
#### Технологический стек 💻 
`Backend` и `Frontend`: 
- **Фреймворк**: .NET 8.0 Web API
- **ORM**: Entity Framework Core 8.0
- **База данных**: MS SQL Server с соотвествующим пакетом
- **Логироание действий пользователя в файл и использованием NLog**

