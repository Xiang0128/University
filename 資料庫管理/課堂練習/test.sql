-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- 主機： 127.0.0.1
-- 產生時間： 2024-05-09 06:03:30
-- 伺服器版本： 10.4.22-MariaDB
-- PHP 版本： 8.1.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 資料庫: `test`
--

-- --------------------------------------------------------

--
-- 資料表結構 `department`
--

CREATE TABLE `department` (
  `DNumber` int(11) NOT NULL,
  `DName` char(20) NOT NULL,
  `Mgr_SSN` int(11) DEFAULT NULL,
  `Mgr_SDate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 傾印資料表的資料 `department`
--

INSERT INTO `department` (`DNumber`, `DName`, `Mgr_SSN`, `Mgr_SDate`) VALUES
(1, 'Headquarters', 888665555, '1988-05-22 00:00:00'),
(4, 'Administration', 987654321, '1992-06-19 00:00:00'),
(5, 'Research', 333445555, '1985-02-01 00:00:00'),
(6, 'System Development', 555334444, '2018-07-17 00:00:00');

-- --------------------------------------------------------

--
-- 資料表結構 `employee`
--

CREATE TABLE `employee` (
  `SSN` int(11) NOT NULL,
  `Name` char(20) NOT NULL,
  `Salary` int(11) NOT NULL DEFAULT 2000,
  `Supervisor` int(11) DEFAULT NULL,
  `DNumber` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 傾印資料表的資料 `employee`
--

INSERT INTO `employee` (`SSN`, `Name`, `Salary`, `Supervisor`, `DNumber`) VALUES
(123456789, 'Smith', 27000, 333445555, 5),
(333445555, 'Franklin', 50000, 888665555, 5),
(345345345, 'Joyce', 26000, 987654321, 4),
(555334444, 'Michael', 46000, 888665555, 6),
(666112222, 'Maryanne', 35000, 555334444, 6),
(777334444, 'Elizabeth', 33000, 333445555, 5),
(888665555, 'Jeniffer', 55000, NULL, 1),
(987654321, 'James', 45000, 888665555, 4);

-- --------------------------------------------------------

--
-- 資料表結構 `project`
--

CREATE TABLE `project` (
  `PName` char(20) NOT NULL,
  `PNumber` int(11) NOT NULL,
  `DNumber` int(11) NOT NULL DEFAULT 5
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 傾印資料表的資料 `project`
--

INSERT INTO `project` (`PName`, `PNumber`, `DNumber`) VALUES
('Reorganization', 1, 1),
('Multimedia', 3, 5),
('Company DB', 6, 4),
('Internet', 8, 5),
('DB Integration', 10, 6);

-- --------------------------------------------------------

--
-- 資料表結構 `works_on`
--

CREATE TABLE `works_on` (
  `SSN` int(11) NOT NULL,
  `PNumber` int(11) NOT NULL,
  `Hours` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 傾印資料表的資料 `works_on`
--

INSERT INTO `works_on` (`SSN`, `PNumber`, `Hours`) VALUES
(123456789, 3, 15),
(333445555, 3, 10),
(333445555, 6, 10),
(333445555, 8, 20),
(555334444, 6, 25),
(888665555, 1, 20),
(888665555, 3, 8),
(888665555, 8, 5),
(987654321, 1, 10);

--
-- 已傾印資料表的索引
--

--
-- 資料表索引 `department`
--
ALTER TABLE `department`
  ADD PRIMARY KEY (`DNumber`),
  ADD KEY `Mgr_SSN` (`Mgr_SSN`);

--
-- 資料表索引 `employee`
--
ALTER TABLE `employee`
  ADD PRIMARY KEY (`SSN`),
  ADD KEY `Supervisor` (`Supervisor`),
  ADD KEY `DNumber` (`DNumber`);

--
-- 資料表索引 `project`
--
ALTER TABLE `project`
  ADD PRIMARY KEY (`PNumber`),
  ADD UNIQUE KEY `PName` (`PName`),
  ADD KEY `DNumber` (`DNumber`);

--
-- 資料表索引 `works_on`
--
ALTER TABLE `works_on`
  ADD PRIMARY KEY (`SSN`,`PNumber`),
  ADD KEY `PNumber` (`PNumber`);

--
-- 已傾印資料表的限制式
--

--
-- 資料表的限制式 `department`
--
ALTER TABLE `department`
  ADD CONSTRAINT `department_ibfk_1` FOREIGN KEY (`Mgr_SSN`) REFERENCES `employee` (`SSN`);

--
-- 資料表的限制式 `employee`
--
ALTER TABLE `employee`
  ADD CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`Supervisor`) REFERENCES `employee` (`SSN`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `employee_ibfk_2` FOREIGN KEY (`DNumber`) REFERENCES `department` (`DNumber`);

--
-- 資料表的限制式 `project`
--
ALTER TABLE `project`
  ADD CONSTRAINT `project_ibfk_1` FOREIGN KEY (`DNumber`) REFERENCES `department` (`DNumber`);

--
-- 資料表的限制式 `works_on`
--
ALTER TABLE `works_on`
  ADD CONSTRAINT `works_on_ibfk_1` FOREIGN KEY (`SSN`) REFERENCES `employee` (`SSN`),
  ADD CONSTRAINT `works_on_ibfk_2` FOREIGN KEY (`PNumber`) REFERENCES `project` (`PNumber`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
