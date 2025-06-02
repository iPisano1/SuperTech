# **Project App**

Made For Client.

### **Execute this Sql Query**
<pre>```CREATE DATABASE IF NOT EXISTS `computer_shop_system` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `computer_shop_system`;

CREATE TABLE IF NOT EXISTS `accounts` (
  `User ID` int(11) NOT NULL AUTO_INCREMENT,
  `Profile Photo` longblob DEFAULT NULL,
  `Username` varchar(50) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `First Name` varchar(50) DEFAULT NULL,
  `Last Name` varchar(50) DEFAULT NULL,
  `Address` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Phone Number` varchar(50) DEFAULT NULL,
  `Permission` varchar(50) DEFAULT 'Customer',
  `Date Created` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`User ID`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE IF NOT EXISTS `orders` (
  `Order ID` int(11) NOT NULL AUTO_INCREMENT,
  `User ID` int(11) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Total Amount` int(11) DEFAULT NULL,
  `Date Ordered` timestamp NULL DEFAULT current_timestamp(),
  `Status` varchar(50) DEFAULT 'Pending',
  PRIMARY KEY (`Order ID`),
  KEY `FK_orders_accounts` (`User ID`),
  KEY `FK_orders_accounts_2` (`Email`),
  CONSTRAINT `FK_orders_accounts` FOREIGN KEY (`User ID`) REFERENCES `accounts` (`User ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_orders_accounts_2` FOREIGN KEY (`Email`) REFERENCES `accounts` (`Email`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE IF NOT EXISTS `products` (
  `Product ID` int(11) NOT NULL AUTO_INCREMENT,
  `Image` longblob DEFAULT NULL,
  `Name` longtext NOT NULL,
  `Type` varchar(120) NOT NULL DEFAULT '0',
  `Price` int(11) NOT NULL DEFAULT 0,
  `Stocks` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Product ID`),
  UNIQUE KEY `Price` (`Price`),
  UNIQUE KEY `Name` (`Name`) USING HASH
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE IF NOT EXISTS `receipts` (
  `Receipt ID` int(11) NOT NULL AUTO_INCREMENT,
  `Order ID` int(11) DEFAULT NULL,
  `User ID` int(11) DEFAULT NULL,
  `Product ID` int(11) DEFAULT NULL,
  `Quantity` int(11) DEFAULT NULL,
  `Total Price` int(11) DEFAULT NULL,
  `Payment Method` varchar(50) DEFAULT NULL,
  `Date Ordered` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`Receipt ID`),
  KEY `FK_receipts_accounts` (`User ID`),
  KEY `FK_receipts_orders` (`Order ID`),
  KEY `FK_receipts_products` (`Product ID`),
  CONSTRAINT `FK_receipts_accounts` FOREIGN KEY (`User ID`) REFERENCES `accounts` (`User ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_receipts_orders` FOREIGN KEY (`Order ID`) REFERENCES `orders` (`Order ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_receipts_products` FOREIGN KEY (`Product ID`) REFERENCES `products` (`Product ID`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE IF NOT EXISTS `shopping_cart` (
  `Cart ID` int(11) NOT NULL AUTO_INCREMENT,
  `User ID` int(11) NOT NULL DEFAULT 0,
  `Product ID` int(11) NOT NULL DEFAULT 0,
  `Quantity` int(11) NOT NULL DEFAULT 1,
  `Total Price` int(11) NOT NULL,
  `Added` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Cart ID`),
  KEY `FK_shopping_cart_accounts` (`User ID`),
  KEY `FK_shopping_cart_products` (`Product ID`),
  CONSTRAINT `FK_shopping_cart_accounts` FOREIGN KEY (`User ID`) REFERENCES `accounts` (`User ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_shopping_cart_products` FOREIGN KEY (`Product ID`) REFERENCES `products` (`Product ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;```</pre>
