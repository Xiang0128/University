-- 創建資料庫
CREATE DATABASE `sql_test1`;
SHOW DATABASES;
USE `sql_test1`;

-- 創建表格
CREATE TABLE `student`(
	`student_id` INT AUTO_INCREMENT,
    `name` VARCHAR(20) NOT NULL,
    `major` VARCHAR(20) NOT NULL,
    PRIMARY KEY(`student_id`)
);
SELECT * FROM `student`;

-- 呈現、刪除表格
DESCRIBE `student`;
DROP TABLE `student`;

-- 新增表格屬性
ALTER TABLE `student` ADD gpa DECIMAL(3,2);
ALTER TABLE `student` DROP COLUMN gpa;

ALTER TABLE `student` ADD `score` INT;

UPDATE `student` SET `score` = 20 WHERE `student_id` = 1;
UPDATE `student` SET `score` = 90 WHERE `student_id` = 2;
UPDATE `student` SET `score` = 70 WHERE `student_id` = 3;
UPDATE `student` SET `score` = 80 WHERE `student_id` = 4;
UPDATE `student` SET `score` = 20 WHERE `student_id` = 5;

-- 儲存資料
INSERT INTO `student` VALUES(1, '小白', '英語');
INSERT INTO `student` VALUES(2, '小黃', '生物');
INSERT INTO `student` VALUES(3, '小綠', '歷史');

INSERT INTO `student`(`student_id`, `name`, `major`) VALUES(1, '小白', '英語');
INSERT INTO `student`(`student_id`, `name`, `major`) VALUES(2, '小黃', '生物');
INSERT INTO `student`(`student_id`, `name`, `major`) VALUES(3, '小綠', '歷史');
INSERT INTO `student`(`student_id`, `name`, `major`) VALUES(4, '小藍', '英語');
INSERT INTO `student`(`student_id`, `name`, `major`) VALUES(5, '小黑', '化學');

-- 修改資料
UPDATE `student`
SET `major` = '英語文學'
WHERE `major` = '英語';

UPDATE `student`
SET `major` = '生物'
WHERE `student_id` = 3;

UPDATE `student`
SET `major` = '生化'
WHERE `major` = '生物' OR `major` = '化學';

UPDATE `student`
SET `name` = '小灰', `major` = '物理'
WHERE `student_id` = 1;

UPDATE `student`
SET `major` = '物理';		# 修改表格所有`student`中的屬性`major`為'物理'

-- 刪除資料
DELETE FROM `student`
WHERE `student_id` = 4;

DELETE FROM `student`
WHERE `name` = '小灰' AND `major` = '物理';

DELETE FROM `student`
WHERE `score` < 60;

DELETE FROM `student`;		# 刪除表格`student`的所有資料

-- 取得資料
SELECT * FROM `student`;	# 取得表格`student`的所有資料

SELECT `name`, `major` FROM `student`;

-- 排序資料
SELECT * FROM `student` 
ORDER BY `score` ASC;		# 由低到高

SELECT * FROM `student` 
ORDER BY `score` DESC;		# 由高到低

SELECT * FROM `student` 
ORDER BY `score`, `student_id`;		# 假設`score`相同，就以`student_id`做排序

-- 限制取得資料數
SELECT * FROM `student`
LIMIT 3;					# 僅回傳前三筆

SELECT * FROM `student`
ORDER BY `score`
LIMIT 3;					# 取得`score`前三低的資料

SELECT * FROM `student`
ORDER BY `score` DESC
LIMIT 3;					# 取得`score`前三高的資料

SELECT * FROM `student`
WHERE `major` = '英語';

SELECT * FROM `student`
WHERE `major` = '英語' AND `student_id` = 1;

SELECT * FROM `student`
WHERE `major` = '英語' OR `score` > 20;

SELECT * FROM `student`
WHERE `major` = '英語' OR `score` <> 70
LIMIT 2;

SELECT * FROM `student`
WHERE `major` IN('歷史', '英語', '生物');		# = WHERE `major` = '歷史' OR `major` = '英語' OR `major` = '生物');