import requests
from bs4 import BeautifulSoup
import json
import os
import time

# ========== 設定區 ==========
START_YEAR = 2024
END_YEAR = 2030
OUTPUT_FILE = "new_words.json"
LIST_URL_TEMPLATE = "https://newtsl.taslifamily.org/年度檢索/{}詞彙手語"
BASE_WORD_URL = "https://newtsl.taslifamily.org"
HEADERS = {"User-Agent": "Mozilla/5.0"}

# ========== 載入舊資料並建立已存在組合的集合 ==========
def load_existing_data_and_keys():
    existing_data = {}
    existing_keys = set()
    if os.path.exists(OUTPUT_FILE):
        with open(OUTPUT_FILE, "r", encoding="utf-8") as f:
            existing_data = json.load(f)
            for year_data in existing_data.values():
                for word in year_data:
                    key = f"{word['title']}|{word['url']}"
                    existing_keys.add(key)
    return existing_data, existing_keys

# ========== 解析 YouTube ==========
def extract_youtube_link(detail_url):
    try:
        res = requests.get(detail_url, headers=HEADERS)
        res.raise_for_status()
    except Exception as e:
        print(f"⚠️ 無法開啟單字頁面：{detail_url}，錯誤：{e}")
        return None

    soup = BeautifulSoup(res.text, "html.parser")
    iframe = soup.find("iframe", src=lambda x: x and "youtube.com/embed/" in x)

    if iframe:
        embed_url = iframe["src"]
        if "embed/" in embed_url:
            video_id = embed_url.split("embed/")[-1].split("?")[0]
            return f"https://www.youtube.com/watch?v={video_id}"
    return None

# ========== 爬蟲 ==========
def crawl_words_by_year(start_year, end_year, existing_data, existing_keys):
    all_words = existing_data.copy()

    for year in range(start_year, end_year + 1):
        print(f"🔍 正在爬取 {year} 年資料...")
        list_url = LIST_URL_TEMPLATE.format(year)
        try:
            res = requests.get(list_url, headers=HEADERS)
            res.raise_for_status()
        except Exception as e:
            print(f"❌ 錯誤：無法連線 {list_url}：{e}")
            continue

        soup = BeautifulSoup(res.text, "html.parser")
        word_links = soup.select("a")
        year_words = all_words.get(str(year), [])

        for a in word_links:
            href = a.get("href")
            title = a.get_text(strip=True)
            if href and "/新詞彙/" in href and title:
                full_url = BASE_WORD_URL + href if href.startswith("/") else href
                key = f"{title}|{full_url}"
                if key in existing_keys:
                    continue  # 自動跳過重複單字

                print(f"📝 處理單字：{title}")
                youtube_url = extract_youtube_link(full_url)

                year_words.append({
                    "title": title,
                    "url": full_url,
                    "youtube": youtube_url or ""
                })

                existing_keys.add(key)
                time.sleep(0.3)

        all_words[str(year)] = year_words
        print(f"✅ {year} 年共取得 {len(year_words)} 筆單字（含舊資料）")

    return all_words

# ========== 儲存 JSON ==========
def save_to_json(data, filename):
    with open(filename, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"💾 已儲存至 {filename}")

if __name__ == "__main__":
    existing_data, existing_keys = load_existing_data_and_keys()
    new_data = crawl_words_by_year(START_YEAR, END_YEAR, existing_data, existing_keys)
    save_to_json(new_data, OUTPUT_FILE)
