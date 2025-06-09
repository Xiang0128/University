from flask import Flask, request, jsonify
import mysql.connector
import bcrypt

app = Flask(__name__)

#Database config
DB_CONFIG = {
    'host': 'localhost',
    'user': 'root',
    'password': '',
    'database': 'language_learning_system'
}

def get_db_connection():
    return mysql.connector.connect(**DB_CONFIG)

#User Registration
@app.route("/register", methods=["POST"])
def register():
    data = request.get_json()
    user_name = data.get("user_name")
    user_email = data.get("user_email")
    user_password = data.get("user_password")

    if not user_name or not user_email or not user_password:
        return jsonify({"status": "fail", "message": "Missing required fields"}), 400

    hashed = bcrypt.hashpw(user_password.encode(), bcrypt.gensalt()).decode()

    conn = get_db_connection()
    cursor = conn.cursor()
    try:
        cursor.execute("""
            INSERT INTO user (user_name, user_email, user_password)
            VALUES (%s, %s, %s)
        """, (user_name, user_email, hashed))
        conn.commit()
    except Exception as e:
        return jsonify({"status": "fail", "message": str(e)}), 500
    finally:
        cursor.close()
        conn.close()

    return jsonify({"status": "success", "message": "User registered"}), 201

#User Login
@app.route("/login", methods=["POST"])
def login():
    data = request.get_json()
    user_email = data.get("user_email")
    user_password = data.get("user_password")

    conn = get_db_connection()
    cursor = conn.cursor(dictionary=True)

    cursor.execute("SELECT * FROM user WHERE user_email = %s", (user_email,))
    user = cursor.fetchone()

    cursor.close()
    conn.close()

    if user and bcrypt.checkpw(user_password.encode(), user["user_password"].encode()):
        del user["user_password"]
        return jsonify({"status": "success", "user": user})
    else:
        return jsonify({"status": "fail", "message": "Invalid credentials"}), 401

#Get Courses
@app.route("/get_courses", methods=["GET"])
def get_courses():
    conn = get_db_connection()
    cursor = conn.cursor(dictionary=True)

    cursor.execute("SELECT course_id, course_name FROM course")
    courses = cursor.fetchall()

    cursor.close()
    conn.close()
    return jsonify({"courses": courses})

#Get Words by Course
@app.route("/get_words/<int:course_id>", methods=["GET"])
def get_words(course_id):
    try:
        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        query = """ SELECT model.model_id, word.word_text,model_name
            FROM word
            JOIN model ON word.model_id = model.model_id
            WHERE word.course_id = %s
            """
        cursor.execute(query, (course_id,))
        word_rows = cursor.fetchall()

        cursor.close()
        conn.close()

        return jsonify({"words": word_rows})
    except Exception as e:
        return jsonify({"status": "fail", "message": str(e)}), 500

#Get Sentences by Course
@app.route("/get_sentences/<int:course_id>", methods=["GET"])
def get_sentences(course_id):
    try:
        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        query = """
            SELECT sign_grammar, chinese_grammar, sentence_order
            FROM sentence
            WHERE course_id = %s
            ORDER BY sentence_order
        """
        cursor.execute(query, (course_id,))
        sentence_rows = cursor.fetchall()

        cursor.close()
        conn.close()

        return jsonify({"status": "success", "sentences": sentence_rows})
    except Exception as e:
        return jsonify({"status": "fail", "message": str(e)}), 500

#Get Quiz Words + Animation Clips
@app.route("/api/quiz_words/<int:course_id>", methods=["GET"])
def get_quiz_words(course_id):
    try:
        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        query = """
        SELECT word.word_id, word.word_text AS word, model.model_name AS clip_name
        FROM word
        LEFT JOIN model ON word.model_id = model.model_id   
        WHERE word.course_id = %s
        """
        cursor.execute(query, (course_id,))
        result = cursor.fetchall()

        cursor.close()
        conn.close()

        return jsonify({"items": result})
    except Exception as e:
        return jsonify({"error": str(e)}), 500

#Get Favorite Words by User
@app.route("/get_favorite_words/<int:user_id>", methods=["GET"])
def get_favorite_words(user_id):
    try:
        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        query = """
             SELECT word.word_id, word.word_text,model.model_name
            FROM word
            JOIN model_favorite mf ON word.word_id = mf.model_id
            JOIN model ON word.model_id = model.model_id
            WHERE mf.user_id = %s
        """
        cursor.execute(query, (user_id,))
        words = cursor.fetchall()

        cursor.close()
        conn.close()

        return jsonify({"words": words})
    except Exception as e:
        return jsonify({"status": "fail", "message": str(e)}), 500

#收藏介面刪除已收藏單字
@app.route("/unfavorite_word", methods=["POST"])
def unfavorite_word():
    try:
        user_id = request.json.get("user_id")
        word_id = request.json.get("word_id")

        conn = get_db_connection()
        cursor = conn.cursor()

        delete_query = """
            DELETE FROM model_favorite
            WHERE user_id = %s AND model_id = %s
        """
        cursor.execute(delete_query, (user_id, word_id))
        conn.commit()

        cursor.close()
        conn.close()

        return jsonify({"status": "success"})
    except Exception as e:
        print("/unfavorite_word 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500

#單字載入介面收藏功能
@app.route("/favorite_word", methods=["POST"])
def favorite_word():
    try:
        data = request.get_json()
        user_id = data.get("user_id")
        model_id = data.get("model_id")

        if user_id is None or model_id is None:
            return jsonify({"status": "fail", "message": "Missing user_id or model_id"}), 400

        conn = get_db_connection()
        cursor = conn.cursor()

        check_query = """
            SELECT 1 FROM model_favorite WHERE user_id = %s AND model_id = %s
        """
        cursor.execute(check_query, (user_id, model_id))
        exists = cursor.fetchone()

        if exists:
            cursor.close()
            conn.close()
            return jsonify({"status": "success", "message": "Already favorited"})

        insert_query = """
            INSERT INTO model_favorite (user_id, model_id)
            VALUES (%s, %s)
        """
        cursor.execute(insert_query, (user_id, model_id))
        conn.commit()

        cursor.close()
        conn.close()

        return jsonify({"status": "success"})
    except Exception as e:
        print("/favorite_word 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500




@app.route("/is_favorited/<int:user_id>/<int:model_id>", methods=["GET"])
def is_favorited(user_id, model_id):
    try:
        conn = get_db_connection()
        cursor = conn.cursor()

        query = """
            SELECT 1 FROM model_favorite
            WHERE user_id = %s AND model_id = %s
        """
        cursor.execute(query, (user_id, model_id))
        result = cursor.fetchone()

        cursor.close()
        conn.close()

        return jsonify({"favorited": "true" if result else "false"})
    except Exception as e:
        print("/is_favorited 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500

@app.route("/unfavorite_word_from_word", methods=["POST"])
def unfavorite_word_from_vocab():
    try:
        data = request.get_json()
        user_id = data.get("user_id")
        model_id = data.get("model_id")

        conn = get_db_connection()
        cursor = conn.cursor()

        delete_query = """
            DELETE FROM model_favorite
            WHERE user_id = %s AND model_id = %s
        """
        cursor.execute(delete_query, (user_id, model_id))
        conn.commit()

        cursor.close()
        conn.close()

        return jsonify({"status": "success"})
    except Exception as e:
        print("/unfavorite_word_from_word 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500

#單字載入介面收藏功能結束
import os
import cv2
import numpy as np
import mediapipe as mp
import tensorflow as tf
from flask import Flask, request, jsonify



# === 模型與資料夾路徑 ===
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
MODEL_PATH = os.path.join(BASE_DIR, '../../DetectionPackege/GRU_model_v1.h5')
SIGN_FOLDER = os.path.join(BASE_DIR, '../../DetectionPackege/sign')

# === 載入模型與手勢標籤 ===
model = tf.keras.models.load_model(MODEL_PATH)
gesture_names = sorted([
    name for name in os.listdir(SIGN_FOLDER)
    if os.path.isdir(os.path.join(SIGN_FOLDER, name))
])


# === Mediapipe 設定 ===
mp_holistic = mp.solutions.holistic
holistic = mp_holistic.Holistic(static_image_mode=False, model_complexity=1, smooth_landmarks=True)

sequence_length = 30
buffer = []

# ✅ 新版 landmark 處理函數（與你成功的腳本相同）
def get_relative_landmarks(results, frame_width, frame_height):
    """Extract and normalize landmarks. Fill 0 if missing hand landmarks."""
    if results.pose_landmarks:
        landmarks = []

        # Nose 當基準點
        nose = results.pose_landmarks.landmark[0]
        origin_x, origin_y, origin_z = nose.x, nose.y, nose.z

        # Pose: 0 + 11~22 共 13 個點
        for idx in [0] + list(range(11, 23)):
            landmark = results.pose_landmarks.landmark[idx]
            landmarks.extend([
                landmark.x - origin_x,
                landmark.y - origin_y,
                landmark.z - origin_z
            ])

        # Left hand
        if results.left_hand_landmarks:
            for lm in results.left_hand_landmarks.landmark:
                landmarks.extend([lm.x - origin_x, lm.y - origin_y, lm.z - origin_z])
        else:
            landmarks.extend([0.0] * 63)

        # Right hand
        if results.right_hand_landmarks:
            for lm in results.right_hand_landmarks.landmark:
                landmarks.extend([lm.x - origin_x, lm.y - origin_y, lm.z - origin_z])
        else:
            landmarks.extend([0.0] * 63)

        return landmarks
    else:
        # 如果沒有偵測到 pose，回傳 165 個 0
        return [0.0] * 165

# ✅ 預測 API
@app.route('/api/predict', methods=['POST'])
def predict():
    global buffer

    file = request.files.get('image')
    if not file:
        return jsonify({"message": "No image provided"}), 400

    img_bytes = file.read()
    npimg = np.frombuffer(img_bytes, np.uint8)
    frame = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    # Mediapipe 處理
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = holistic.process(frame_rgb)

    landmarks = get_relative_landmarks(results, frame.shape[1], frame.shape[0])

    # ➕ 滑動窗口
    buffer.append(landmarks)
    if len(buffer) > sequence_length:
        buffer.pop(0)

    # ❗只有當長度達標時才預測
    if len(buffer) == sequence_length:
        input_sequence = np.expand_dims(np.array(buffer), axis=0)
        prediction = model.predict(input_sequence)
        predicted_class = np.argmax(prediction)
        confidence = prediction[0][predicted_class]

        if confidence >= 0.99:
            return gesture_names[predicted_class], 200
        else:
            return "無法辨識出的手勢", 200
    else:
        return "Waiting", 200

# 更改使用者資料
@app.route("/update_profile", methods=["POST"])
def update_profile():
    data = request.get_json()
    user_id = data.get("user_id")
    new_name = data.get("user_name")
    new_email = data.get("user_email")
    old_password = data.get("old_password")
    new_password = data.get("new_password")

    if not user_id or not old_password:
        return jsonify({"status": "fail", "message": "Missing required fields"}), 400

    conn = get_db_connection()
    cursor = conn.cursor()

    try:
        # 查詢目前的密碼
        cursor.execute("SELECT user_password FROM user WHERE user_id = %s", (user_id,))
        result = cursor.fetchone()
        if not result:
            return jsonify({"status": "fail", "message": "User not found"}), 404

        hashed_pw = result[0]
        if not bcrypt.checkpw(old_password.encode(), hashed_pw.encode()):
            return jsonify({"status": "fail", "message": "Incorrect current password"}), 403

        # 如果有提供新密碼，就更新
        if new_password:
            new_hashed = bcrypt.hashpw(new_password.encode(), bcrypt.gensalt()).decode()
            cursor.execute("UPDATE user SET user_password = %s WHERE user_id = %s", (new_hashed, user_id))

        # 更新姓名與信箱（如有）
        if new_name:
            cursor.execute("UPDATE user SET user_name = %s WHERE user_id = %s", (new_name, user_id))
        if new_email:
            cursor.execute("UPDATE user SET user_email = %s WHERE user_id = %s", (new_email, user_id))

        conn.commit()
        return jsonify({"status": "success", "message": "Profile updated successfully"})

    except Exception as e:
        return jsonify({"status": "fail", "message": str(e)}), 500
    finally:
        cursor.close()
        conn.close()


#chat bot 輸入框送出
CLAUDE_API_URL = "https://api.anthropic.com/v1/messages"
CLAUDE_API_KEY = "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"  # Ensure this is set in your environment variables.
import requests
@app.route('/api/process_input', methods=['POST'])
def process_input():
    try:
        # Step 1: Parse the JSON from the request
        data = request.get_json(force=True)
        print(f"Request headers: {request.headers}")
        print(f"Request body: {request.data.decode('utf-8')}")

        if not data or 'user_input' not in data:
            print("Error: Missing 'user_input' in request data.")
            return jsonify({'error': "Invalid input: 'user_input' is required"}), 400

        user_input = data['user_input']
        print(f"User input received: {user_input}")

        # Step 2: Prepare the request to Claude API
        if not CLAUDE_API_KEY:
            print("Error: CLAUDE_API_KEY not set.")
            return jsonify({'error': "Claude API key is not configured"}), 500

        headers = {
            "x-api-key": CLAUDE_API_KEY,
            "anthropic-version": "2023-06-01",
            "content-type": "application/json"
        }

        payload = {
            "model": "claude-3-opus-20240229",
            "max_tokens": 1024,
            "system": (
                "你是一位專業台灣手語老師AI機器人，負責回答使用者對台灣手語相關的問題，並且熟知 SignUp 手語教學系統的一切運作。\n\n"

                "🔤【單字資料】\n"
                "SignUp 手語教學系統目前提供以下單字：\n"
                "(101:起床, 102:早安, 103:刷牙, 104:洗臉, 105:穿衣服, 106:開心, 107:讀書, 108:去, 109:你, 110:我,\n"
                "201:老師, 202:好, 203:同學, 204:畫畫, 205:運動, 206:學, 207:多, 208:他,\n"
                "301:朋友, 302:一起, 303:玩, 304:聊天, 305:跑步, 306:我們兩個, 307:你們,\n"
                "401:書包, 402:大, 403:筆, 404:橡皮擦, 405:書, 406:水壺, 407:一, 408:有)\n"
                "代碼中第一碼是課程編號，後兩碼是單字號碼，例如 208 表示第二課第八個單字「他」。\n\n"

                "🧪【互動測驗規則】\n"
                "當使用者提到「測驗」「考試」「考我」「小考」等詞，請隨機從單字清單中抽出一個單字，並提示使用者：\n"
                "👉『請比出單字：<抽到的單字>』\n"
                "接著請在訊息中加入一行以 [target_word] <單字> 開頭，例如：\n"
                "[target_word] 起床\n"
                "這樣系統才能辨識目前的目標單字。\n"
                "❗請務必在收到系統回傳前不要繼續下一題。\n\n"

                "📥 若收到：\n"
                "[system] result: correct\n"
                "✅ 表示使用者比對成功，請用鼓勵語氣稱讚他，並詢問是否要繼續下一題。\n\n"

                "[system] result: incorrect\n"
                "❌ 表示比錯了，請鼓勵使用者再嘗試一次，**不要更換題目。**\n\n"

                "📘【SignUp 系統功能總覽】\n"
                "一、登入/註冊系統：\n"
                "用戶可透過「註冊」建立帳號，或輸入帳號密碼進行登入。\n\n"

                "二、學習系統模組：\n"
                "1 登入後可進入主介面，選擇：課程學習、互動練習、測驗、收藏、設定等功能。\n"
                "2 課程學習：\n"
                "  - 點選「課程學習」可選擇想學的課程。\n"
                "  - 可選擇「單字學習」：提供單字列表與3D模型影片播放。\n"
                "    ・可旋轉視角、調整播放速度、暫停、開啟鏡頭即時實作。\n"
                "  - 可選擇「課文學習」：顯示中文語法與手語語法，點句子可播放整句手語。\n\n"

                "3 互動練習模組：\n"
                "與手語聊天機器人互動，可問手語相關問題，機器人會出題讓使用者即時比手語並辨識，也可詢問系統操作相關問題。\n\n"

                "4 牛刀小試測驗模組：\n"
                "  - 可選擇測驗課程或題型。\n"
                "  - 題型一：選擇題，觀看手語影片選正確意思。\n"
                "  - 題型二：實作題，做出手語由系統辨識。\n"
                "  - 測驗完會顯示測驗成績單並且可藉由點選錯誤的單詞直接開始複習\n"
                "  - 可從測驗介面點選測驗紀錄查看之前的測驗成績單，也可點選錯誤單字進行複習\n\n"
                

                "5 收藏模組：\n"
                "顯示使用者收藏的單字，可取消收藏或播放學習。\n\n"

                "6 設定模組：\n"
                "  - 可設定鏡頭來源、學習提醒頻率。\n"
                "  - 從選擇鏡頭的下拉選單選擇想用的鏡頭。\n"
                "  - 以hh:mm的格式輸入學習提醒頻率(範例20:45 每天的20:45會通知你來學習手語喔)。\n"
                "  - 修改使用者密碼，輸入新密碼送出即可。\n\n"
               

                "📣【回答風格】\n"
                "使用者可能會問：「怎麼開始學課程？」「可以改密碼嗎？」「互動練習怎麼用？」等。\n"
                "請用以下格式回答：\n\n"
                "👌 當然可以～流程如下：\n"
                "1 ...\n"
                "2 ...\n"
                "3 ...\n"
                "結尾可主動問：想看其他模組怎麼操作嗎？我可以幫你！\n\n"

                "🚫 不要的行為：\n"
                "❌ 不要主動幫使用者切換頁面（你只是語音/對話助理，不是操作指令）。\n"
                "❌ 不要胡亂編造尚未實作的功能，要據實說明。\n"
                "❌ 不要提供與 SignUp 系統無關的建議。"
            ),
            "messages": [
                {
                    "role": "user",
                    "content": user_input
                }
                # 如果需要加入比對結果，動態添加：
                # {"role": "user", "content": "[system] result: correct"}
            ]
        }





        print("Sending request to Claude API...")
        print(f"Payload: {payload}")

        # Step 3: Send the request to Claude
        response = requests.post(CLAUDE_API_URL, headers=headers, json=payload)

        if response.status_code != 200:
            print(f"Claude API Error: {response.status_code} - {response.text}")
            return jsonify({
                'error': "Claude API request failed",
                'status_code': response.status_code,
                'response': response.text
            }), 500

        # Extract the AI's response
        reply = response.json()
        print(f"Claude API response: {reply}")

        # New (correct)
        if reply.get('content') and isinstance(reply['content'], list):
            ai_message = reply['content'][0].get('text', 'Claude 回傳內容格式錯誤')
        else:
            ai_message = "Claude 回傳內容格式錯誤"
        print(f"Claude content structure: {reply.get('content')}")

        if not ai_message:
            ai_message = "No response from Claude."  # Fallback if no valid response

        return jsonify({'response': ai_message}), 200

    except Exception as e:
        # Catch all unexpected errors
        print(f"Unexpected server error: {str(e)}")
        return jsonify({'error': "Internal server error", 'details': str(e)}), 500

# 獲取使用者測驗紀錄
@app.route("/get_user_quiz_records", methods=["GET"])
def get_user_quiz_records():
    try:
        user_id = request.args.get("user_id")
        if not user_id:
            return jsonify({"status": "fail", "message": "缺少 user_id"}), 400

        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        query = """ 
            SELECT test_id, test_score, test_time, course_id
            FROM test_history 
            WHERE user_id = %s
            ORDER BY test_time DESC
        """
        cursor.execute(query, (user_id,))
        results = cursor.fetchall()

        # 轉換為 Unity 需要的格式
        for i, row in enumerate(results):
            row["quizName"] = f"第{row['course_id']}課測驗" 
            row["correctCount"] = row.pop("test_score")
            row["dateTime"] = row.pop("test_time").strftime("%m-%d %H:%M")

        cursor.close()
        conn.close()

        return jsonify(results)
    except Exception as e:
        print("/get_user_quiz_records 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500

# 儲存錯誤單字
@app.route("/save_wrong_answers", methods=["POST"])
def save_wrong_answers():
    try:
        data = request.get_json()
        user_id = data.get("user_id")
        test_score = data.get("test_score")
        course_id = data.get("course_id")  # ⬅️ 新增
        wrong_word_ids = data.get("wrong_word_ids", [])

        if not user_id or test_score is None or course_id is None:
            return jsonify({"status": "fail", "message": "缺少 user_id、test_score 或 course_id"}), 400

        conn = get_db_connection()
        cursor = conn.cursor()

        # 新增 test_history 並取得 test_id
        insert_test_sql = """
            INSERT INTO test_history (user_id, test_score, test_time, course_id)
            VALUES (%s, %s, NOW(), %s)
        """
        cursor.execute(insert_test_sql, (user_id, test_score, course_id))
        test_id = cursor.lastrowid

        # 批次插入 wrong_history
        if wrong_word_ids:
            insert_wrong_sql = """
                INSERT INTO wrong_history (test_id, word_id)
                VALUES (%s, %s)
            """
            wrong_data = [(test_id, word_id) for word_id in wrong_word_ids]
            cursor.executemany(insert_wrong_sql, wrong_data)

        conn.commit()
        cursor.close()
        conn.close()

        return jsonify({"status": "success", "message": "測驗結果已儲存"})

    except Exception as e:
        print("/save_wrong_answers 錯誤：", e)
        return jsonify({"status": "fail", "message": str(e)}), 500


# 獲取錯誤單字列表
@app.route('/get_wrong_words_by_test_id')
def get_wrong_words_by_test_id():
    test_id = request.args.get("test_id")

    try:
        conn = get_db_connection()
        cursor = conn.cursor(dictionary=True)

        sql = """
            SELECT w.word_text, m.model_name
            FROM wrong_history wh
            JOIN word w ON wh.word_id = w.word_id
            JOIN model m ON w.model_id = m.model_id
            WHERE wh.test_id = %s
        """
        cursor.execute(sql, (test_id,))
        results = cursor.fetchall()

        return jsonify({"wrong_words": results})

    except Exception as e:
        print("/get_wrong_words_by_test_id 錯誤：", e)
        return jsonify({"error": str(e)}), 500



if __name__ == "__main__":
    app.run(debug=True)
