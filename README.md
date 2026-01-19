# MyFinalProject

[TR]

**Kapsamlı Final Projesi (Bootcamp/Akademi Bitirme Projesi)**

[![C#](https://img.shields.io/badge/Language-C%23-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Framework](https://img.shields.io/badge/Framework-ASP.NET%20Core%20%7C%20MVC-602C78.svg)]()
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%7C%20Layered-orange.svg)]()
[![Database](https://img.shields.io/badge/Database-SQL%20Server%20%7C%20)]()
[![GitHub repo size](https://img.shields.io/github/repo-size/abdullahhaktan/MyFinalProject)](https://github.com/abdullahhaktan/MyFinalProject)

---

## 💻 Proje Hakkında

Bu depo **kapsamlı final projesini** içerir. Proje, tüm temel ve ileri düzey bilgileri (çok katmanlı mimari, veritabanı yönetimi, kimlik doğrulama/yetkilendirme ve modern arayüz geliştirme) bir araya getirerek gerçek bir Portfolio uygulamanın temelini oluşturmayı amaçlamaktadır.

---

## ✨ Teknik Mimari ve Özellikler

### Mimari ve Desenler
* **Çok Katmanlı Mimari (N-Tier/Layered Architecture):**
    * **Presentation (Sunum):** Kullanıcı arayüzü (örneğin ASP.NET Core MVC/Razor Pages veya bir API için Frontend).
    * **Business (İş Mantığı):** İş kurallarını ve süreçlerini yöneten katman.
    * **Data Access (Veri Erişimi):** Veritabanı işlemlerini (CRUD) yürüten katman (genellikle **Entity Framework Core** kullanılarak).
* **Bağımlılık Enjeksiyonu (Dependency Injection):** Modüller arasındaki bağımlılıkları yönetmek için kullanılmıştır.

### Temel Özellikler
* **Kimlik Yönetimi (Authentication & Authorization):** Kullanıcı kaydı, girişi ve rol tabanlı erişim kontrolü.
* **CRUD Operasyonları:** Veritabanındaki ana varlıklar üzerinde tam oluşturma, okuma, güncelleme ve silme yeteneği.
* **Veritabanı Yönetimi:** Veri kalıcılığı için **SQL Server, PostgreSQL** veya benzeri bir RDBMS.

---

## 🚀 Nasıl Kurulur ve Çalıştırılır?

Bu projenin çalıştırılması için gerekli **.NET SDK** ve bir **veritabanı sunucusu** gereklidir.

1.  **Projeyi Klonlama:**
    ```bash
    git clone [https://github.com/abdullahhaktan/MyFinalProject](https://github.com/abdullahhaktan/MyFinalProject)
    cd MyFinalProject
    ```

2.  **Veritabanını Hazırlama:**
    * **Bağlantı Dizesini** (`appsettings.json` dosyasında) kendi yerel **SQL Sunucusu** ayarlarınıza (örneğin LocalDB veya SQL Express) göre güncelleyin.
    * Veritabanı şemasını oluşturmak ve **veritabanını otomatik olarak oluşturmak** için Entity Framework Core migrasyonlarını uygulayın:
      ```bash
      # Projenin Data Access katmanında veya kök dizinde çalıştırılır.
      dotnet ef database update 
      ```
    * ***Not: Bu komut, belirtilen sunucuda veritabanı yoksa otomatik olarak oluşturacaktır. El ile veritabanı oluşturmanıza gerek yoktur.***

3.  **Çözümü Başlatma:**
    * **Visual Studio** veya VS Code ile `.sln` (Solution) dosyasını açın.
    * Projeyi derleyin ve **F5** tuşu (Visual Studio) veya **`dotnet run`** komutu ile uygulamayı başlatın.

---
---

# MyFinalProject

[EN]

**Comprehensive Final Project (Bootcamp/Academy Graduation Project)**

[![C#](https://img.shields.io/badge/Language-C%23-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Framework](https://img.shields.io/badge/Framework-ASP.NET%20Core%20%7C%20MVC-602C78.svg)]()
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%7C%20Layered-orange.svg)]()
[![Database](https://img.shields.io/badge/Database-SQL%20Server%20%7C)]()
[![GitHub repo size](https://img.shields.io/github/repo-size/abdullahhaktan/MyFinalProject)](https://github.com/abdullahhaktan/MyFinalProject)

---

## 💻 About the Project

This repository contains a **comprehensive final project**. The project aims to combine all fundamental and advanced knowledge (multi-layered architecture, database management, authentication/authorization, and modern UI development) to build the foundation of a real Portfolio application.

---

## ✨ Technical Architecture and Features

### Architecture & Patterns
* **Multi-Layered Architecture (N-Tier/Layered Architecture):**
    * **Presentation Layer:** User interface (e.g., ASP.NET Core MVC/Razor Pages or Frontend for an API).
    * **Business Layer:** Handles business rules and processes.
    * **Data Access Layer:** Performs database operations (CRUD), typically using **Entity Framework Core**.
* **Dependency Injection:** Used to manage dependencies between modules.

### Core Features
* **Authentication & Authorization:** User registration, login, and role-based access control.
* **CRUD Operations:** Full create, read, update, and delete capabilities on main database entities.
* **Database Management:** Data persistence using **SQL Server, PostgreSQL**, or another RDBMS.

---

## 🚀 How to Install and Run

This project requires **.NET SDK** and a **database server** to run.

1.  **Clone the Project:**
    ```bash
    git clone [https://github.com/abdullahhaktan/MyFinalProject](https://github.com/abdullahhaktan/MyFinalProject)
    cd MyFinalProject
    ```

2.  **Prepare the Database:**
    * Update the **Connection String** in the `appsettings.json` file to match your local **SQL Server** settings (e.g., LocalDB or SQL Express).
    * Apply Entity Framework Core migrations to create the database schema and **automatically create the database**:
      ```bash
      # Run in the Data Access layer or root directory of the project
      dotnet ef database update
      ```
    * ***Note: This command will automatically create the database if it doesn't exist on the server. Manual creation is not required.***

3.  **Run the Solution:**
    * Open the `.sln` (Solution) file in **Visual Studio** or VS Code.
    * Build the project and start the application with **F5** (Visual Studio) or **`dotnet run`** command.

---


🌐 Live Project  

🔗 [abdullahhaktan.com.tr](http://abdullah.haktan.com.tr)  

🌐 Live Portfolio

🔗 [abdullahhaktan.com.tr/Default/AbdullahhaktanCV](http://abdullah.haktan.com.tr/Default/AbdullahhaktanCV)  
