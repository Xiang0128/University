-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- 主機： 127.0.0.1
-- 產生時間： 2024-05-19 19:55:04
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
-- 資料庫： `hw_4`
--

-- --------------------------------------------------------

--
-- 資料表結構 `director`
--

CREATE TABLE `director` (
  `DNumber` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `YearBorn` int(11) NOT NULL,
  `YearDied` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- 傾印資料表的資料 `director`
--

INSERT INTO `director` (`DNumber`, `Name`, `YearBorn`, `YearDied`) VALUES
(1, 'Woody Allen', 1935, NULL),
(2, 'Steven Spielberg', 1946, NULL),
(3, 'Quentin Tarantino', 1963, NULL);

-- --------------------------------------------------------

--
-- 資料表結構 `movie`
--

CREATE TABLE `movie` (
  `MNumber` int(11) NOT NULL,
  `Title` varchar(200) NOT NULL,
  `Year` int(11) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `Rating` varchar(10) NOT NULL,
  `Nomination#` int(11) NOT NULL,
  `Award#` int(11) NOT NULL,
  `DNumber` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- 傾印資料表的資料 `movie`
--

INSERT INTO `movie` (`MNumber`, `Title`, `Year`, `Type`, `Rating`, `Nomination#`, `Award#`, `DNumber`) VALUES
(1, 'Annie Hall', 1977, 'Comedy', 'PG', 5, 4, 1),
(2, 'Jaws', 1975, 'Thriller', 'PG', 4, 3, 2),
(3, 'Pulp Fiction', 1994, 'Drama', 'R', 7, 1, 3),
(4, 'Manhattan', 1979, 'Comedy', 'R', 2, 0, 1);

-- --------------------------------------------------------

--
-- 資料表結構 `moviestar`
--

CREATE TABLE `moviestar` (
  `MNumber` int(11) NOT NULL,
  `SNumber` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- 傾印資料表的資料 `moviestar`
--

INSERT INTO `moviestar` (`MNumber`, `SNumber`) VALUES
(1, 1),
(3, 2);

-- --------------------------------------------------------

--
-- 資料表結構 `star`
--

CREATE TABLE `star` (
  `SNumber` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Birthplace` varchar(100) NOT NULL,
  `YearBorn` int(11) NOT NULL,
  `YearDied` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- 傾印資料表的資料 `star`
--

INSERT INTO `star` (`SNumber`, `Name`, `Birthplace`, `YearBorn`, `YearDied`) VALUES
(1, 'Diane Keaton', 'Los Angeles, CA', 1946, NULL),
(2, 'John Travolta', 'Englewood, NJ', 1954, NULL);

--
-- 已傾印資料表的索引
--

--
-- 資料表索引 `director`
--
ALTER TABLE `director`
  ADD PRIMARY KEY (`DNumber`);

--
-- 資料表索引 `movie`
--
ALTER TABLE `movie`
  ADD PRIMARY KEY (`MNumber`);

--
-- 資料表索引 `moviestar`
--
ALTER TABLE `moviestar`
  ADD PRIMARY KEY (`MNumber`,`SNumber`);

--
-- 資料表索引 `star`
--
ALTER TABLE `star`
  ADD PRIMARY KEY (`SNumber`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
