
-- Tabla MarketplaceUser
CREATE TABLE MarketplaceUser (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(45),
    email VARCHAR(85),
    password VARCHAR(55)
);
drop table MarketingUser


-- Tabla MarketingUser
CREATE TABLE MarketingUser (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(45),
    email VARCHAR(45),
    password VARCHAR(55)
);

-- Tabla Coupons
CREATE TABLE Coupons (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(45),
    Description VARCHAR(45),
    CreationDate DATE,
    ActivationDate DATE,
    expiration_date DATE,
    DiscountType ENUM('NET', 'PERCENTUAL'),
    DiscountValue DECIMAL(10, 2),
    UseType ENUM('limited', 'unlimited'),
    quantity_uses INT,
    MinPurchaseAmount DECIMAL(10, 2),
    MaxPurchaseAmount DECIMAL(10, 2),
    status ENUM('active', 'inactive', 'deleted'),
    RedemptionLimit INT,
    CurrentRedemptions INT,
    MarketingUserId INT,
    FOREIGN KEY (MarketingUserId) REFERENCES MarketingUser(id)
);

drop Table Coupons

drop Table CouponsSent


INSERT INTO MarketplaceUser (username, Email, password) 
VALUES 
    ('dina','dinamartinezpant','123'),
    ('Mateo','mateo.velez.censa@gmail.com','123'),
    ('Juan','juanpabloint@gmail.com ','123')
    
drop Table `CouponHistories`

DROP TABLE MarketplaceUser

DROP TABLE MarketplaceUsers

DROP TABLE `MarketplaceUser`

DROP TABLE PurchaseCoupon

-- Tabla CouponHistory
CREATE TABLE CouponHistories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    CouponId INT,
    ChangeDate DATE,
    FieldChanged VARCHAR(45),
    OldValue VARCHAR(45),
    NewValue VARCHAR(45),
    changed_by_user INT,
    FOREIGN KEY (couponId) REFERENCES Coupons(id)
);

ALTER TABLE CouponHistories
CHANGE COLUMN OldValue OldValue VARCHAR(55);

ALTER TABLE CouponHistories
ADD COLUMN ChangedByUser INT;


-- Tabla UserRole
CREATE TABLE UserRole (
    id INT PRIMARY KEY AUTO_INCREMENT,
    MarketingUser_id INT,
    role_id INT,
    FOREIGN KEY (MarketingUser_id) REFERENCES MarketingUser(id),
    FOREIGN KEY (role_id) REFERENCES Role(id)
);

-- Tabla Role
CREATE TABLE Role (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(45)
);

-- Tabla Purchase
CREATE TABLE Purchase (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT,
    date DATE,
    amount DECIMAL(10, 2),
    FOREIGN KEY (user_id) REFERENCES MarketplaceUser(id)
);

-- Tabla CouponUsage
CREATE TABLE CouponUsage (
    couponId INT,
    userId INT,
    usage_date DATE,
    transaction_amount DECIMAL(10, 2),
    status ENUM('redeemed', 'pending'),
    PRIMARY KEY (couponId, userId),
    FOREIGN KEY (couponId) REFERENCES Coupons(coupon_id),
    FOREIGN KEY (userId) REFERENCES MarketplaceUser(id)
);

-- Tabla PurchaseCoupon
CREATE TABLE PurchaseCoupon (
    id INT PRIMARY KEY AUTO_INCREMENT,
    purchaseId INT,
    couponId INT,
    FOREIGN KEY (purchaseId) REFERENCES Purchase(id),
    FOREIGN KEY (couponId) REFERENCES Coupons(coupon_id)
);

-- Tabla Coupons_sent
CREATE TABLE CouponsSent (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT,
    couponid INT,
    FOREIGN KEY (user_id) REFERENCES MarketplaceUser(id),
    FOREIGN KEY (couponid) REFERENCES Coupons(id)
);



INSERT INTO `Coupons`(`name`,`description`,`creation_date`,`activation_date`,`expiration_date`,`discount_type`,`discount_value`,`use_type`,`quantity_uses`,`min_purchase_amount`,`max_purchase_amount`,`status`,`MarketingUserId`,`redemption_limit`,`current_redemptions`) VALUES('Cumpleaños 2','descuento compra','2024-06-14','2024-06-14','2024-07-14','NET',20.000,'limited',3,100000,200000,'active',1,3,0);

INSERT INTO `Coupons`(`name`,`description`,`creation_date`,`activation_date`,`expiration_date`,`discount_type`,`discount_value`,`use_type`,`quantity_uses`,`min_purchase_amount`,`max_purchase_amount`,`status`,`redemption_limit`,`current_redemptions`,`MarketingUserId`) VALUES('Descuento','Descuento por cumpleaños','2024-06-14','2024-06-14','2024-07-14','NET',20.000,'limited',3,100000,200000,'active',3,0,1);


INSERT INTO `Coupons`(
    `Name`, `Description`, `CreationDate`, `ActivationDate`, `expiration_date`, `DiscountType`, 
    `DiscountValue`, `UseType`, `quantity_uses`, `MinPurchaseAmount`, `MaxPurchaseAmount`, 
    `status`, `RedemptionLimit`, `CurrentRedemptions`, `MarketingUserId`
) VALUES (
    'Cupon 1', 'descripcion CUpon 1', '2024-06-16', '2024-06-16', '2024-07-16', 'NET', 
    22000, 'limited', 2, 100000, 200000, 'active', 0, 0, 1
);
