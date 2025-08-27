# Rest API - Stock Control - Miroc

## 🧱 Architecture and Technologies

This REST API is built with **ASP.NET CORE 9**, following the principles of the **Hexagonal Architecture** which allows for better separation of responsibilities, scalability and ease of maintenance.

### 🛠️ Main technologies
- **ASP.NET CORE 9** – Core framework for building the API.
- **SQL Server** - Database

### Roles
- **ADMIN**
- **PRESUPUESTISTA**

## 🌐 Production URL
**The API is deployed and available at:**
**https://api-mds.onrender.com**

## Steps to Setup

## ⚙️ Configuration

**Requirements:**
- **NET 9+**
- **SQL Server**

**1. Clone the Project**
```bash**
$ https://github.com/GomezDiegoElias/stock-control-api.git
```

**2. Create SQL Server database**
```sql
create database bd_stock_miroc
```

**2.a Create stored procedure**
```sql
-- Stored Procedures for users
DELIMITER //

CREATE PROCEDURE `getUserPagination`(
    IN PageIndex INT,
    IN PageSize INT
)
BEGIN
    DECLARE Offset INT;
    SET Offset = (PageSize * (PageIndex - 1));

    SELECT
        u.id,
        u.dni,
        u.email,
        u.first_name,
        u.role_name,
        u.status_name,
        @row_number:=@row_number + 1 AS Fila,
        (SELECT COUNT(*) FROM tbl_users) AS TotalFilas
    FROM 
        tbl_users u,
        (SELECT @row_number:=0) AS r
    ORDER BY u.id ASC
    LIMIT Offset, PageSize;
END //

DELIMITER ;
```

**2.b Test the created stored procedure**
```sql
-- Testing stored procedures users

-- Procedure call (page 1, 10 records per page)
CALL getUserPagination(1, 10);
```

**3. Creating the user secret for credentials**

```json
{
  "CONNECTION_SQLSERVER": "Persist Security Info=True;Initial Catalog=bd_stock_miroc;Data Source=.; Integrated Security=True;TrustServerCertificate=True;",
  "Jwt": {
    "Secret": "***",
    "Issuer": "http://localhost:5027",
    "Audience": "http://localhost:5027",
    "ExpirationMinutes": 60
  }
}
```

**4. Run the application in this location `src\\org.pos.software>`**
```bash
$ dotnet run
```

**The app will start running at <https://localhost:5027>**

**Swagger Documentation <https://localhost:5027/swagger>**

## Explore Rest APIs

**API Response Standard**

```text
{
  "success": boolean,
  "message": string,
  "data": object | array | null,
  "error": object | null,
  "status": number
}
```

**Examples with users**

```json
{
  "success": true,
  "message": "Users successfully obtained",
  "data": {
    "items": [
      {
        "id": "usr_20250717194721_fb87b56d",
        "dni": 88888888,
        "firstname": "Diego",
        "email": "example1234@gmail.com",
        "role": "ADMIN"
      },
      {
        "id": "usr_20250717195912_5185abd1",
        "dni": 99999999,
        "firstname": "Valentina",
        "email": "example3624@gmail.com",
        "role": "PRESUPUESTISTA"
      }
    ],
    "pagination": {
      "totalItems": 8,
      "currentPage": 1,
      "perPage": 10,
      "totalPages": 1
    }
  },
  "error": null,
  "status": 200
}

```

**Error Response**
```json
{
  "success": false,
  "message": "Something went wrong",
  "data": null,
  "error": {
    "message": "NotFoundException",
    "details": "Not found Exception . User does not exist with DNI: 12345678",
    "path": "/api/v1/users/12345678"
  },
  "status": 400
}
```

The app defines following CRUD APIs.

### Authentication

| Method | Url                   | Decription | Sample Valid Request Body | 
| ------ |-----------------------| ---------- | --------------------------- |
| POST   | /api/v1/auth/register | Sign up | [JSON](#signup) |
| POST   | /api/v1/auth/login    | Log in | [JSON](#signin) |

### Users

| Method | Url                               | Description    | Sample Valid Request Body |
|--------|-----------------------------------|----------------|---------------------------|
| GET    | /api/v1/users                     | Returns all users. Default pagination values: pageIndex=1 and pageSize=5               |                           |
| GET    | /api/v1/users?pageIndex=X&pageSize=X | Returns all users indicated by pagination           |
| GET    | /api/v1/users/{dni}                  | Returns the user found by their ID. |                           |
| POST   | /api/v1/users                        | Create a new user | [JSON](#usercreate)       |
| DELETE | /api/v1/users/{dni}                  | Delete an existing user |                           |

### Materials

| Method | Url                                      | Decription                                                                   | Sample Valid Request Body | 
|--------|------------------------------------------|------------------------------------------------------------------------------|---------------------------|
| GET    | /api/v1/materials                        | Returns all materials. Defualt pagination values: pageIndex=1 and pageSize=5 |                           |
| GET    | /api/v1/materials?pageIndex=X&pageSize=X | Returns all materials indicated by paginated                                 |                           |

**Test them using postman or any other rest client.**

## Sample Valid JSON Request Bodys

##### <a id="signup">Sign Up -> /api/v1/auth/register</a>

```json
{
  "dni": 12345678,
  "firstName": "Diego",
  "email": "example123@gmail.com",
  "password": "password123"
}
```

##### <a id="signin">Log In -> /api/v1/auth/login</a>
```json
{
  "email": "example123@gmail.com",
  "password": "password123"
}
```

##### <a id="usercreate">Create User -> /api/v1/users</a>
```json
{
  "dni": 77777777,
  "email": "example789@gmail.com",
  "password": "123456",
  "firstname": "Jose"
}
```



