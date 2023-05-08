DROP DATABASE IF EXISTS Fleurs;
CREATE DATABASE IF NOT EXISTS Fleurs;
USE Fleurs;

CREATE TABLE IF NOT EXISTS address (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  complement varchar(255) DEFAULT NULL,
  street varchar(255) DEFAULT NULL,
  city varchar(255) DEFAULT NULL,
  postal_code int(11) DEFAULT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS category (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS bouquets (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  description text,
  price decimal(10,2) DEFAULT NULL,
  category int(11) DEFAULT NULL,
  stock int(11) DEFAULT NULL,
  alert tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (id),
  FOREIGN KEY (category) REFERENCES category (id)
);

CREATE TABLE IF NOT EXISTS credit_card (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  holder varchar(255) DEFAULT NULL,
  number varchar(16) DEFAULT NULL,
  expiry date DEFAULT NULL,
  ccv int(11) DEFAULT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS clients (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) NOT NULL,
  surname varchar(255) NOT NULL,
  phone varchar(10) NOT NULL,
  email varchar(255) NOT NULL,
  password varchar(255) NOT NULL,
  address int(11) DEFAULT NULL,
  credit_card int(11) DEFAULT NULL,
  employee tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (id,email),
  FOREIGN KEY (address) REFERENCES address (id),
  FOREIGN KEY (credit_card) REFERENCES credit_card (id)
);

CREATE TABLE IF NOT EXISTS flowers (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  price decimal(10,2) DEFAULT NULL,
  start_month int(11) DEFAULT NULL,
  end_month int(11) DEFAULT NULL,
  stock int(11) DEFAULT NULL,
  alert tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (id),
  KEY stock (stock)
);

CREATE TABLE IF NOT EXISTS shops (
  id int(11) NOT NULL AUTO_INCREMENT,
  city varchar(255) NOT NULL,
  PRIMARY KEY (id,city)
);

CREATE TABLE IF NOT EXISTS orders (
  id int(11) NOT NULL AUTO_INCREMENT,
  client_id int(11) NOT NULL,
  type enum('CP','CS') NOT NULL,
  wishes text,
  max_price decimal(10,2) DEFAULT '0.00',
  address int(11) NOT NULL,
  message text,
  delivery date NOT NULL,
  creation_date datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  status enum('VINV','CC','CPAV','CAL','CL') NOT NULL DEFAULT 'VINV',
  shop int(11) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (shop) REFERENCES shops (id),
  FOREIGN KEY (client_id) REFERENCES clients (id),
  FOREIGN KEY (address) REFERENCES address (id)
);

CREATE TABLE IF NOT EXISTS order_bouquets (
  id int(11) NOT NULL AUTO_INCREMENT,
  order_id int(11) NOT NULL,
  bouquet_id int(11) NOT NULL,
  quantity int(11) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (order_id) REFERENCES orders (id),
  FOREIGN KEY (bouquet_id) REFERENCES bouquets (id)
);

CREATE TABLE IF NOT EXISTS order_flowers (
  id int(11) NOT NULL AUTO_INCREMENT,
  order_id int(11) NOT NULL,
  flower_id int(11) NOT NULL,
  quantity int(11) NOT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (order_id) REFERENCES orders (id),
  FOREIGN KEY (flower_id) REFERENCES flowers (id)

);

CREATE TABLE IF NOT EXISTS products (
  id int(11) NOT NULL AUTO_INCREMENT,
  name varchar(255) DEFAULT NULL,
  description text,
  price decimal(10,2) DEFAULT NULL,
  stock int(11) DEFAULT NULL,
  alert tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (id)
  );

CREATE TABLE IF NOT EXISTS order_products (
  id int(11) NOT NULL AUTO_INCREMENT,
  order_id int(11) DEFAULT NULL,
  product_id int(11) DEFAULT NULL,
  quantity int(11) DEFAULT NULL,
  PRIMARY KEY (id),
  FOREIGN KEY (order_id) REFERENCES orders (id),
  FOREIGN KEY (product_id) REFERENCES products (id)
);

DELIMITER $$

CREATE TRIGGER alert_stock_flowers_update
BEFORE UPDATE ON flowers
FOR EACH ROW 
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER alert_stock_flowers_insert
BEFORE INSERT ON flowers
FOR EACH ROW
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER alert_stock_products_update
BEFORE UPDATE ON products
FOR EACH ROW 
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER alert_stock_products_insert
BEFORE INSERT ON products
FOR EACH ROW
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER alert_stock_bouquets_update
BEFORE UPDATE ON bouquets
FOR EACH ROW 
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER alert_stock_bouquets_insert
BEFORE INSERT ON bouquets
FOR EACH ROW
BEGIN
    DECLARE current_stock INT;
    DECLARE current_alert INT;
    SET current_stock = NEW.stock;
    SET current_alert = NEW.alert;
    IF current_stock < 5 AND current_alert = 0 THEN
        SET NEW.alert = 1;
    END IF;
    IF current_stock >= 5 AND current_alert = 1 THEN
        SET NEW.alert = 0;
    END IF;
END $$

CREATE TRIGGER CP_in_CPAV
BEFORE INSERT ON orders
FOR EACH ROW
BEGIN
    IF NEW.type = 'CP' THEN
        SET NEW.status = 'CPAV';
    END IF;
END $$

DELIMITER ;

CREATE USER 'bozo'@'localhost' IDENTIFIED BY 'bozo';
GRANT SELECT ON Fleurs.* TO 'bozo'@'localhost';