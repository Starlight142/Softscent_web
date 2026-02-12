# Database Sequence Diagrams (System Flow)

## 1. User Registration Flow (สมัครสมาชิก)
Interaction between User, Web Application, and Identity Tables (`Users`, `Roles`, `UserRoles`).

```mermaid
sequenceDiagram
    participant User
    participant WebApp as Web Application
    participant DB_Users as Table: Users
    participant DB_Roles as Table: Roles
    participant DB_UserRoles as Table: UserRoles

    User->>WebApp: Fill Registration Form (Email, Password, Info)
    WebApp->>DB_Users: Check if Email exists
    DB_Users-->>WebApp: Result (Exists/Not)
    
    alt Email already exists
        WebApp-->>User: Show Error "Email taken"
    else Email is unique
        WebApp->>WebApp: Hash Password
        WebApp->>DB_Users: INSERT INTO Users (Id, Email, PasswordHash, FullName...)
        DB_Users-->>WebApp: Success
        
        WebApp->>DB_Roles: SELECT Id FROM Roles WHERE Name = 'Member'
        DB_Roles-->>WebApp: Return RoleId
        
        WebApp->>DB_UserRoles: INSERT INTO UserRoles (UserId, RoleId)
        DB_UserRoles-->>WebApp: Success
        
        WebApp-->>User: Registration Complete (Redirect to Login)
    end
```

## 2. Product Purchase Flow (การสั่งซื้อสินค้า)
Interaction when a user places a normal order involving `Orders` and `OrderDetails`.

```mermaid
sequenceDiagram
    participant User
    participant WebApp as Web Application
    participant DB_Products as Table: Products
    participant DB_Orders as Table: Orders
    participant DB_OrderDetails as Table: OrderDetails

    User->>WebApp: Add Items to Cart
    User->>WebApp: Checkout & Confirm Order

    WebApp->>DB_Products: SELECT Price FROM Products WHERE Id IN (CartItems)
    DB_Products-->>WebApp: Return Prices (Validate amounts)
    
    WebApp->>WebApp: Calculate TotalAmount

    WebApp->>DB_Orders: INSERT INTO Orders (UserId, TotalAmount, Status='Pending', ...)
    DB_Orders-->>WebApp: Return New OrderId

    loop For Each Item in Cart
        WebApp->>DB_OrderDetails: INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice)
    end
    DB_OrderDetails-->>WebApp: Success

    WebApp-->>User: Show Order Confirmation
```

## 3. Custom Blend Order Flow (สั่งปรุงน้ำหอม/ยาดม)
Interaction for the Custom Inhaler feature involving `Herbs` and `OrderDetails` configuration.

```mermaid
sequenceDiagram
    participant User
    participant WebApp as Web Application
    participant DB_Herbs as Table: Herbs
    participant DB_Orders as Table: Orders
    participant DB_OrderDetails as Table: OrderDetails

    User->>WebApp: Select Herbs for Blend (e.g., Peppermint, Lavender)
    WebApp->>DB_Herbs: SELECT * FROM Herbs
    DB_Herbs-->>WebApp: Show List
    
    User->>WebApp: Confirm Selection & Add to Cart
    
    WebApp->>DB_Orders: INSERT INTO Orders (UserId, TotalAmount, ...)
    DB_Orders-->>WebApp: Return OrderId

    WebApp->>DB_OrderDetails: INSERT INTO OrderDetails (OrderId, ProductId=SpecialID, ..., CustomConfiguration="Peppermint, Lavender")
    Note right of DB_OrderDetails: "CustomConfiguration" field stores the selected herbs string
    DB_OrderDetails-->>WebApp: Success

    WebApp-->>User: Order Confirmed
```

## 4. Admin Update Order Status (จัดการสถานะออเดอร์)
Admin updates an order from "Pending" to "Shipped".

```mermaid
sequenceDiagram
    participant Admin
    participant WebApp as Web Application
    participant DB_Orders as Table: Orders

    Admin->>WebApp: View Order Management Page
    WebApp->>DB_Orders: SELECT * FROM Orders ORDER BY Date DESC
    DB_Orders-->>WebApp: Return Order List
    WebApp-->>Admin: Show Orders Table

    Admin->>WebApp: Change Status to "Shipped"
    WebApp->>DB_Orders: UPDATE Orders SET Status='Shipped' WHERE Id=@Id
    DB_Orders-->>WebApp: Success
    
    WebApp-->>Admin: Refresh Table (Status Updated)
```

## 5. Admin Manage Products (จัดการสินค้า)
Adding a new product or ingredient.

```mermaid
sequenceDiagram
    participant Admin
    participant WebApp as Web Application
    participant DB_Products as Table: Products
    participant DB_Herbs as Table: Herbs

    alt Add Normal Product
        Admin->>WebApp: Enter Product Details (Name, Price, Image)
        WebApp->>DB_Products: INSERT INTO Products (...)
        DB_Products-->>WebApp: Success
    else Add Custom Ingredient
        Admin->>WebApp: Enter Ingredient Details (Name, Benefit)
        WebApp->>DB_Herbs: INSERT INTO Herbs (...)
        DB_Herbs-->>WebApp: Success
    end
    
    WebApp-->>Admin: Update List
```
