
-- Tabla MarketplaceUser
CREATE TABLE MarketplaceUser (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(45),
    email VARCHAR(85),
    password VARCHAR(55)
);

-- Tabla MarketingUser
CREATE TABLE MarketingUser (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(45),
    email VARCHAR(45),
    password VARCHAR(55)
);

-- Tabla Coupons
CREATE TABLE Coupons (
    coupon_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(45),
    description VARCHAR(45),
    creation_date DATE,
    activation_date DATE,
    expiration_date DATE,
    discount_type ENUM('NET', 'PERCENTUAL'),
    discount_value DECIMAL(10, 2),
    use_type ENUM('limited', 'unlimited'),
    quantity_uses INT,
    min_purchase_amount DECIMAL(10, 2),
    max_purchase_amount DECIMAL(10, 2),
    status ENUM('active', 'inactive', 'deleted'),
    redemption_limit INT,
    current_redemptions INT,
    MarketingUserId INT,
    FOREIGN KEY (MarketingUserId) REFERENCES MarketingUser(id)
);

-- Tabla CouponHistory
CREATE TABLE CouponHistory (
    PK INT PRIMARY KEY AUTO_INCREMENT,
    couponId INT,
    change_date DATE,
    field_changed VARCHAR(45),
    old_value VARCHAR(45),
    new_value VARCHAR(45),
    FOREIGN KEY (couponId) REFERENCES Coupons(coupon_id)
);

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
    FOREIGN KEY (couponid) REFERENCES Coupons(coupon_id)
);
