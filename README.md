# MailService.MinWebAPI

Send emails via SMTP with MailKit library, create logs in SQL Server database.

## Configuration 
Fill in the SQL Server connection string and SMTP server configuration in appsettings.json. Database initial migration is in the corresponding folder.

## Methods

**POST** /api/mails  
Request body model:
```
{
  "subject": "string",
  "body": "string",
  "recipients": ["string"]
}
```
Email message is formed and sent. A record in the database is made containing the fields above, email creation date and result with send error message if there is one.

**GET** /api/mails  
Get JSON object with all emails from the database.

**DELETE** /api/mails/{id}  
Delete email with certain id from the database.
