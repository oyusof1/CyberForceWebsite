# CyberForceWebsite
CyberForce Competition 2022 - Sole Zon Solis Website, a subsidiary of Vita Vehiculum

## Table of Contents

- [About](#about)
- [Features](#features)
- [Built With](#built-with)
- [Screenshots](#screenshots)

## About
I created this website for the 2022 National CyberForce competition environment. Written in C#, it allows green team users to view our solar farm power generation data, company information, and submit a contact form that can upload files to an FTP server and send emails to an SMTP server. This website also contains an admin interface that allows an admin, which is authenticated with Active Directory, to view all files on the FTP server and outgoing emails from the contact page.

Our team placed 17th nationally out of over 170 teams, with one of the highest green team scores throughout the competiton.

### Features

- Power generation data pulled in from MySQL 
- Contact form with FTP file upload and automatic email on submit
- LDAP authentication to on-premise AD server
- Authorized page for admins that can view all ftp files and emails 

### Built With

- C#
- Bootstrap
- ASP.NET MVC
- FluentFTP
- OpenPOP.Net

### Screenshots 

- Home
![home](https://user-images.githubusercontent.com/77765671/204096751-8e31b243-cb3b-47b5-ad15-7c70a73982b9.png)

- Contact Form 
![contact](https://user-images.githubusercontent.com/77765671/204096764-eae67f48-fdaf-4eb2-a9c0-e03dc6770ce3.png)

- Information Page
![info](https://user-images.githubusercontent.com/77765671/204096793-5ec5bc5e-2f1e-4604-abaa-720fe9c42fd8.png)

- Login 
![login](https://user-images.githubusercontent.com/77765671/204096801-46d44446-1de4-462e-8172-f4d8e102a8b0.png)

- Admin Page
![admin](https://user-images.githubusercontent.com/77765671/204096820-3ad337da-b591-4606-8cc9-23fafa00a96c.png)


