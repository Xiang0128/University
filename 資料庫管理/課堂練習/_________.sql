-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- 主機： 127.0.0.1
-- 產生時間： 2024-06-14 13:39:41
-- 伺服器版本： 10.4.32-MariaDB
-- PHP 版本： 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 資料庫： `資料庫`
--

-- --------------------------------------------------------

--
-- 資料表結構 `buyer`
--

CREATE TABLE `buyer` (
  `user_no` varchar(10) NOT NULL,
  `address` varchar(100) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- 傾印資料表的資料 `buyer`
--

INSERT INTO `buyer` (`user_no`, `address`) VALUES
('C1', '1'),
('C123', ''),
('C3', '1'),
('C4', '1'),
('C456', '嘉義市180號');

-- --------------------------------------------------------

--
-- 資料表結構 `orders`
--

CREATE TABLE `orders` (
  `quantity` int(11) NOT NULL,
  `order_no` varchar(10) NOT NULL,
  `total_price` int(100) NOT NULL,
  `time` datetime(6) NOT NULL,
  `user_no` varchar(10) NOT NULL,
  `transport_no` varchar(10) NOT NULL,
  `transport_situation` varchar(1) NOT NULL,
  `product_no` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- 傾印資料表的資料 `orders`
--

INSERT INTO `orders` (`quantity`, `order_no`, `total_price`, `time`, `user_no`, `transport_no`, `transport_situation`, `product_no`) VALUES
(10, '1', 300, '2024-04-16 00:45:38.000000', 'C1', 'D1', 'D', '1'),
(20, '2', 600, '2024-05-14 00:33:31.000000', 'C123', '123', 'C', '1'),
(10, '3', 350, '2024-04-23 00:34:06.000000', 'C3', '123', 'C', '2'),
(8, '4', 400, '2024-04-15 00:34:35.000000', 'C4', 'D1', 'A', '3');

-- --------------------------------------------------------

--
-- 資料表結構 `product`
--

CREATE TABLE `product` (
  `product_no` varchar(10) NOT NULL,
  `product_name` varchar(225) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `product_quantity` int(100) NOT NULL,
  `picture_path` varchar(100) NOT NULL,
  `introduction` varchar(100) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `price` int(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- 傾印資料表的資料 `product`
--

INSERT INTO `product` (`product_no`, `product_name`, `product_quantity`, `picture_path`, `introduction`, `price`) VALUES
('1', '蘋果', 50, '', '', 30),
('2', '鳳梨', 60, '', '', 35),
('3', '西瓜', 60, '', '', 50);

-- --------------------------------------------------------

--
-- 資料表結構 `seller`
--

CREATE TABLE `seller` (
  `user_no` varchar(10) NOT NULL,
  `bank_account` varchar(14) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

-- --------------------------------------------------------

--
-- 資料表結構 `transport`
--

CREATE TABLE `transport` (
  `transport_no` varchar(10) NOT NULL,
  `transport_company` varchar(100) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `transport_price` int(20) NOT NULL,
  `transport_situation` varchar(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- 傾印資料表的資料 `transport`
--

INSERT INTO `transport` (`transport_no`, `transport_company`, `transport_price`, `transport_situation`) VALUES
('123', 'v', 0, ''),
('D1', '7-11', 38, 'D');

-- --------------------------------------------------------

--
-- 資料表結構 `user`
--

CREATE TABLE `user` (
  `user_no` varchar(10) NOT NULL,
  `user_name` varchar(10) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `phone` varchar(10) NOT NULL,
  `identity` varchar(2) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- 傾印資料表的資料 `user`
--

INSERT INTO `user` (`user_no`, `user_name`, `phone`, `identity`, `password`) VALUES
('C1', '2', '1', '買家', '123'),
('C123', 'lo', '', '', '123'),
('C3', '\\lo', '1', '買家', '123'),
('C4', 'cx', '1', '買家', '123'),
('C456', '4', '4', '買家', '4'),
('D1', '1', '1', '物流', '1'),
('S1', '1', '1', '賣家', '1');

--
-- 已傾印資料表的索引
--

--
-- 資料表索引 `buyer`
--
ALTER TABLE `buyer`
  ADD KEY `user_no.` (`user_no`);

--
-- 資料表索引 `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`order_no`),
  ADD KEY `product_no.` (`product_no`),
  ADD KEY `user_no.` (`user_no`),
  ADD KEY `transport_no.` (`transport_no`);

--
-- 資料表索引 `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`product_no`);

--
-- 資料表索引 `seller`
--
ALTER TABLE `seller`
  ADD KEY `user_no.` (`user_no`);

--
-- 資料表索引 `transport`
--
ALTER TABLE `transport`
  ADD PRIMARY KEY (`transport_no`);

--
-- 資料表索引 `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`user_no`);

--
-- 已傾印資料表的限制式
--

--
-- 資料表的限制式 `buyer`
--
ALTER TABLE `buyer`
  ADD CONSTRAINT `buyer_ibfk_1` FOREIGN KEY (`user_no`) REFERENCES `user` (`user_no`) ON DELETE CASCADE;

--
-- 資料表的限制式 `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`product_no`) REFERENCES `product` (`product_no`) ON DELETE CASCADE,
  ADD CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`user_no`) REFERENCES `buyer` (`user_no`) ON DELETE CASCADE,
  ADD CONSTRAINT `orders_ibfk_3` FOREIGN KEY (`transport_no`) REFERENCES `transport` (`transport_no`) ON DELETE CASCADE;

--
-- 資料表的限制式 `seller`
--
ALTER TABLE `seller`
  ADD CONSTRAINT `seller_ibfk_1` FOREIGN KEY (`user_no`) REFERENCES `user` (`user_no`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
