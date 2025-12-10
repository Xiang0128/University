-- 創建公司資料庫
CREATE DATABASE `sql_test2`;
USE sql_test2;


-- 創建公司資料庫表格
CREATE TABLE `employee`(
	`emp_id` INT,
    `name` VARCHAR(20),
    `birth_date` DATE,
    `sex` VARCHAR(1),
    `salary` INT,
    `branch_id` INT,
    `sup_id` INT,
    PRIMARY KEY (`emp_id`)
);

CREATE TABLE `branch`(
	`branch_id` INT,
    `branch_name` VARCHAR(20),
    `manager_id` INT,
    PRIMARY KEY (`branch_id`),
    FOREIGN KEY (`manager_id`) REFERENCES `employee`(`emp_id`) ON DELETE SET NULL
);

ALTER TABLE `employee`
ADD FOREIGN KEY (`branch_id`)
REFERENCES `branch`(`branch_id`)
ON DELETE SET NULL;

ALTER TABLE `employee`
ADD FOREIGN KEY (`sup_id`)
REFERENCES `employee`(`emp_id`)
ON DELETE SET NULL;

CREATE TABLE `client`(
	`client_id` INT,
    `client_name` VARCHAR(20),
    `phone` VARCHAR(20),
    PRIMARY KEY (`client_id`)
);

CREATE TABLE `works_with`(
	`emp_id` INT,
    `client_id` INT,
    `total_sales` INT,
    PRIMARY KEY (`emp_id`, `client_id`),
    FOREIGN KEY (`emp_id`) REFERENCES `employee`(`emp_id`) ON DELETE CASCADE,
    FOREIGN KEY (`client_id`) REFERENCES `client`(`client_id`) ON DELETE CASCADE
);


-- 新增公司資料
INSERT INTO `branch` VALUES(1, '研發', NULL);
INSERT INTO `branch` VALUES(2, '行政', NULL);
INSERT INTO `branch` VALUES(3, '資訊', NULL);

INSERT INTO `employee` VALUES(206, '小黃', '1998-10-08', 'F', 50000, 1, NULL);
INSERT INTO `employee` VALUES(207, '小綠', '1985-09-16', 'M', 29000, 2, 206);
INSERT INTO `employee` VALUES(208, '小黑', '2000-12-19', 'M', 35000, 3, 206);
INSERT INTO `employee` VALUES(209, '小白', '1997-01-22', 'F', 39000, 3, 207);
INSERT INTO `employee` VALUES(210, '小蘭', '1925-11-10', 'F', 84000, 1, 207);

UPDATE `branch`
SET `manager_id` = 206
WHERE `branch_id` = 1;

UPDATE `branch`
SET `manager_id` = 207
WHERE `branch_id` = 2;

UPDATE `branch`
SET `manager_id` = 208
WHERE `branch_id` = 3;

INSERT INTO `client` VALUES(400, '阿狗', '254354335');
INSERT INTO `client` VALUES(401, '阿貓', '25633899');
INSERT INTO `client` VALUES(402, '旺來', '45354345');
INSERT INTO `client` VALUES(403, '露西', '54354365');
INSERT INTO `client` VALUES(404, '艾瑞克', '18783783');

INSERT INTO `works_with` VALUES(206, 400, '70000');
INSERT INTO `works_with` VALUES(207, 401, '24000');
INSERT INTO `works_with` VALUES(208, 402, '9800');
INSERT INTO `works_with` VALUES(208, 403, '24000');
INSERT INTO `works_with` VALUES(210, 404, '87940');


-- 練習

-- 1. 取得所有員工資料
SELECT * FROM `employee`;

-- 2. 取得所有客戶資料
SELECT * FROM `client`;

-- 3. 按薪水低到高取得員工資料
SELECT * FROM `employee`
ORDER BY `salary`;

-- 4. 取得薪水前3高的員工
SELECT * FROM `employee`
ORDER BY `salary` DESC
LIMIT 3;

-- 5. 取得所有員工名字
SELECT `name` FROM `employee`;


-- aggregate functions 聚合函數

-- 1. 取得員工人數
SELECT COUNT(*) FROM `employee`;

-- 2. 取得所有出生於 1970-01-01 之後的女性員工人數
SELECT COUNT(*) FROM `employee`
WHERE `birth_date` > '1970-01-01'
AND `sex` = 'F';

-- 3. 取得所有員工的平均薪水
SELECT AVG(`salary`) FROM `employee`;

-- 4. 取得所有員工薪水的總和
SELECT SUM(`salary`) FROM `employee`;

-- 5. 取得薪水最高的員工
SELECT MAX(`salary`) FROM `employee`;

-- 6. 取得薪水最低的員工
SELECT MIN(`salary`) FROM `employee`;


-- wildcards 萬用字元	% 代表多個字元, _ 代表一個字元

-- 1. 取得電話號碼尾數是335的客戶
SELECT * FROM `client`
WHERE `phone` LIKE '%335';

-- 2. 取得姓艾的客戶
SELECT * FROM `client`
WHERE `client_name` LIKE '艾%';

-- 3. 取得生日在12月的員工
SELECT * FROM `employee`
WHERE `birth_date` LIKE '_____12%';


-- union 聯集

-- 1. 員工名字 union 客戶名字
SELECT `name` FROM `employee`
UNION
SELECT `client_name` FROM `client`;

-- 2. 員工id + 員工名字 union 客戶id + 客戶名字
SELECT `emp_id` AS `total_id`, `name` AS `total_name` FROM `employee`
UNION
SELECT `client_id`, `client_name` FROM `client`;
-- 3. 員工薪水 union 銷售金額
SELECT `salary` AS `total_money` FROM `employee`
UNION
SELECT `total_sales` FROM `works_with`;


-- join 連接

INSERT INTO `branch` VALUES(4, '偷懶', NULL);

-- 取得所有部門經理的名字
SELECT `employee`.`emp_id`, `employee`.`name`, `branch`.`branch_name` FROM `employee`
JOIN `branch`
ON `employee`.`emp_id` = `branch`.`manager_id`;


-- subquery 子查詢

-- 1. 找出研發部門的經理名字
SELECT `name` FROM `employee`
WHERE `emp_id` = (
	SELECT `manager_id` FROM `branch`
	WHERE `branch_name` = '研發'
);

-- 2. 找出對單一位客戶銷售金額超過50000的員工名字
SELECT `name` FROM `employee`
WHERE `emp_id` IN(
SELECT `emp_id` FROM `works_with`
WHERE `total_sales` > 50000
);


-- on delete

CREATE TABLE `branch`(
	`branch_id` INT,
    `branch_name` VARCHAR(20),
    `manager_id` INT,
    PRIMARY KEY (`branch_id`),
    FOREIGN KEY (`manager_id`) REFERENCES `employee`(`emp_id`) ON DELETE SET NULL
);

CREATE TABLE `works_with`(
	`emp_id` INT,
    `client_id` INT,
    `total_sales` INT,
    PRIMARY KEY (`emp_id`, `client_id`),
    FOREIGN KEY (`emp_id`) REFERENCES `employee`(`emp_id`) ON DELETE CASCADE,
    FOREIGN KEY (`client_id`) REFERENCES `client`(`client_id`) ON DELETE CASCADE
);


DELETE FROM `employee`
WHERE `emp_id` = 207;

SELECT * FROM `branch`;

SELECT * FROM `works_with`;


