-- phpMyAdmin SQL Dump
-- version 4.5.4.1deb2ubuntu2.1
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Apr 28, 2023 at 04:57 PM
-- Server version: 5.7.33-0ubuntu0.16.04.1
-- PHP Version: 7.0.33-0ubuntu0.16.04.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `Fleurs`
--

-- --------------------------------------------------------

--
-- Table structure for table `address`
--

CREATE TABLE `address` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `complement` varchar(255) DEFAULT NULL,
  `street` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `postal_code` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `address`
--

INSERT INTO `address` (`id`, `name`, `complement`, `street`, `city`, `postal_code`) VALUES
(1, 'Home', 'Apartment 10', '123 Main St', 'New York', 10001),
(2, 'Work', 'Suite 100', '456 Park Ave', 'Los Angeles', 90001),
(3, 'Home', '', '789 Broadway', 'Chicago', 60007),
(4, 'Work', 'Floor 3', '1010 Market St', 'Houston', 77002),
(5, 'Home', '', '1234 Elm St', 'Miami', 33101);

-- --------------------------------------------------------

--
-- Table structure for table `bouquets`
--

CREATE TABLE `bouquets` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text,
  `price` decimal(10,2) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `bouquets`
--

INSERT INTO `bouquets` (`id`, `name`, `description`, `price`, `category`, `stock`) VALUES
(1, 'Gros Merci', 'Arrangement floral avec marguerites et verdure', '45.00', 1, 10),
(2, 'L’amoureux', 'Arrangement floral avec roses blanches et roses rouges', '65.00', 2, 10),
(3, 'L’Exotique', 'Arrangement floral avec ginger, oiseaux du paradis, roses et genet', '40.00', 1, 10),
(4, 'Maman', 'Arrangement floral avec gerbera, roses blanches, lys et alstroméria', '80.00', 3, 10),
(5, 'Vive la mariée', 'Arrangement floral avec lys et orchidées', '120.00', 4, 10);

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

CREATE TABLE `category` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `category`
--

INSERT INTO `category` (`id`, `name`) VALUES
(1, 'Toute occasion'),
(2, 'St-Valentin'),
(3, 'Fête des mères'),
(4, 'Mariage');

-- --------------------------------------------------------

--
-- Table structure for table `clients`
--

CREATE TABLE `clients` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `surname` varchar(255) DEFAULT NULL,
  `phone` varchar(11) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `address` int(11) DEFAULT NULL,
  `credit_card` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `clients`
--

INSERT INTO `clients` (`id`, `name`, `surname`, `phone`, `email`, `password`, `address`, `credit_card`) VALUES
(11, 'John', 'Doe', '1234567890', 'john.doe@example.com', 'password', 1, 1),
(12, 'Jane', 'Doe', '0987654321', 'jane.doe@example.com', 'password', 2, 2),
(13, 'Bob', 'Smith', '5551234567', 'bob.smith@example.com', 'password', 3, 3),
(14, 'Alice', 'Johnson', '5559876543', 'alice.johnson@example.com', 'password', 4, 4),
(15, 'Tom', 'Wilson', '5555555555', 'tom.wilson@example.com', 'password', 5, 5);

-- --------------------------------------------------------

--
-- Table structure for table `credit_card`
--

CREATE TABLE `credit_card` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `holder` varchar(255) DEFAULT NULL,
  `number` varchar(16) DEFAULT NULL,
  `expiry` date DEFAULT NULL,
  `ccv` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `credit_card`
--

INSERT INTO `credit_card` (`id`, `name`, `holder`, `number`, `expiry`, `ccv`) VALUES
(1, 'Visa', 'John Doe', '1234567812345678', '2025-01-01', 123),
(2, 'Mastercard', 'Jane Doe', '8765432187654321', '2023-02-01', 456),
(3, 'Discover', 'Bob Smith', '1111222233334444', '2024-03-01', 789),
(4, 'American Express', 'Alice Johnson', '5555666677778888', '2025-04-01', 12),
(5, 'Visa', 'Tom Wilson', '9999888877776666', '2022-05-01', 345);

-- --------------------------------------------------------

--
-- Table structure for table `flowers`
--

CREATE TABLE `flowers` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL,
  `start_month` int(11) DEFAULT NULL,
  `end_month` int(11) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `flowers`
--

INSERT INTO `flowers` (`id`, `name`, `price`, `start_month`, `end_month`, `stock`) VALUES
(26, 'Gerbera', '5.00', 1, 12, 10),
(27, 'Ginger', '4.00', 1, 12, 10),
(28, 'Glaïeul', '1.00', 5, 11, 10),
(29, 'Marguerite', '2.25', 1, 12, 10),
(30, 'Rose rouge', '2.50', 1, 12, 10);

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `id` int(11) NOT NULL,
  `client_id` int(11) DEFAULT NULL,
  `type` enum('CP','CS') DEFAULT NULL,
  `wishes` text,
  `max_price` decimal(10,2) DEFAULT NULL,
  `address` int(11) DEFAULT NULL,
  `message` text,
  `delivery` date DEFAULT NULL,
  `creation_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `status` enum('VINV','CC','CPAV','CAL','CL') DEFAULT 'VINV'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`id`, `client_id`, `type`, `wishes`, `max_price`, `address`, `message`, `delivery`, `creation_date`, `status`) VALUES
(1, 11, 'CS', NULL, NULL, 1, 'Bonjour mon petit Marc, cette commande est pour toi. Je t\'aime. Ton amoureux secret', '2023-05-01', '2023-04-28 16:48:38', 'VINV'),
(2, 12, 'CP', 'Je veux pleins de fleurs et trois vases. Kisses', '20.50', 2, NULL, '2023-04-11', '2023-04-28 16:51:03', 'VINV'),
(3, 13, 'CP', NULL, NULL, 3, 'Coucou, je t\'aime pas.', '2023-04-24', '2023-04-28 16:51:28', 'VINV');

-- --------------------------------------------------------

--
-- Table structure for table `order_bouquets`
--

CREATE TABLE `order_bouquets` (
  `id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `bouquet_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `order_bouquets`
--

INSERT INTO `order_bouquets` (`id`, `order_id`, `bouquet_id`, `quantity`) VALUES
(1, 1, 1, 1),
(2, 1, 2, 2);

-- --------------------------------------------------------

--
-- Table structure for table `order_flowers`
--

CREATE TABLE `order_flowers` (
  `id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `flower_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `order_flowers`
--

INSERT INTO `order_flowers` (`id`, `order_id`, `flower_id`, `quantity`) VALUES
(1, 3, 28, 3);

-- --------------------------------------------------------

--
-- Table structure for table `order_products`
--

CREATE TABLE `order_products` (
  `id` int(11) NOT NULL,
  `order_id` int(11) DEFAULT NULL,
  `product_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `order_products`
--

INSERT INTO `order_products` (`id`, `order_id`, `product_id`, `quantity`) VALUES
(1, 3, 4, 2);

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text,
  `price` decimal(10,2) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`id`, `name`, `description`, `price`, `stock`) VALUES
(1, 'Vase en verre', 'Un vase en verre élégant et raffiné.', '20.00', 50),
(2, 'Boîte-cadeau', 'Une boîte-cadeau de qualité pour offrir vos fleurs.', '10.00', 100),
(3, 'Ruban', 'Un ruban en satin pour parfaire la présentation de vos bouquets.', '5.00', 200),
(4, 'Carte de vœux', 'Une carte de vœux pour accompagner votre bouquet.', '3.00', 500),
(5, 'Chocolats', 'Des chocolats fins pour accompagner votre bouquet.', '15.00', 100);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `address`
--
ALTER TABLE `address`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `bouquets`
--
ALTER TABLE `bouquets`
  ADD PRIMARY KEY (`id`),
  ADD KEY `stock` (`stock`),
  ADD KEY `category` (`category`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`id`),
  ADD KEY `address` (`address`),
  ADD KEY `credit_card` (`credit_card`);

--
-- Indexes for table `credit_card`
--
ALTER TABLE `credit_card`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `flowers`
--
ALTER TABLE `flowers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `stock` (`stock`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `address` (`address`),
  ADD KEY `orders_ibfk_1` (`client_id`);

--
-- Indexes for table `order_bouquets`
--
ALTER TABLE `order_bouquets`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `bouquet_id` (`bouquet_id`);

--
-- Indexes for table `order_flowers`
--
ALTER TABLE `order_flowers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `flower_id` (`flower_id`);

--
-- Indexes for table `order_products`
--
ALTER TABLE `order_products`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `product_id` (`product_id`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`id`),
  ADD KEY `stock` (`stock`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `address`
--
ALTER TABLE `address`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `bouquets`
--
ALTER TABLE `bouquets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;
--
-- AUTO_INCREMENT for table `credit_card`
--
ALTER TABLE `credit_card`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `flowers`
--
ALTER TABLE `flowers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;
--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `order_bouquets`
--
ALTER TABLE `order_bouquets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `order_flowers`
--
ALTER TABLE `order_flowers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `order_products`
--
ALTER TABLE `order_products`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `bouquets`
--
ALTER TABLE `bouquets`
  ADD CONSTRAINT `bouquets_ibfk_3` FOREIGN KEY (`category`) REFERENCES `category` (`id`);

--
-- Constraints for table `clients`
--
ALTER TABLE `clients`
  ADD CONSTRAINT `clients_ibfk_1` FOREIGN KEY (`address`) REFERENCES `address` (`id`),
  ADD CONSTRAINT `clients_ibfk_2` FOREIGN KEY (`credit_card`) REFERENCES `credit_card` (`id`);

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`id`),
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`address`) REFERENCES `address` (`id`);

--
-- Constraints for table `order_bouquets`
--
ALTER TABLE `order_bouquets`
  ADD CONSTRAINT `order_bouquets_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_bouquets_ibfk_2` FOREIGN KEY (`bouquet_id`) REFERENCES `bouquets` (`id`);

--
-- Constraints for table `order_flowers`
--
ALTER TABLE `order_flowers`
  ADD CONSTRAINT `order_flowers_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_flowers_ibfk_2` FOREIGN KEY (`flower_id`) REFERENCES `flowers` (`id`);

--
-- Constraints for table `order_products`
--
ALTER TABLE `order_products`
  ADD CONSTRAINT `order_products_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_products_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
