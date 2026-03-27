# DVLD (Driving & Vehicle License Department) 🚗💳

A comprehensive, enterprise-level Windows Forms desktop application designed to digitalize and manage the complete lifecycle of driving license operations. 

This project demonstrates advanced concepts in software engineering, database design, and secure application development using C# and the .NET Framework.
<img width="1366" height="768" alt="Screenshot (282)" src="https://github.com/user-attachments/assets/ba07d3a3-5c1e-4f9d-8f53-06c4ca1ba84c" />

---

## 🏗️ System Architecture
This application is strictly built on a **3-Tier Architecture** to ensure high cohesion, low coupling, and easy maintainability:

1.  **Data Access Layer (DAL):** The foundational layer responsible for all database interactions. It utilizes **ADO.NET** for executing queries, stored procedures, and managing SQL transactions securely.
2.  **Business Logic Layer (BLL):** The intermediary layer that acts as the brain of the application. It processes raw data from the DAL, enforces strict business rules, and passes the refined data to the UI.
3.  **Presentation Layer (UI):** The visual component where users interact with the system, featuring strict input validation and a modern interface.
<img width="1366" height="768" alt="Screenshot (283)" src="https://github.com/user-attachments/assets/37b6ea51-fae9-4370-a876-e6b17b77d015" />

---
<img width="1366" height="768" alt="Screenshot (281)" src="https://github.com/user-attachments/assets/0a9d0a10-ddc6-4352-90c9-050fe199905b" />

## 🛡️ Security & Integrity
* **Password Cryptography:** User passwords are never stored in plain text. The system employs advanced **Hash and Salt** algorithms to secure user credentials.
* **Data Validation:** Comprehensive validation ensures data integrity, preventing SQL injection and handling invalid inputs gracefully.
<img width="1366" height="768" alt="Screenshot (284)" src="https://github.com/user-attachments/assets/a1359393-e887-499f-88e0-9cd1cc4e17ce" />

---

## 🎨 UI/UX Design
The graphical user interface is powered by the **Guna2 UI Framework**, providing a sleek dark-mode experience:
* **Primary Background:** `#212121`
* **Secondary Background:** `#323232`
* **Accent/Highlight Color:** `#00ADB5` (Turquoise)

---
<img width="1366" height="768" alt="Screenshot (287)" src="https://github.com/user-attachments/assets/ad39aac2-f231-495b-8e22-42f506cfa41f" />

## 🧩 Core Modules & Dashboard
The main dashboard is logically divided into 5 primary operational sections:
1.  **Applications:** Processing new/renewed/replaced licenses and tests.
2.  **People:** Centralized directory for personal and national records.
3.  **Drivers:** Tracking individuals with issued licenses and their history.
4.  **Users:** Administrative control panel for employee accounts and permissions.
5.  **Settings:** Global configurations for test/application types and fees.
<img width="1366" height="768" alt="Screenshot (286)" src="https://github.com/user-attachments/assets/8716918b-6ec9-436f-a657-5ac6ccf57ed6" />

---

## 🗄️ Database Schema
The backend is supported by a highly normalized Microsoft SQL Server database. Key tables include:
* **Core Entities:** `People`, `Users`, `Drivers`, `Countries`.
* **Licensing System:** `LicenseClasses`, `Applications`, `LocalDrivingLicenseApplications`.
* **Examination System:** `Tests`, `TestAppointments`, `TestTypes`.

---

## 💻 Technology Stack
* **Language:** C#
* **Framework:** .NET Framework (Windows Forms)
* **Database:** Microsoft SQL Server
* **Data Access:** ADO.NET
* **UI Library:** Guna2 UI
* **IDE:** Visual Studio 2022

---

## 🚀 Getting Started

### Prerequisites
* Visual Studio 2022.
* Microsoft SQL Server Management Studio (SSMS).

### Installation & Setup
1.  **Clone the Repository:** `git clone https://github.com/YourUsername/DVLD-Project.git`
2.  **Database Setup:** Restore the `DVLD_Database.bak` file in SSMS.
3.  **Project Configuration:** Update the **Connection String** in the DAL layer to match your SQL instance.
4.  **Build and Run:** Build the solution to restore NuGet packages and press `F5`.

### 🔑 Default Credentials
To log in and explore the system features, use the following administrator credentials:
* **Username:** `z`
* **Password:** `Aa111111`
<img width="1366" height="768" alt="Screenshot (280)" src="https://github.com/user-attachments/assets/2245dd55-92ad-47dc-8219-6a2fbea98f73" />

---

## 🎓 Acknowledgements
This project was developed as a comprehensive application applying the advanced concepts learned through structured programming and software engineering courses.
