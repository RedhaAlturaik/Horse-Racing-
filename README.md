# Horse-Racing-
This project is a full-stack Horse Racing Database System built using MySQL, C#, and ASP.NET. It manages horse racing data including horses, owners, trainers, races, tracks, and results. The system provides two user roles (Admin and Guest) with different functionalities, allowing users to query, update, and manage racing data efficiently through a web-based interface connected to a relational database.

## Team
1. Mohammed Alfaraj – 202323090 – Section 1
2. Redha Alturaik – 202323010 – Section 3
- Project Team: 46

## Technologies Used
- C# (.NET / ASP.NET)
- HTML / CSS
- MySQL 8.0
- MySQL.Data (NuGet Package)
- Visual Studio 2022
- MySQL Workbench


## System Overview

The system is built around a relational database that includes:
- Horses and stables
- Owners and ownership relations
- Trainers assigned to stables
- Races and tracks
- Race results and prize distribution

## Users interact with the system through a simple UI connected to MySQL.

User Roles:
### Guest Features
- View horses by owner last name
- View trainers of winning horses
- View total prize earnings per trainer
- View tracks with race and horse statistics

### Admin Features
- Add new races and results
- Delete owners and related data
- Move horses between stables
- Approve/add trainers to stables
- Manage stable and horse records

## Database Design
Main tables include:
- Horse
- Owner
- Owns
- Stable
- Trainer
- Track
- Race
- RaceResults

Relationships include:

- Many-to-many (Owners ↔ Horses)
- One-to-many (Stable → Horses/Trainers)
- One-to-many (Race → Results)

## Advanced SQL Features
- Stored Procedure: Delete owner and all related data
- Trigger: Copy horse data into old_info before deletion
- Joins across multiple tables for analytics queries

## Key Challenges
- MySQL connection issues in Visual Studio (resolved using MySQL.Data)
- GridView display issues (fixed by correcting bindings and column mapping)
- Handling complex multi-table joins for guest queries

## Key Functional Queries
- Trainer earnings aggregation using SUM(prize)
- Multi-table joins for horse-owner-trainer relationships
- Race result filtering using conditions (e.g., first place)
- Track statistics using grouping and counting

## Project Highlights
- Fully relational database design
- Role-based system (Admin vs Guest)
- Real-world racing data simulation
- Use of stored procedures and triggers
- Dynamic web-based query system

## Project Structure
- Frontend: ASP.NET Web Forms (C#)
- Backend: C# logic + MySQL queries
- Database: MySQL 8.0

## Summary
This project demonstrates strong understanding of relational database design, SQL querying, and backend integration with a web interface. It focuses on real-world data modeling, efficient querying, and procedural SQL features.
