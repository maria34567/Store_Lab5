# ERD — Store

```mermaid
erDiagram
    Categories ||--o{ Products : contains
    Products ||--o| ProductDetails : has
    Products ||--o| Electronics : "TPT subtype"
    Products ||--o{ ProductSuppliers : linked
    Suppliers ||--o{ ProductSuppliers : linked
    Categories { int Id PK string Name }
    Products { Guid Id PK string Name double Price int CategoryId FK }
    Electronics { Guid Id PK string Brand int WarrantyMonths }
    ProductDetails { Guid ProductId PK string Description }
    Suppliers { int Id PK string Name }
    ProductSuppliers { Guid ProductId PK int SupplierId PK }
```
