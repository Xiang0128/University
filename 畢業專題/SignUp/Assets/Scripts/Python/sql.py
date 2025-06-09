import mysql.connector
from mysql.connector import errorcode

DB_NAME = "language_learning_system"
USER = "root"  
PASSWORD = ""  
HOST = "localhost"
PORT = "3306"

TABLES = {}

TABLES["user"] = '''
CREATE TABLE `user` (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    user_name VARCHAR(50) NOT NULL,
    user_email VARCHAR(100) UNIQUE,
    user_password VARCHAR(100) NOT NULL,
    last_progress TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;
'''

TABLES["model"] = '''
CREATE TABLE `model` (
    model_id INT AUTO_INCREMENT PRIMARY KEY,
    model_name VARCHAR(100) NOT NULL
) ENGINE=InnoDB;
'''

TABLES["course"] = '''
CREATE TABLE `course` (
    course_id INT AUTO_INCREMENT PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    course_content TEXT,
    word_count INT DEFAULT 0
) ENGINE=InnoDB;
'''

TABLES["sentence"] = '''
CREATE TABLE `sentence` (
    sentence_id INT AUTO_INCREMENT PRIMARY KEY,
    sentence_content TEXT NOT NULL,
    course_id INT,
    sentence_order INT,
    FOREIGN KEY (course_id) REFERENCES course(course_id) ON DELETE CASCADE,
    INDEX (course_id, sentence_order)
) ENGINE=InnoDB;
'''

TABLES["choice_test"] = '''
CREATE TABLE `choice_test` (
    test_id INT AUTO_INCREMENT PRIMARY KEY,
    question_image VARCHAR(255),
    choices_answer TEXT
) ENGINE=InnoDB;
'''

TABLES["practice_test"] = '''
CREATE TABLE `practice_test` (
    test_id INT AUTO_INCREMENT PRIMARY KEY,
    text_question TEXT NOT NULL,
    test_answer TEXT
) ENGINE=InnoDB;
'''

TABLES["sentence_model"] = '''
CREATE TABLE `sentence_model` (
    sentence_id INT,
    model_id INT,
    word_order INT,
    PRIMARY KEY (sentence_id, model_id),
    FOREIGN KEY (sentence_id) REFERENCES sentence(sentence_id) ON DELETE CASCADE,
    FOREIGN KEY (model_id) REFERENCES model(model_id) ON DELETE CASCADE,
    INDEX (model_id)
) ENGINE=InnoDB;
'''

TABLES["model_favorite"] = '''
CREATE TABLE `model_favorite` (
    user_id INT,
    model_id INT,
    PRIMARY KEY (user_id, model_id),
    FOREIGN KEY (user_id) REFERENCES `user`(user_id) ON DELETE CASCADE,
    FOREIGN KEY (model_id) REFERENCES model(model_id) ON DELETE CASCADE
) ENGINE=InnoDB;
'''

TABLES["word"] = '''
CREATE TABLE `word` (
    word_id INT AUTO_INCREMENT PRIMARY KEY,
    word_text VARCHAR(100) NOT NULL,
    model_id INT,
    course_id INT,
    FOREIGN KEY (model_id) REFERENCES model(model_id) ON DELETE SET NULL,
    FOREIGN KEY (course_id) REFERENCES course(course_id) ON DELETE SET NULL
) ENGINE=InnoDB;
'''

def create_database():
    try:
        conn = mysql.connector.connect(
            host=HOST,
            user=USER,
            password=PASSWORD
        )
        cursor = conn.cursor()

        cursor.execute(f"DROP DATABASE IF EXISTS {DB_NAME}")
        print(f"已刪除現有資料庫 `{DB_NAME}`")

        cursor.execute(f"CREATE DATABASE {DB_NAME}")
        print(f"資料庫 `{DB_NAME}` 建立成功")
            
        cursor.close()
        conn.close()
        
    except mysql.connector.Error as err:
        print(f"資料庫操作失敗: {err}")
        raise

def create_tables():
    try:
        conn = mysql.connector.connect(
            host=HOST,
            user=USER,
            password=PASSWORD,
            database=DB_NAME
        )
        cursor = conn.cursor()

        creation_order = [
            'user', 'model', 'course', 
            'sentence', 'choice_test', 'practice_test',
            'sentence_model', 'model_favorite', 'word'
        ]
        
        for table_name in creation_order:
            print(f"建立資料表 `{table_name}`...", end=" ")
            try:
                cursor.execute(TABLES[table_name])
                print("成功")
            except mysql.connector.Error as err:
                print(f"失敗: {err}")
        
        conn.commit()
        cursor.close()
        conn.close()
        print("所有資料表建立完成")
        
    except mysql.connector.Error as err:
        print(f"資料表建立失敗: {err}")
        raise

if __name__ == "__main__":
    print("開始初始化資料庫...")
    create_database()
    create_tables()
    print("資料庫初始化完成")